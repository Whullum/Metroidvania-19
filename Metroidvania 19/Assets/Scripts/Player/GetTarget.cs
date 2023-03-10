using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetTarget : MonoBehaviour
{
    bool objectClose = false;
    List<Transform> targets = new List<Transform>();
    public Transform objectTransform;
    
    
    bool caught = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (targets.Count == 0 && caught == false) { objectClose = false; }
        //objectTransform = GetNearestTarget();
        Debug.Log(targets.Count);
        Debug.Log("Is caught: " + caught);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!targets.Contains(collision.transform) && (collision.gameObject.tag == "Respawn" || collision.gameObject.tag == "Finish")) {
            
            objectClose = true;
            
            targets.Add(collision.transform);
            
            if(caught == false)
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


    private Transform GetNearestTarget() {
        if (targets.Count != 0) {
            Transform nearestTarget = targets[0];
            float curDistance = Vector3.Distance(transform.parent.position, targets[0].position);
            foreach (Transform t in targets)
            {

                if (curDistance > Vector3.Distance(transform.parent.position, t.position))
                {
                    //Debug.Log(t);
                    curDistance = Vector3.Distance(transform.parent.position, t.position);
                    nearestTarget = t;
                    //Debug.Log("curDistance " + curDistance);
                    //Debug.Log("t " + t.position);
                }
            }
            //Debug.Log("Nearest Positon: " + nearestTarget);
            return nearestTarget;
        }
        return null;
    }

    public void CheckObjects(Collider2D col =null)
    {
        //remove last caught object
        if(col!=null) {
            if (col.gameObject.transform == objectTransform && targets.Count > 0 && caught == false)
            {
                targets.Remove(col.transform);
                
            }
            else if(col.gameObject.transform != objectTransform && targets.Count > 0 && caught == false)
            {
                targets.Remove(col.transform);
            }
            else if (col.gameObject.transform != objectTransform && targets.Count > 0 && caught == true)
            {
                targets.Remove(col.transform);
            }
            else if (col.gameObject.transform == objectTransform && targets.Count > 0 && caught == true)
            {
                targets.Remove(col.transform);
            }

            

        }
        
    }
    public bool IsObjectClose() {
        
        return objectClose;
        
        
    }

    public void IsCaught(bool isCaught) { caught = isCaught;  }


}
