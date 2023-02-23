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
    private int movementInput;

    [Header("Core Movement")]
    [Range(0.1f, 100)]
    [SerializeField] private float accelerationSpeed = 1;
    [Range(0.1f, 100)]
    [SerializeField] private float decelerationSpeed = 2;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float rotationSpeed = 3;
    [Range(-5f, 5f)]
    [SerializeField] private float segmentSeparation = -.1f;
    [Range(3, 100)]
    [SerializeField] private int bodySize = 3;
    [SerializeField] private GameObject bodySegment;
    [SerializeField] private GameObject tailSegment;
    [Header("Sine Wave Movement")]
    [Range(0f, 100f)]
    [SerializeField] private float sineWaveScale = 40;
    [Range(0f, 100f)]
    [SerializeField] private float sineWaveSpeed = 10;
    [Header("Input Type")]
    [SerializeField] private InputType inputType = InputType.Mouse;


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

    /// <summary>
    /// Checks player input and sets all input-related variables.
    /// </summary>
    private void GetInput()
    {
        if (inputType == InputType.Keyboard)
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

    /// <summary>
    /// Accelerates or decelerates the body depending on player input.
    /// </summary>
    private void Accelerate()
    {
        if (movementInput == 1)
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * accelerationSpeed);
        else
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * decelerationSpeed);
    }

    /// <summary>
    /// Moves the body towards it's right vector and limits rigidbody velocity.
    /// </summary>
    private void Move()
    {
        float accelerationMultiplier = 1 - (rBdoy.velocity.magnitude / maxSpeed);
        rBdoy.AddForce(transform.right * Time.deltaTime * currentSpeed * accelerationMultiplier * movementInput, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Moves the body in a sine wave motion always in its perpendicular movement vector.
    /// </summary>
    private void SineWaveMotion()
    {
        float acceleration = -Mathf.Sin(Time.time * sineWaveSpeed) * sineWaveScale;
        float force = acceleration * rBdoy.mass;
        force = force * currentSpeed / maxSpeed;
        rBdoy.AddForce(Vector2.Perpendicular(transform.right) * force);
    }

    /// <summary>
    /// Rotates the body to look towards the mouse position or keyboard input and flips player sprite depending on it's facing direction.
    /// </summary>
    private void Rotate()
    {
        if (inputType == InputType.Mouse)
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

    /// <summary>
    /// Moves all the body segments except for the head
    /// </summary>
    private void MoveSegments()
    {
        for (int i = 1; i < bodyParts.Count; i++)
        {
            // Get previous segment position
            Vector3 prevPosition = bodyParts[i - 1].transform.position;

            // Calculate rotation
            Vector3 lookDir = (prevPosition - bodyParts[i].transform.position).normalized; // We want to look towards the previous segment
            float rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.Euler(0, 0, rotation);

            bodyParts[i].Move(prevPosition, segmentSeparation, newRotation);
        }
    }

    /// <summary>
    /// Initializes the body with it's initial shape and lenght.
    /// </summary>
    private void CreateBody()
    {
        bodyParts.Add(GetComponent<BodySegment>());

        for (int i = 0; i < bodySize; i++)
        {
            if (i == bodySize - 1)
                AddBodySegment(true);
            else
                AddBodySegment(false);
        }
    }

    /// <summary>
    /// Creates and adds a body segment to the player.
    /// </summary>
    /// <param name="isTail">If set to true, the last segment will be changes to be a normal body part and the tail segment will be created at the end of the body.</param>
    private void AddBodySegment(bool isTail)
    {
        GameObject segment = bodySegment;

        // If this is the last body part, we create the tail GameObject and recreate the previous one
        if (isTail)
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

    /// <summary>
    /// Calculates the sorting order of the body parts.
    /// </summary>
    private void CalculateSortingOrder()
    {
        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            bodyParts[bodyParts.Count - 1 - i].SetSortingOrder(i);
        }
        spriteRenderer.sortingOrder = bodyParts.Count;
    }
}
