using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rBdoy;
    private SpriteRenderer spriteRenderer;
    private Vector3 mousePos;
    private float currentSpeed;
    private float horizontalTurn;
    private int movementInput;

    [Header("Core Movement")]
    [Range(0.1f, 100)]
    [Tooltip("Acceleration speed of the body.")]
    [SerializeField] private float accelerationSpeed = 1;
    [Range(0.1f, 100)]
    [Tooltip("Deceleration speed of the body.")]
    [SerializeField] private float decelerationSpeed = 2;
    [Tooltip("Maximum speed the body will reach.")]
    [SerializeField] private float maxSpeed = 30;
    [Tooltip("Rotation speed when mouse input is selected.")]
    [SerializeField] private float mouseRotationSpeed = 3;
    [Tooltip("Rotation speed when keyboard input is selected.")]
    [SerializeField] private float keyboardRotationSpeed = 200;
    [Range(-5f, 5f)]
    [Tooltip("Separation between body segments. Can be positive (more separation), or negative (less separation).")]
    [SerializeField] private float segmentSeparation = -.1f;
    [Header("Sine Wave Movement")]
    [Range(0f, 100f)]
    [Tooltip("Amount of movement of the sine wave.")]
    [SerializeField] private float sineWaveScale = 40;
    [Range(0f, 100f)]
    [Tooltip("Speed of the sine wave.")]
    [SerializeField] private float sineWaveSpeed = 10;
    [Header("Input Type")]
    [Tooltip("Input type for controlling the player.")]
    [SerializeField] private InputType inputType = InputType.Mouse;


    private void Awake()
    {
        rBdoy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), Time.fixedDeltaTime * mouseRotationSpeed);

            rBdoy.MoveRotation(rotation);
        }
        else
        {
            if (horizontalTurn != 0)
                rBdoy.MoveRotation(rBdoy.rotation - horizontalTurn * Time.fixedDeltaTime * keyboardRotationSpeed);
            // Reset rigidbody angular velocity so if it collided with something doesn't keep turning 
            else
                rBdoy.angularVelocity = 0f;
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
        for (int i = 1; i < PlayerController.BodyParts.Count; i++)
        {
            // Get previous segment position
            Vector3 prevPosition = PlayerController.BodyParts[i - 1].transform.position;

            // Calculate rotation
            Vector3 lookDir = (prevPosition - PlayerController.BodyParts[i].transform.position).normalized; // We want to look towards the previous segment
            float rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            Quaternion newRotation = Quaternion.Euler(0, 0, rotation);

            PlayerController.BodyParts[i].Move(prevPosition, segmentSeparation, newRotation);
        }
    }
}
