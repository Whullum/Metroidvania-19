using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEnemy : MonoBehaviour
{
    bool objectClose = false;
    public Transform objectPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player") {
            objectClose = true;
            objectPos = collision.transform;
        }
        
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectClose= false;
        objectPos = null;
    }

    public bool IsObjectClose() {
        
        return objectPos;
        
        
    }
}
