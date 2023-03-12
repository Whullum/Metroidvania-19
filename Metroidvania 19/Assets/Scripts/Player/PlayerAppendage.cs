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
    

    [SerializeField] float breakDistance = 20f;
    public float targetPull = 2.0f;
    


    [SerializeField] float playerPull = 2.0f;
    [SerializeField] int appendageSize = 12;
    [SerializeField] float appendageWidth = 1.0f;
    [SerializeField] float segmentSize = 3f;
    



    void Start()
    {
        target = transform.parent.parent.transform.GetChild(3).GetComponent<GetTarget>();
        grapplePoint = gameObject.transform.parent.parent.GetChild(2).transform;
        copyGrapplePoint = grapplePoint.position;
        appendage = GetComponent<LineRenderer>();
        cam = Camera.main;
        playerMove = GetComponent<PlayerMovement>();
        for (int x = 0; x < appendageSize; x++)
        {
            appendageSegments.Add(new AppendageSegment(grapplePoint.position));
            //grapplePoint.position = new Vector3(0, 0, 90);
            copyGrapplePoint -= new Vector3(0, segmentSize);


        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) == true)
        {
            caught = true;
            
            
        }
        else
        {
            caught= false;
            
        }
        
        if (Input.GetMouseButton(1) == true && target.isCaught()) {
            if (target.objectTransform != null)
            {
                objectPos = target.objectTransform.position;

                appendage.enabled = true;
                DrawAppendage();

                target.IsCaught(true);

                //gameObject.transform.localScale.Set(Vector3.Distance(appendage.GetPosition(1), appendage.GetPosition(0)),appendage.startWidth,0);
                if (target.objectTransform.tag == "Enemy")
                {
                    
                    gameObject.transform.parent.parent.GetComponent<Rigidbody2D>().AddForce((objectPos - transform.parent.parent.position).normalized * targetPull);
                }
                else if (target.objectTransform.tag == "Lever")
                    target.objectTransform.GetComponent<Rigidbody2D>().AddForce((transform.parent.parent.position - objectPos).normalized * playerPull);

                
            }
            
        }
        else { appendage.enabled = false; }

        if ((Vector3.Distance(transform.parent.parent.position, objectPos) > breakDistance) || Input.GetMouseButton(1) == false)
        {
            caught= false;
            target.IsCaught(false);
        }

        if (caught == false )
        {
            appendage.enabled = false;
            appendage.SetPosition(0, Vector3.zero);
            appendage.SetPosition(appendage.positionCount - 1, Vector3.zero);
            target.IsCaught(false);




        }


        
    }

    

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && (target.IsObjectClose() || caught))
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
        for (int x = 0; x < appendageSize; x++)
        {
            appendPos[x] = appendageSegments[x].curPos;
        }

        appendage.positionCount = appendPos.Length;
        appendage.SetPositions(appendPos);
    }

    private void SimulatePhysics()
    {
        Vector3 gravity = new Vector3(0, -1f);
        for(int x = 1; x < appendageSize-2; x++)
        {
            AppendageSegment firstSeg= appendageSegments[x];
            Vector3 velocity = firstSeg.curPos - firstSeg.prevPos;
            firstSeg.prevPos = firstSeg.curPos;
            firstSeg.curPos += velocity;
            firstSeg.curPos +=  gravity * Time.deltaTime;
            appendageSegments[x] = firstSeg;
        }
        
        for(int x =0; x < 50; x++)
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

        for (int x = 0; x < appendageSize-1; x++)
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
