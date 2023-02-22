using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodySegment))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    public static bool IsMoving { get; private set; }

    private List<BodySegment> bodySegments = new List<BodySegment>();
    private Rigidbody2D rBdoy;
    private Vector3 mousePos;
    private Vector3 lastPosition;
    private Vector3 velocity;
    private float segmentSize;
    private float horizontalTurn;

    [Header("Movement values")]
    [SerializeField] private float reachedDistance = 0.5f;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bodyMoveVelocity = 3;
    [SerializeField] private bool keyboardInput;
    [Header("Body segments")]
    [SerializeField] private List<GameObject> segmentPrefabs;
    [SerializeField] private float distanceBetweenSegments;

    private void Awake()
    {
        rBdoy = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastPosition = transform.position;
        segmentSize = GetComponent<Collider2D>().bounds.size.x;
        bodySegments.Clear();
        bodySegments.Add(GetComponent<BodySegment>());
    }

    private void Update()
    {
        GetInput();
        CalculateVelocity();
    }

    private void FixedUpdate()
    {
        CreateSegments();
        Move();
        Rotate();
        MoveSegments();
    }

    private void GetInput()
    {
        if (keyboardInput)
            horizontalTurn = Input.GetAxis("Horizontal");
        else
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Move()
    {
        rBdoy.velocity = transform.right * Time.fixedDeltaTime * speed;
    }

    private void Rotate()
    {
        Vector3 lookDirection = mousePos - transform.position;
        float targetRotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), Time.fixedDeltaTime * rotationSpeed);

        rBdoy.MoveRotation(rotation);

        //if (horizontalTurn != 0)
        //    transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * horizontalTurn * Time.fixedDeltaTime));
    }

    private void CalculateVelocity()
    {
        if (transform.hasChanged)
        {
            velocity = (transform.position - lastPosition) / Time.deltaTime;
            transform.hasChanged = false;
        }
        lastPosition = transform.position;

        if (velocity.magnitude <= bodyMoveVelocity)
            IsMoving = false;
        else
            IsMoving = true;
    }

    private void CreateSegments()
    {
        if (segmentPrefabs.Count <= 0) return;

        BodySegment leader = bodySegments[bodySegments.Count - 1];
        float distance = Vector2.Distance(leader.Path[0].Position, leader.Path[leader.Path.Count - 1].Position);

        if (distance >= segmentSize + distanceBetweenSegments)
        {
            BodySegment newSegment = Instantiate(segmentPrefabs[0], leader.Path[0].Position, leader.Path[0].Rotation, transform).GetComponent<BodySegment>();

            bodySegments.Add(newSegment);

            segmentPrefabs.RemoveAt(0);
        }
    }

    private void MoveSegments()
    {
        if (!IsMoving) return;

        for (int i = 1; i < bodySegments.Count; i++)
        {
            BodySegment nextSegment = bodySegments[i - 1];

            bodySegments[i].transform.position = nextSegment.Path[0].Position;
            bodySegments[i].transform.rotation = nextSegment.Path[0].Rotation;

            nextSegment.Path.RemoveAt(0);
        }
    }
}
