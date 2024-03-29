using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAppendage : MonoBehaviour
{
    Transform grapplePoint;
    LineRenderer appendage;
    Vector3 vector3;
    Camera cam;
    private Vector3 appendageRotation;
    GetTarget target;
    bool caught = false;
    Vector3 objectPos;
    PlayerMovement playerMove;
    float curSpeed;
    private List<AppendageSegment> appendageSegments = new List<AppendageSegment>();
    int segmentCounter = 0;
    Vector3 copyGrapplePoint;
    float cooldown = 0f;
    float grappleTime = 0f;
    bool wasGrappling = false;
    float timeInactive = 0f;
    float timeActive = 0f;
    bool isGrappling = false;
    float distance;
    float appendageSize = 12;
    float segmentSize = 3f;

    [SerializeField] float breakDistance = 5f;
    public float targetPull = 2.0f;
    [SerializeField] float playerPull = 2.0f;
    [SerializeField] float appendageWidth = 1.0f;
    [SerializeField] float grappleCooldownLever = 3f;
    [SerializeField] float grappleCooldownEnemy = 5f;

    void Start()
    {
        //Debug.Log("AppendageHere");
        target = transform.parent.parent.transform.GetChild(3).GetComponent<GetTarget>();
        grapplePoint = gameObject.transform.parent.transform;
        copyGrapplePoint = grapplePoint.position;
        appendage = GetComponent<LineRenderer>();
        cam = Camera.main;
        playerMove = GetComponent<PlayerMovement>();
        for (int x = 0; x < appendageSize; x++)
        {

            appendageSegments.Add(new AppendageSegment(grapplePoint.position));
            
            copyGrapplePoint -= new Vector3(0, segmentSize);


        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(1) && (target.IsObjectClose() || target.isCaught()) && cooldown < Time.time) {
            
            if (target.objectTransform != null)
            {
                grapplePoint = gameObject.transform.parent.transform;
                copyGrapplePoint = grapplePoint.position;

                for (int x = 0; x < appendageSize; x++)
        {

                    appendageSegments.Add(new AppendageSegment(grapplePoint.position));
                    
                    copyGrapplePoint -= new Vector3(0, segmentSize);


                }
                
                objectPos = target.objectTransform.position;
                if(wasGrappling == false) {
                    appendageSize = (int)Vector3.Distance(grapplePoint.position, objectPos);
                    distance = appendageSize+breakDistance;
                    segmentSize = (float)appendageSize*0.08f;
                    Debug.Log("Set Grapple Size");
                }
                
                
                caught = true;
                wasGrappling = true;
                target.IsCaught(true);
                appendage.enabled = true;
                DrawAppendage();

                //target.IsCaught(true);

                //Adds a pulling or pushing force to the player or grappled object respectively 
                if (target.objectTransform.tag == "Enemy")
                {

                    target.objectTransform.GetComponent<Rigidbody2D>().AddForce((transform.parent.parent.position - objectPos).normalized * targetPull);
                    timeInactive = grappleCooldownEnemy;
                    //Debug.Log(timeInactive);
                }
                else if (target.objectTransform.tag == "Lever") {
                    target.objectTransform.GetComponent<Rigidbody2D>().AddForce((transform.parent.parent.position - objectPos).normalized * playerPull);
                    timeInactive = grappleCooldownLever;
                    //Debug.Log(timeInactive);
                }
                    


            }
            
        }
        else { appendage.enabled = false; }

        if ((Vector3.Distance(transform.parent.parent.position, objectPos) > distance) || Input.GetMouseButton(1) == false)
        {
            caught= false;
            
            
        }
        
        if (caught == false )
        {
            appendage.enabled = false;
            //appendage.SetPosition(0, Vector3.zero);
            //appendage.SetPosition(appendage.positionCount - 1, Vector3.zero);
            target.IsCaught(false);
            //Debug.Log("Detached Time: " + timeInactive);
            if(wasGrappling && cooldown == 0) {
                Debug.Log("Timer Start ");
                cooldown = Time.time + timeInactive;
                target.CheckObjects(target.objectTransform);
            }


        }

        if(cooldown < Time.time && wasGrappling && cooldown != 0)
        {
            Debug.Log("Timer Done");
            cooldown = 0;
            timeInactive = 0;
            wasGrappling= false;
        }
        
    }

    

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && (target.IsObjectClose() || target.isCaught()))
            SimulatePhysics();
    }

    public struct AppendageSegment
    {
        public Vector3 curPos;
        public Vector3 prevPos;

        public AppendageSegment(Vector3 pos)
        {
            curPos = pos;   
            prevPos = pos;
        }
    }

    private void DrawAppendage()
    {
        float appendWidth = appendageWidth;
        appendage.startWidth = appendWidth;
        appendage.endWidth = appendWidth;

        Vector3[] appendPos = new Vector3[(int)appendageSize];
        for (int x = 0; x < (int)appendageSize; x++)
        {
            appendPos[x] = appendageSegments[x].curPos;
        }

        appendage.positionCount = appendPos.Length;
        appendage.SetPositions(appendPos);
    }

    private void SimulatePhysics()
    {
        Vector3 gravity = new Vector3(0, -1f);
        for(int x = 1; x < (int)appendageSize -2; x++)
        {
            AppendageSegment firstSeg= appendageSegments[x];
            Vector3 velocity = firstSeg.curPos - firstSeg.prevPos;
            firstSeg.prevPos = firstSeg.curPos;
            firstSeg.curPos += velocity;
            firstSeg.curPos +=  gravity * Time.deltaTime;
            appendageSegments[x] = firstSeg;
        }
        
        for(int x =0; x < 500; x++)
        {
            Constraints();
        }
    }

    private void Constraints()
    {
        AppendageSegment firstSeg = appendageSegments[0];
        firstSeg.curPos = grapplePoint.position;
        appendageSegments[0] = firstSeg;



        AppendageSegment lastSeg = appendageSegments[(int)appendageSize-1];

        //lastSeg.curPos = Vector3.MoveTowards(lastSeg.curPos, objectPos, 1f * Time.deltaTime);
        lastSeg.curPos = objectPos;
        appendageSegments[(int)appendageSize - 1] = lastSeg;

        for (int x = 0; x < (int)appendageSize -1; x++)
        {
            AppendageSegment segOne= appendageSegments[x];
            AppendageSegment segTwo= appendageSegments[x+1];

            float distance = (segOne.curPos - segTwo.curPos).magnitude;
            float error = Mathf.Abs(distance - segmentSize);
            Vector3 direction = Vector3.zero;

            if(distance > segmentSize)
            {
                direction = (segOne.curPos - segTwo.curPos).normalized;
            }else if(distance < segmentSize)
            {
                direction = (segTwo.curPos- segOne.curPos).normalized;
            }

            Vector3 errorAmount = direction * error;

            if(x != 0)
            {
                segOne.curPos -= errorAmount * 0.5f;
                appendageSegments[x] = segOne;
                segTwo.curPos += errorAmount * 0.5f;
                appendageSegments[x+1] = segTwo;
            }
            else
            {
                segTwo.curPos += errorAmount;
                appendageSegments[x + 1] = segTwo;
            }
        }
    }
    
}
