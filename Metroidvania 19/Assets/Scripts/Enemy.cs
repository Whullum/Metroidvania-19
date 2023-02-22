using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public string enemyType;
    public float health;
    public float detectDistance;
    public float patrolSpeed;
    public float attackSpeed;
    public List<Transform> patrolPositions;
    public GameObject player;

    private int patrolIndex;
    private int patrolDirection;
    private bool canSwapTarget;
    private bool onPatrol;
    private bool onAttack;

    private Seeker seeker;
    private Rigidbody2D rb;
    private AIPath aiPath;
    private AIDestinationSetter aiDestSetter;

    void Awake()
    {
        if (patrolPositions.Count == 0) { 
            GameObject g = new GameObject("Default Patrol Spot");
            g.transform.parent = transform.parent;
            g.transform.position = transform.position;
            patrolPositions.Add(g.transform);
        }

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        aiDestSetter = GetComponent<AIDestinationSetter>();

        if (enemyType == null) { enemyType = "Melee"; }
        patrolIndex = 0;
        detectDistance = 10f;
        patrolDirection = 1;
        canSwapTarget = true;
        onPatrol = true;
        onAttack = false;
        transform.position = patrolPositions[0].transform.position;
        aiDestSetter.target = patrolPositions[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerDirection = player.transform.position - transform.position;
        float playerDistance = (playerDirection).magnitude;

        Debug.DrawRay(transform.position,playerDirection*10f, Color.red);
        RaycastHit2D sightTest = Physics2D.Raycast(transform.position, playerDirection, 10f);

        if (playerDistance < detectDistance && sightTest.collider.tag == "Player" && onPatrol) {
            StartCoroutine(BeginAttack());
        } else if (playerDistance > detectDistance && onAttack) {
            StartCoroutine(BeginPatrol());
        }

        if (onPatrol) {
            Patrol();
        } else if (onAttack) {
            Attack();
        }
    }

    IEnumerator BeginPatrol() {
        yield return new WaitForEndOfFrame();
        onAttack = false;

        float closestPatrolDistance = 99999f;
        int closestPatrolIndex = 0;
        for (int i = 0; i < patrolPositions.Count; i++) {
            float newDistance = (player.transform.position - patrolPositions[i].position).magnitude;
            if (newDistance < closestPatrolDistance) {
                closestPatrolDistance = newDistance;
                closestPatrolIndex = i;
            }
        }

        patrolIndex = closestPatrolIndex;
        aiPath.maxSpeed = patrolSpeed;
        aiDestSetter.target = patrolPositions[patrolIndex];
        
        yield return new WaitForSeconds(1f);
        onPatrol = true;
    }

    void Patrol() {
        if ((aiPath.reachedEndOfPath) && patrolPositions.Count > 1) {
            if (patrolIndex == 0) {
                aiPath.slowdownDistance = patrolSpeed;
                patrolDirection = 1;
            } else if (patrolIndex == patrolPositions.Count - 1) {
                aiPath.slowdownDistance = patrolSpeed;
                patrolDirection = -1;
            } else {
                aiPath.slowdownDistance = 0;
            }

            if (canSwapTarget) {
                patrolIndex += patrolDirection;
                aiDestSetter.target = patrolPositions[patrolIndex];
                canSwapTarget = false;
                StartCoroutine(SwapTargetTimer());
            }
        }
    }

    IEnumerator BeginAttack() {
        yield return new WaitForEndOfFrame();
        onPatrol = false;
        aiPath.maxSpeed = 0f;
        aiDestSetter.target = player.transform;
        Debug.LogWarning("DETECTED!");
        yield return new WaitForSeconds(1f);
        Debug.LogWarning("ATTACK!");
        onAttack = true;
    }

    void Attack() {
        switch (enemyType) {
            case ("Melee"):
                aiPath.maxSpeed = attackSpeed;
                break;
            case ("Ranged"):
                Debug.LogWarning("pew pew pew");
                //Fire Bullets
                break;
            default:
                // Do Nothing
                break;
        }
    }

    IEnumerator SwapTargetTimer() {
        yield return new WaitForSeconds(0.1f);
        canSwapTarget = true;
    }
}
