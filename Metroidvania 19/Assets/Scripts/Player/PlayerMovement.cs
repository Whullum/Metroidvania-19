using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(BodySegment))]
public class PlayerMovement : MonoBehaviour
{
    private List<BodySegment> bodyParts = new List<BodySegment>();
    private Rigidbody2D rBdoy;
    private SpriteRenderer spriteRenderer;
    private Vector3 mousePos;
    private float currentSpeed;
    private float horizontalTurn;
    private float segmentSize;
    public int movementInput;

    [Header("Movement")]
    [SerializeField] private float sineWaveScale = 40;
    [SerializeField] private float sineWaveSpeed = 10;
    [SerializeField] private float accelerationSpeed = 1;
    [SerializeField] private float decelerationSpeed = 2;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float rotationSpeed = 3;
    [Range(3, 100)]
    [SerializeField] private int bodySize = 3;
    [SerializeField] private GameObject bodySegment;
    [SerializeField] private GameObject tailSegment;
    [SerializeField] private float segmentSeparation = -.5f;
    [SerializeField] private bool keyboardInput;


    private void Awake()
    {
        rBdoy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CreateBody();
    }

    private void Update()
    {
        GetInput();
        Accelerate();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
        MoveSegments();
        SineWaveMotion();
    }

    private void GetInput()
    {
        if (keyboardInput)
        {
            horizontalTurn = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                movementInput = 1;
            else
                movementInput = 0;
        }
        else
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0))
                movementInput = 1;
            else
                movementInput = 0;
        }
    }

    private void Accelerate()
    {
        if (movementInput == 1)
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * accelerationSpeed);
        else
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * decelerationSpeed);
    }

    private void Move()
    {
        float accelerationMultiplier = 1 - (rBdoy.velocity.magnitude / maxSpeed);
        rBdoy.AddForce(transform.right * Time.deltaTime * currentSpeed * accelerationMultiplier * movementInput, ForceMode2D.Impulse);
    }

    private void Rotate()
    {
        if (!keyboardInput)
        {
            Vector3 lookDirection = mousePos - transform.position;
            float targetRotation = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), Time.fixedDeltaTime * rotationSpeed);

            rBdoy.MoveRotation(rotation);
        }
        else
        {
            if (horizontalTurn != 0)
                transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * horizontalTurn * Time.fixedDeltaTime));
        }

        if (transform.right.x >= 0)
            spriteRenderer.flipY = false;
        else
            spriteRenderer.flipY = true;
    }

    private void MoveSegments()
    {
        for (int i = 1; i < bodyParts.Count; i++)
        {
            Vector3 prevPosition = bodyParts[i - 1].transform.position;
            Vector3 moveDirection = (prevPosition - bodyParts[i].transform.position).normalized;

            Vector3 lookDir = (prevPosition - bodyParts[i].transform.position).normalized;
            float rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Vector3 newPosition = prevPosition - moveDirection * (segmentSize + segmentSeparation);
            Quaternion newRotation = Quaternion.Euler(0, 0, rotation);

            bodyParts[i].Move(newPosition, newRotation);
        }
    }

    private void CreateBody()
    {
        segmentSize = GetComponent<Collider2D>().bounds.size.x;
        bodyParts.Add(GetComponent<BodySegment>());

        for (int i = 0; i < bodySize; i++)
        {
            AddBodySegment(i);
        }
    }

    private void SineWaveMotion()
    {
        float acceleration = -Mathf.Sin(Time.time * sineWaveSpeed) * sineWaveScale;
        float force = acceleration * rBdoy.mass;
        force = force * currentSpeed / maxSpeed;
        rBdoy.AddForce(Vector2.Perpendicular(transform.right) * force);
    }

    private void AddBodySegment(int sortingOrder)
    {
        GameObject segment = bodySegment;

        if (sortingOrder == bodySize - 1)
        {
            segment = tailSegment;
            GameObject lastSegment = bodyParts[bodyParts.Count - 1].gameObject;
            BodySegment oldSegment = Instantiate(bodySegment, lastSegment.transform.position, lastSegment.transform.rotation, transform).GetComponent<BodySegment>();

            bodyParts.RemoveAt(bodyParts.Count - 1);
            bodyParts.Add(oldSegment);

            Destroy(lastSegment);
        }

        BodySegment newSegment = Instantiate(segment, bodyParts[bodyParts.Count - 1].transform.position, bodyParts[bodyParts.Count - 1].transform.rotation, transform).GetComponent<BodySegment>();
        bodyParts.Add(newSegment);

        CalculateSortingOrder();
    }

    private void CalculateSortingOrder()
    {
        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            bodyParts[bodyParts.Count - 1 - i].SetSortingOrder(i);
        }
        spriteRenderer.sortingOrder = bodyParts.Count;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }
}
