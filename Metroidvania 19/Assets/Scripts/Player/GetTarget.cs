using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetTarget : MonoBehaviour
{
    bool objectClose = false;
    List<Transform> targets = new List<Transform>();
    public Transform objectPos;
    
    /// bool caught = false;
    

    private void Update()
    {
        objectPos = GetNearestTarget();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player") {
            
            objectClose = true;
            
            targets.Add(collision.transform);
            //Debug.Log(targets.Count);
            //Debug.Log(targets[0].position);
            objectPos = GetNearestTarget();
        }
        
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectClose= false;
        objectPos = null;
        targets.Clear();
    }


    private Transform GetNearestTarget() {
        if (targets.Count != 0) {
            Transform nearestTarget = targets[0];
            float curDistance = Vector3.Distance(transform.position, targets[0].position);
            foreach (Transform t in targets)
            {

                if (curDistance > Vector3.Distance(transform.position, t.position))
                {
                    //Debug.Log(t);
                    curDistance = Vector3.Distance(transform.position, t.position);
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
    public bool IsObjectClose() {
        
        return objectPos;
        
        
    }

    //public void IsCaught(bool isCaught) { caught = isCaught;  }
}
