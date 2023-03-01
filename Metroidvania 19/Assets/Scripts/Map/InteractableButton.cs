using UnityEngine;

public class InteractableButton : InteractableObject
{
    private Rigidbody2D rBody;
    private float triggerDistance;
    private bool isPressed;
    private bool isActivated;

    [Header("Button Properties")]
    [Tooltip("Transform used to calculated button traveled distance.")]
    [SerializeField] private Transform groundUnion;
    [Tooltip("Use for modifying rigidbody constraints. If button is facing vertically, this should be set to true.")]
    [SerializeField] private bool isVertical = true;
    [Header("Interactable Object")]
    [SerializeField] private InteractableObject interactableObject;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (isVertical)
            rBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        triggerDistance = GetComponent<Collider2D>().bounds.size.y / 2;
    }

    private void Update()
    {
        if (!isActivated && isPressed)
            CheckActivation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPressed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPressed = false;
        }
    }

    private void CheckActivation()
    {
        if (Vector2.Distance(transform.position, groundUnion.position) < triggerDistance)
        {
            OnActivation();
        }
    }

    public override void OnActivation()
    {
        interactableObject.OnActivation();

        isActivated = true;
    }

    public override void OnDeactivation()
    {
        interactableObject.OnDeactivation();
    }
}
