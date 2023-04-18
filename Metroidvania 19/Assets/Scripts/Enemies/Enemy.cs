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
    private Vector2 playerDirection;
    private float playerDistance;
    private RaycastHit2D lineOfSight;

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
        scaleSize = transform.localScale.x;

        // Sets up enemy at first patrol position, and sets A* Path target as well
        transform.position = patrolPositions[0].transform.position;
        aiDestSetter.target = patrolPositions[0];
        aiPath.maxSpeed = patrolSpeed;

        StartCoroutine(findLineOfSight());
    }

    // Update is called once per frame
    void Update() {

        // Update calls for figuring out patrol/attack patterns
        if (onPatrol) { // In the midst of a patrol
            if (playerDistance < detectDistance // Close enough
            && (lineOfSight.collider.tag == "Player" || lineOfSight.collider.tag == "PlayerSegment")) { // Can see player
                StartCoroutine(BeginAttack());
            }
            Patrol();
        }
        else if (onAttack && (playerDistance > detectDistance)) { // In the midst of attacking & player is too far
            StartCoroutine(BeginPatrol());
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
        enemyAnimator.SetBool("playerSpotted", false);
        StartCoroutine(findLineOfSight());
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
        enemyAnimator.SetBool("playerSpotted", true);
        //Debug.Log("ATTACK!");

        aiPath.maxSpeed = attackSpeed;
        if (enemyType == "Ranged") {
            StartCoroutine(FireAnim());
        }
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

    // Function that spaces out how much time it takes to fire each enemy bullet.
    private IEnumerator FireAnim() {
        yield return new WaitForEndOfFrame();
        while (onAttack) {
            Fire();
            yield return new WaitForSeconds(3f);
        }
    }

    // Function used to fire projectiles at the player
    private void Fire() {
        playerDirection = player.transform.position - transform.position;
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

    private IEnumerator findLineOfSight() {
        yield return new WaitForEndOfFrame();
        while (onPatrol) {
            playerDirection = player.transform.position - transform.position;
            playerDistance = (playerDirection).magnitude;
            lineOfSight = Physics2D.Raycast(transform.position, playerDirection, 100f);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
