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
    public int damagePower;
    public float detectDistance;
    public float patrolSpeed;
    public float attackSpeed;
    public float kickbackStrength;
    public float recoveryDelay;
    public List<Transform> patrolPositions;
    public GameObject bulletObject;

    // Private variables
    private int patrolIndex;
    private int patrolDirection;
    private bool canSwapTarget;
    private bool onPatrol;
    private bool onAttack;
    private bool readyToFire;
    private float scaleSize;
    private GameObject player;

    // Component variables
    private Seeker seeker;
    private Rigidbody2D rb;
    private AIPath aiPath;
    private AIDestinationSetter aiDestSetter;

    [SerializeField]
    private Animator enemyAnimator;

    void Awake() {
    
        // Set defaults for public variables
        player = (player == null) ? GameObject.FindGameObjectWithTag("Player") : player;
        enemyType = (enemyType == null) ? "Melee" : enemyType;
        health = (health == 0) ? 100 : health;
        detectDistance = (detectDistance == 0f) ? 10f : detectDistance;
        patrolSpeed = (patrolSpeed == 0f) ? 3f : patrolSpeed;
        attackSpeed = (attackSpeed == 0f && enemyType != "Ranged") ? 5f : attackSpeed;
        kickbackStrength = (kickbackStrength == 0f) ? 3f : kickbackStrength;
        recoveryDelay = (recoveryDelay == 0f) ? 2f : recoveryDelay;

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
        readyToFire = true;
        scaleSize = transform.localScale.x;

        // Sets up enemy at first patrol position, and sets A* Path target as well
        transform.position = patrolPositions[0].transform.position;
        aiDestSetter.target = patrolPositions[0];
        aiPath.maxSpeed = patrolSpeed;
    }

    // Update is called once per frame
    void Update() {
    
        // Temp variables
        Vector2 playerDirection = player.transform.position - transform.position;
        float playerDistance = (playerDirection).magnitude;

        RaycastHit2D lineOfSight = Physics2D.Raycast(transform.position, playerDirection, 100f);
        //Debug.DrawRay(transform.position,playerDirection * 100f, Color.red); // Uncomment to debug line of sight

        // Update calles for beginning patrol or attack
        if (playerDistance < detectDistance // Close enough
            && (lineOfSight.collider.tag == "Player" || lineOfSight.collider.tag == "PlayerSegment") // Can see player
            && onPatrol) // In the midst of a patrol
        { 
            StartCoroutine(BeginAttack());
        }
        else if (playerDistance > detectDistance // Far enough
                 && onAttack) // In the midst of attacking
        { 
            StartCoroutine(BeginPatrol());
        }

        // Update calls while on patrol or attack
        if (onPatrol) {
            Patrol();
            enemyAnimator.SetBool("playerSpotted", false);
        }
        else if (onAttack) {
            Attack();
            enemyAnimator.SetBool("playerSpotted", true);
        }
        
        // Flips the sprite horizontally based on what direction it wants to travel
        if (aiPath.enabled) {
            if (aiPath.desiredVelocity.x < 0) {
                transform.localScale = new Vector3(-1, 1, 1) * scaleSize;
            } else {
                transform.localScale = new Vector3(1, 1, 1) * scaleSize;
            }
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
        for (int i = 0; i < patrolPositions.Count; i++) {
            float newDistance = (player.transform.position - patrolPositions[i].position).magnitude;
            if (newDistance < closestPatrolDistance) {
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
        onAttack = true;
        //Debug.Log("ATTACK!");
    }

    // Function that's constantly called while on patrol mode.
    private void Patrol()
    {
        if (aiPath.reachedEndOfPath // Arrived at a patrol point
            && patrolPositions.Count > 1) // There's a patrol path instead of just a single position
        {

            if (patrolIndex == 0) { // If at the start
                aiPath.slowdownDistance = patrolSpeed; // Slowdown on approach
                patrolDirection = 1; // Going from first to last
            }
            else if (patrolIndex == patrolPositions.Count - 1) { // If at the end
                aiPath.slowdownDistance = patrolSpeed; // Slowdown on approach
                patrolDirection = -1; // Going from last to first
            }
            else { // If at midpoint of patrol path
                aiPath.slowdownDistance = 0; // Remove slowdown on approach
            }

            // If we're ready to swap targets to the next patrol point.
            if (canSwapTarget) {
                patrolIndex += patrolDirection;
                aiDestSetter.target = patrolPositions[patrolIndex];
                canSwapTarget = false;
                StartCoroutine(SwapTargetTimer());
            }
        }
    }

    // Function used to prevent Update from calling twice while swapping targets on patrol
    private IEnumerator SwapTargetTimer() {
        yield return new WaitForSeconds(0.1f);
        canSwapTarget = true;
    }
    
    // Function that's constantly called while on attack mode.
    private void Attack() {
        aiPath.maxSpeed = attackSpeed;
        if (enemyType == "Ranged" && readyToFire) {
            StartCoroutine(FireAnim());
        }
    }

    // Function that spaces out how much time it takes to fire each enemy bullet.
    private IEnumerator FireAnim() {
        readyToFire = false;
        Fire();
        yield return new WaitForSeconds(3f);
        readyToFire = true;
    }

    // Function used to fire projectiles at the plaer
    private void Fire() {
        Vector2 playerDirection = player.transform.position - transform.position;
        GameObject bulletInstance = Instantiate(bulletObject, transform.position, transform.rotation);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = playerDirection;
    }

    // Function that's called when the enemy takes damage
    public bool Damage(int amount) {
        if (health - amount <= 0) {
            return true; // When this function returns true, that output is used to determine death.
        }

        health -= amount;
        StartCoroutine(damageIndicator());
        return false;
    }

    // Function that's called when the enemy dies
    public void Death() {
        GetComponent<Dropper>().Drop(true); // Drops health collectables
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Player" || other.collider.tag == "PlayerSegment") { // Collides with player
            StartCoroutine(contactAnim(other));
            if (other.gameObject.GetComponent<IDamageable>().Damage(damagePower)) { // If damage on other object is enough to die
                other.gameObject.GetComponent<IDamageable>().Death(); // Call that object's death function
            }
        }
    }

    // Function used to add a bit of kickback between the colliding objects
    private IEnumerator contactAnim(Collision2D player) {
        yield return new WaitForEndOfFrame();
        Vector3 forceDir = this.transform.position - player.transform.position;

        aiPath.enabled = false; // Briefly turns off the A* path to add RB force
        rb.AddForce(forceDir * kickbackStrength, ForceMode2D.Impulse); // Adds force to enemy
        player.gameObject.GetComponent<Rigidbody2D>().AddForce(-forceDir * kickbackStrength, ForceMode2D.Impulse); // Adds force to player
        yield return new WaitForSeconds(recoveryDelay);
        aiPath.enabled = true;
    }

    // Function used to highlight enemy in red briefly, to visually indicate damage dealt
    private IEnumerator damageIndicator() {
        yield return new WaitForEndOfFrame();
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
