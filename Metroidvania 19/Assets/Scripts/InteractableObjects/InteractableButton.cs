using UnityEngine;

public class InteractableButton : InteractableObject
{
    private Rigidbody2D rBody;
    private float triggerDistance;
    private bool isPressed;
    private bool canActivate = true; // Prevents multiple activations on the same hit

    [SerializeField] private SpriteRenderer buttonSprite;
    [SerializeField] private Color activatedColor;
    [SerializeField] private Color deactivatedColor;

    [Header("Button Properties")]
    [Tooltip("Transform used to calculated button traveled distance.")]
    [SerializeField] private Transform groundUnion;
    [Tooltip("Use for modifying rigidbody constraints. If button is facing vertically, this should be set to true.")]
    [SerializeField] private bool isVertical = true;
    [Tooltip("Initial state of this button.")]
    [SerializeField] protected bool isActivated;
    [Header("Interactable Object")]
    [SerializeField] private InteractableObject interactableObject;

    private void Awake() => rBody = GetComponent<Rigidbody2D>();

    private void Start()
    {
        // Sets the rigidbody constraints
        if (isVertical)
            rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        // Calculate the trigger distance
        triggerDistance = GetComponent<Collider2D>().bounds.size.y / 2;

        // Initialize the interactable object attached to this button
        if (isActivated)
            interactableObject.OnActivation();
        else
            interactableObject.OnDeactivation();
    }

    private void Update()
    {
        if (isPressed)
            CheckActivation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPressed = true;
            Invoke("ResetPressed", 1f);
        }
    }

    /// <summary>
    /// Checks if the button is pressed enought.
    /// </summary>
    private void CheckActivation()
    {
        if (Vector2.Distance(transform.position, groundUnion.position) < triggerDistance)
        {
            if (!isActivated)
                OnActivation();
            else
                OnDeactivation();
        }
    }

    /// <summary>
    /// Resets the button pressed state.
    /// </summary>
    private void ResetPressed() => isPressed = false;

    /// <summary>
    /// Resets the button activation, so it can be used again.
    /// </summary>
    private void ResetActivation() => canActivate = true;

    public override void OnActivation()
    {
        if (isActivated || !canActivate) return;

        interactableObject.OnActivation();
        isActivated = true;
        canActivate = false;
        buttonSprite.color = activatedColor;

        Invoke("ResetActivation", 1f);
    }

    public override void OnDeactivation()
    {
        if (!isActivated || !canActivate) return;

        interactableObject.OnDeactivation();
        isActivated = false;
        canActivate = false;
        buttonSprite.color = deactivatedColor;

        Invoke("ResetActivation", 1f);
    }
}
