using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get { return health; } set { health = value; } }

    // Public variables
    public string enemyType;
    public int health;
    public float detectDistance;
    public float patrolSpeed;
    public float attackSpeed;
    public List<Transform> patrolPositions;

    // Private variables
    private int patrolIndex;
    private int patrolDirection;
    private bool canSwapTarget;
    private bool onPatrol;
    private bool onAttack;
    private GameObject player;

    // Component variables
    private Seeker seeker;
    private Rigidbody2D rb;
    private AIPath aiPath;
    private AIDestinationSetter aiDestSetter;

    void Awake()
    {
        // Set defaults for public variables
        player = (player == null) ? GameObject.FindGameObjectWithTag("Player") : player;
        enemyType = (enemyType == null) ? "Melee" : enemyType;
        health = (health == 0) ? 100 : health;
        detectDistance = (detectDistance == 0f) ? 10f : detectDistance;
        patrolSpeed = (patrolSpeed == 0f) ? 3f : patrolSpeed;
        attackSpeed = (attackSpeed == 0f) ? 5f : attackSpeed;

        // If no patrol positions declared, make one
        if (patrolPositions.Count == 0)
        {
            GameObject g = new GameObject("Default Patrol Spot");
            g.transform.parent = transform.parent;
            g.transform.position = transform.position;
            patrolPositions.Add(g.transform);
        }

        // Collect component variables
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        aiDestSetter = GetComponent<AIDestinationSetter>();

        // Sets private variables
        player = GameObject.FindGameObjectWithTag("Player");
        patrolIndex = 0;
        patrolDirection = 1;
        canSwapTarget = true;
        onPatrol = true;
        onAttack = false;

        // Sets up enemy at first patrol position, and sets A* Path target as well
        transform.position = patrolPositions[0].transform.position;
        aiDestSetter.target = patrolPositions[0];
        aiPath.maxSpeed = patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Temp variables
        Vector2 playerDirection = player.transform.position - transform.position;
        float playerDistance = (playerDirection).magnitude;

        RaycastHit2D lineOfSight = Physics2D.Raycast(transform.position, playerDirection, 100f);
        //Debug.DrawRay(transform.position,playerDirection * 100f, Color.red); // Uncomment to debug line of sight

        if (playerDistance < detectDistance // Close enough
            && lineOfSight.collider.tag == "Player" // Can see player
            && onPatrol)
        { // In the midst of a patrol
            StartCoroutine(BeginAttack());
        }
        else if (playerDistance > detectDistance // Far enough
                 && onAttack)
        { // In the midst of attacking
            StartCoroutine(BeginPatrol());
        }

        // Update calls for patrol and attack
        if (onPatrol)
        {
            Patrol();
        }
        else if (onAttack)
        {
            Attack();
        }
    }

    // Function that's called once to transition from attack to patrol
    private IEnumerator BeginPatrol()
    {
        yield return new WaitForEndOfFrame();
        onAttack = false;

        float closestPatrolDistance = 99999f;
        int closestPatrolIndex = 0;

        // For loop that finds the closest patrol position
        for (int i = 0; i < patrolPositions.Count; i++)
        {
            float newDistance = (player.transform.position - patrolPositions[i].position).magnitude;
            if (newDistance < closestPatrolDistance)
            {
                closestPatrolDistance = newDistance;
                closestPatrolIndex = i;
            }
        }

        // Sets speed back to patrolSpeed and heads back into patrol
        patrolIndex = closestPatrolIndex;
        aiPath.maxSpeed = patrolSpeed;
        aiDestSetter.target = patrolPositions[patrolIndex];
        //Debug.Log("Heading back to patrol in spot " + patrolIndex);

        // Wait for a bit before setting onPatrol true again
        yield return new WaitForSeconds(1f);
        onPatrol = true;
    }

    // Function that's called once to transition from patrol to attack
    private IEnumerator BeginAttack()
    {
        yield return new WaitForEndOfFrame();
        onPatrol = false;

        // Enemy stops for a second and targets player.
        aiPath.maxSpeed = 0f;
        aiDestSetter.target = player.transform;
        //Debug.Log("DETECTED!");

        // Wait for a bit before setting onAttack true again
        yield return new WaitForSeconds(1f);
        //Debug.LogWarning("ATTACK!");
        onAttack = true;
    }

    // Function that's constantly called while on patrol mode.
    private void Patrol()
    {
        if (aiPath.reachedEndOfPath // Arrived at a patrol point
            && patrolPositions.Count > 1)
        { // There's a patrol path instead of just a single position

            if (patrolIndex == 0)
            { // If at the start
                aiPath.slowdownDistance = patrolSpeed; // Slowdown on approach
                patrolDirection = 1;
            }
            else if (patrolIndex == patrolPositions.Count - 1)
            { // If at the end
                aiPath.slowdownDistance = patrolSpeed; // Slowdown on approach
                patrolDirection = -1;
            }
            else
            { // If at midpoint of patrol path
                aiPath.slowdownDistance = 0; // Remove slowdown on approach
            }

            // If we're ready to swap targets to the next patrol point.
            if (canSwapTarget)
            {
                patrolIndex += patrolDirection;
                aiDestSetter.target = patrolPositions[patrolIndex];
                canSwapTarget = false;
                StartCoroutine(SwapTargetTimer());
            }
        }
    }

    // Function used to prevent Update from calling twice while swapping targets on patrol
    private IEnumerator SwapTargetTimer()
    {
        yield return new WaitForSeconds(0.1f);
        canSwapTarget = true;
    }

    // Function that's constantly called while on attack mode.
    private void Attack()
    {
        switch (enemyType)
        {
            case ("Melee"):
                aiPath.maxSpeed = attackSpeed;
                //Debug.Log("chaaaaaarge!");
                // Bash into player
                break;
            case ("Ranged"):
                aiPath.maxSpeed = attackSpeed;
                //Debug.Log("pew pew pew");
                // Fire Bullets
                break;
            default:
                //Debug.Log("idk what to do");
                // Do Nothing
                break;
        }
    }

    public bool Damage(int amount)
    {
        if (health - amount <= 0)
            return true;

        health -= amount;
        return false;
    }

    public void Death()
    {
        GetComponent<Dropper>().Drop(true);
    }
}
