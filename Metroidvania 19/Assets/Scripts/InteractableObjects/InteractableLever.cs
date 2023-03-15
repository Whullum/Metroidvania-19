using System.Collections;
using UnityEngine;

public class InteractableLever : InteractableObject
{
    private HingeJoint2D joint;
    private Coroutine checkCoroutine;
    private bool isUsed;
    private const int CHECK_TIME = 2;

    [Tooltip("Initial state of this lever.")]
    [SerializeField] private bool isActivated;
    [Tooltip("Minimum angle to activate the lever.")]
    [SerializeField] private float triggerSensibility = 5f;
    [SerializeField] private InteractableObject interactableObject;

    private void Awake() => joint = GetComponent<HingeJoint2D>();

    private void Start() => InitializeLever();

    private void Update()
    {
        if (isUsed)
            CheckLever();

        if (isActivated)
            GetComponent<SpriteRenderer>().color = Color.green;
        else
            GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.collider.CompareTag("Player"))
        // {
            
        // }

        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(CheckLever());
    }

    /// <summary>
    /// For a short amount of time, the lever checks if it's angle is near one of the trigger sides. If it's close enought, launchs the specific event.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckLever()
    {
        float checkTime = 0;

        while (checkTime < CHECK_TIME)
        {
            // Check current angles
            float activationAngle = Mathf.Abs(joint.jointAngle - joint.limits.min);
            float deactivationAngle = Mathf.Abs(joint.jointAngle - joint.limits.max);

            // Check if lever has been activated
            if (!isActivated && activationAngle <= triggerSensibility)
            {
                OnActivation();
                checkCoroutine = null;
                yield break;
            }

            // Check if lever has been deactivated
            if (isActivated && deactivationAngle <= triggerSensibility)
            {
                OnDeactivation();
                checkCoroutine = null;
                yield break;
            }

            checkTime += Time.deltaTime;
            yield return null;
        }
        checkCoroutine = null;
    }

    /// <summary>
    /// Initializes the lever depending on its initial state.
    /// </summary>
    private void InitializeLever()
    {
        if (isActivated)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, joint.limits.max);
            interactableObject.OnActivation();
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, joint.limits.min);
            interactableObject.OnDeactivation();
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

        isActivated = false;
    }
}
