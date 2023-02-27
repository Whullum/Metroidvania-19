using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    #region variables

    private CinemachineVirtualCamera cmCam;
    private CinemachineComponentBase cmBody;
    private Coroutine lensSizeCoroutine;
    private float currentLensSize;

    [Tooltip("Amount to add or substract from the current camera lens size.")]
    [SerializeField] private float lensSizeStep = 1.5f;
    [Tooltip("Smooth time from camera size transitions.")]
    [SerializeField] private float smoothTime = 50f;

    #endregion

    private void Awake()
    {
        cmCam = GetComponent<CinemachineVirtualCamera>();
        cmBody = cmCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        currentLensSize = cmCam.m_Lens.OrthographicSize;
        SetCameraSize(currentLensSize);
    }

    private void OnEnable()
    {
        PlayerController.SegmentAdded += IncreaseLensSize;
        PlayerController.SegmentRemoved += DecreaseLensSize;
    }

    private void OnDisable()
    {
        PlayerController.SegmentAdded -= IncreaseLensSize;
        PlayerController.SegmentRemoved -= DecreaseLensSize;
    }

    /// <summary>
    /// Increases the camera lens size.
    /// </summary>
    private void IncreaseLensSize()
    {
        if (lensSizeCoroutine != null)
            StopCoroutine(lensSizeCoroutine);

        currentLensSize += lensSizeStep;
        lensSizeCoroutine = StartCoroutine(ChangeLensSize(currentLensSize));
    }

    /// <summary>
    /// Decreases the camera lens size.
    /// </summary>
    private void DecreaseLensSize()
    {
        if (lensSizeCoroutine != null)
            StopCoroutine(lensSizeCoroutine);

        currentLensSize -= lensSizeStep;
        lensSizeCoroutine = StartCoroutine(ChangeLensSize(currentLensSize));
    }

    /// <summary>
    /// Sets a specific camera size.
    /// </summary>
    /// <param name="size">Camera size.</param>
    private void SetCameraSize(float size)
    {
        if (lensSizeCoroutine != null)
            StopCoroutine(lensSizeCoroutine);

        lensSizeCoroutine = StartCoroutine(ChangeLensSize(size));
    }

    /// <summary>
    /// Modifies the camera size over time.
    /// </summary>
    /// <param name="desiredSize">The desired camera size.</param>
    /// <returns></returns>
    private IEnumerator ChangeLensSize(float desiredSize)
    {
        float velocity = 0;

        while (cmCam.m_Lens.OrthographicSize != desiredSize)
        {
            float nextSize = Mathf.SmoothDamp(cmCam.m_Lens.OrthographicSize, desiredSize, ref velocity, Time.deltaTime * smoothTime);
            cmCam.m_Lens.OrthographicSize = nextSize;
            yield return null;
        }
        lensSizeCoroutine = null;
    }
}
