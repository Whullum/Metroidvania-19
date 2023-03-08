using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppendage : MonoBehaviour
{
    Transform grapplePoint;
    LineRenderer appendage;
    Vector3 vector3;
    Camera cam;
    private Vector3 appendageRotation;
    GetEnemy enemy;
    bool caught = false;
    Vector3 objectPos;

    [SerializeField] float maxDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.GetChild(3).GetComponent<GetEnemy>();
        grapplePoint = gameObject.transform.GetChild(2);
        appendage = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Object is " + enemy.IsObjectClose());
        //Debug.Log("Caught object: " + caught);
        Debug.Log("Distance: " + Vector3.Distance(transform.position, objectPos));
        if (Input.GetMouseButton(1) && (enemy.IsObjectClose() || caught)) {
            if(enemy.objectPos != null)
                objectPos = enemy.objectPos.position;
            
            vector3 = Input.mousePosition;
            appendage.SetPosition(0, grapplePoint.position);
            appendage.SetPosition(1, objectPos);
            caught= true;
        }
        
        if(Vector3.Distance(transform.position, objectPos) > maxDistance || Input.GetMouseButton(1) == false)
        {
            caught= false;
        }

        if (caught == false)
        {
            appendage.SetPosition(0, Vector3.zero);
            appendage.SetPosition(1, Vector3.zero);
        }


    }
}
