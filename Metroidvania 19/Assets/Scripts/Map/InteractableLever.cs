using System.Collections;
using UnityEngine;

public class InteractableLever : InteractableObject
{
    private HingeJoint2D joint;
    private Coroutine checkCoroutine;
    private bool isUsed;
    private bool isActivated;
    private const int CHECK_TIME = 2;

    [SerializeField] private InteractableObject interactableObject;
    [Tooltip("Minimum angle to activate the lever.")]
    [SerializeField] private float triggerSensibility = 5f;

    private void Awake()
    {
        joint = GetComponent<HingeJoint2D>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, joint.limits.min);
    }

    private void Update()
    {
        if (isUsed)
            CheckLever();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (checkCoroutine != null)
                StopCoroutine(checkCoroutine);

            checkCoroutine = StartCoroutine(CheckLever());
        }
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
            float activationAngle = Mathf.Abs(joint.jointAngle - joint.limits.min);
            float deactivationAngle = Mathf.Abs(joint.jointAngle - joint.limits.max);

            if (!isActivated && activationAngle <= triggerSensibility)
            {
                OnActivation();
                checkCoroutine = null;
                yield break;
            }

            if (isActivated && deactivationAngle <= triggerSensibility)
            {
                OnDeactivation();
                checkCoroutine = null;
                yield break;
            }

            checkTime += Time.deltaTime;
            yield return null;
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
