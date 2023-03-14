using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetTarget : MonoBehaviour
{
    bool objectClose = false;
    List<Transform> targets = new List<Transform>();
    public Transform objectTransform;
    Rigidbody2D objectRig;
    int direct = -1;
    bool caught = false;
    string tempTag = null;
    bool isYLocked = false;
    bool wasCaught = false;

    [SerializeField] int thrashForce = 4000;
    [SerializeField] int attackDamage = 1;
    


    private void Start()
    {
        
    }

    private void Update()
    {
        //if (targets.Count == 0 && caught == false) { objectClose = false; }

        //Debug.Log("Targets: " + targets.Count);
        //Debug.Log("Can Grapple " + canGrappling);
        /*if (caught == true)
        {
            wasCaught = true;
            if(objectTransform)
            {
                if (objectTransform.tag == "Enemy")
                {
                    timeInactive = grappleCooldownEnemy;
                }
                else if (objectTransform.tag == "Lever")
                {
                    timeInactive = grappleCooldownLever;
                }
            }
            

        }else if (wasCaught == true && caught == false)
        {
            DeactivateGrapple();
            wasCaught= false;
        }else if(Time.time > cooldown)
        {
            
            canGrappling=true;
            cooldown = 0;
            timeInactive = 0;
            objectTransform = GetNearestTarget();
        }*/

        if (caught)
        {

            if (objectTransform != null && caught && Input.GetKeyDown(KeyCode.F))
            {
           
                if(objectTransform.GetComponent<Enemy>()) { 
                    if (objectTransform.TryGetComponent(out IDamageable damageable))
                    {
                        //Halts grappled object
                        
                        
                        //StartCoroutine(SetAIPath());
                        Debug.Log("Thrashing " + objectTransform.gameObject.name);
                        objectTransform.GetComponent<AIPath>().enabled = false;
                        Debug.Log(objectTransform.GetComponent<Rigidbody2D>().constraints);
                        objectTransform.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                        /*if(objectTransform.GetComponent<Rigidbody2D>().constraints.con)
                        {
                            Debug.Log("Unlocked Y Position");
                            isYLocked= true;
                            
                        }*/

                        direct *= -1;
                        objectTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        objectTransform.GetComponent<Rigidbody2D>().angularVelocity = 0;
                        objectTransform.GetComponent<Rigidbody2D>().AddForce((new Vector3(objectTransform.position.x , objectTransform.position.y + (20 * direct)) - objectTransform.position).normalized * thrashForce);
                        //Add force to bottom or top of a grappled object. "direct" alternates the direction.
                        var vec = (new Vector3(objectTransform.position.x, objectTransform.position.y + (20 * direct)));
                        Debug.Log(vec);
                        
                        if (damageable.Damage(attackDamage))
                            damageable.Death();
                            targets.Remove(objectTransform);

                        
                    }
                }
            }
            StartCoroutine(SetAIPath());
        }/*else if(wasCaught == true && caught == false)
        {
            DeactivateGrapple();
            wasCaught = true;
        }
        else if(cooldown > Time.time)
        {
            //Debug.Log("No Grappling");
            canGrappling =false;
            
        }*/

        
    }


    private IEnumerator SetAIPath()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(4);
        //objectTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        if(objectTransform != null)
            objectTransform.GetComponent<AIPath>().enabled = true;
        
        /*objectTransform.GetComponent<Rigidbody2D>().
        objectTransform.GetComponent<Rigidbody2D>().AddForce((new Vector3(.5f,.5f,0)), ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(1);
        objectTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        objectTransform.GetComponent<Rigidbody2D>().angularVelocity = 0;
        yield return new WaitForSeconds(0.5f);
        objectTransform.GetComponent<Rigidbody2D>().AddForce((new Vector3(-.5f, -.5f, 0)), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        objectTransform.GetComponent<AIPath>().enabled = true;*/
    }
    ///<summary> 
    ///When an object enters the collider, that object's tag is checked to determine if it is one of the targets or if it has the required tag to become a target.
    ///Searches for nearest target at the end
    ///<param name="collision"/> collision that has entered this object's collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!targets.Contains(collision.transform) && (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Lever") && !caught) {
            
            objectClose = true;

            targets.Add(collision.transform);

            //Debug.Log("Got Object");
            objectTransform = GetNearestTarget();
            

            
            
        }
        
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targets.Contains(collision.transform))
        {
            CheckObjects(collision);
        }
            

        //targets.Clear();
    }


    /// <summary>
    /// Gets the nearest target realative to the player.
    /// </summary>
    /// <returns></returns>
    private Transform GetNearestTarget() {
        //Debug.Log("Getting new target");
        if (targets.Count != 0) {
            Transform nearestTarget = targets[0];
            float curDistance = Vector3.Distance(transform.parent.position, targets[0].position);
            foreach (Transform t in targets)
            {

                if (curDistance > Vector3.Distance(transform.parent.position, t.position))
                {
                    
                    curDistance = Vector3.Distance(transform.parent.position, t.position);
                    nearestTarget = t;
                    
                }
            }
            
            
            return nearestTarget;
        }
        return null;
    }

    /// <summary>
    /// Determines if collier can be removed.
    /// </summary>
    /// <param name="col"> collider to be removed</param>
    public void CheckObjects(Collider2D col =null)
    {
        
        //remove last caught object
        if(col.transform != objectTransform) {

            targets.Remove(col.transform);


        }
        else if((col.transform == objectTransform && caught == false))
        {
            targets.Remove(col.transform);
            objectTransform = null;
        }
        
    }
    public bool IsObjectClose() {
        
        return objectClose;
        
        
    }

    public bool isCaught() {
        return caught;
    }
    public void IsCaught(bool isCaught) { 
        caught = isCaught; 
        
    }

    /// <summary>
    /// Sets cooldown timer for grappling.
    /// </summary>
    public void DeactivateGrapple()
    {
        Debug.Log(objectTransform);
        //Debug.Log("caught " + caught);
        //Debug.Log("was caught " + wasCaught);
        //Debug.Log("can g " + canGrappling);
        //canGrappling = false;
        //cooldown = Time.time + timeInactive;
        //Debug.Log("No Grappling for: " + timeInactive);
    }
}
