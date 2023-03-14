using System.Collections;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    #region variables

    /// <summary>
    /// When invoked, shakes the camera with the default values.
    /// </summary>
    public static Action ShakeCamera;

    private GameObject confiner;
    private CinemachineVirtualCamera cmCam;
    private CinemachineBasicMultiChannelPerlin cmNoise;
    private Coroutine lensSizeCoroutine;
    private float currentLensSize;
    private bool isShaking;

    [Header("Zoom Properties")]
    [Tooltip("Amount to add or substract from the current camera lens size.")]
    [SerializeField] private float lensSizeStep = 1.5f;
    [Tooltip("Smooth time from camera size transitions.")]
    [SerializeField] private float smoothTime = 50f;
    [Header("Shake Properties")]
    [Range(0, 10)]
    [Tooltip("Amount of movement.")]
    [SerializeField] private float amplitudeGain = 2f;
    [Range(0, 10)]
    [Tooltip("Velocity of movement.")]
    [SerializeField] private float frequencyGain = 3f;
    [Range(0, 1)]
    [Tooltip("Duration of the shake.")]
    [SerializeField] private float duration = .2f;

    #endregion

    private void Awake()
    {
        cmCam = GetComponent<CinemachineVirtualCamera>();
        cmNoise = cmCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        currentLensSize = cmCam.m_Lens.OrthographicSize;
        SetCameraSize(currentLensSize);

        CreateConfinerCollider();
    }

    private void OnEnable()
    {
        PlayerController.SegmentAdded += IncreaseLensSize;
        PlayerController.SegmentRemoved += DecreaseLensSize;
        Door.LevelLoaded += CreateConfinerCollider;
        ShakeCamera += StartShake;
    }

    private void OnDisable()
    {
        PlayerController.SegmentAdded -= IncreaseLensSize;
        PlayerController.SegmentRemoved -= DecreaseLensSize;
        Door.LevelLoaded -= CreateConfinerCollider;
        ShakeCamera -= StartShake;
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

    private void StartShake()
    {
        if (isShaking) return;

        StartCoroutine(Shake());
    }

    /// <summary>
    /// Shakes the camera and then restores it's default values.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shake()
    {
        float previousAmplitude = cmNoise.m_AmplitudeGain;
        float previousFrequency = cmNoise.m_FrequencyGain;

        cmNoise.m_AmplitudeGain = amplitudeGain;
        cmNoise.m_FrequencyGain = frequencyGain;
        isShaking = true;

        yield return new WaitForSeconds(duration);

        cmNoise.m_AmplitudeGain = previousAmplitude;
        cmNoise.m_FrequencyGain = previousFrequency;
        isShaking = false;
    }

    private void CreateConfinerCollider()
    {
        if (confiner == null)
        {
            confiner = new GameObject("Confiner");
            confiner.AddComponent<PolygonCollider2D>();
            confiner.layer = LayerMask.NameToLayer("Confiner");
        }

        PolygonCollider2D boundary = confiner.GetComponent<PolygonCollider2D>();
        Tilemap tileMap = FindObjectOfType<Tilemap>();
        tileMap.CompressBounds();

        Vector2[] path = new Vector2[4];

        path[0] = new Vector2(tileMap.cellBounds.xMin, tileMap.cellBounds.yMax);
        path[1] = new Vector2(tileMap.cellBounds.xMin, tileMap.cellBounds.yMin);
        path[2] = new Vector2(tileMap.cellBounds.xMax, tileMap.cellBounds.yMin);
        path[3] = new Vector2(tileMap.cellBounds.xMax, tileMap.cellBounds.yMax);
        boundary.pathCount = 1;
        boundary.SetPath(0, path);
        boundary.isTrigger = true;

        CinemachineConfiner2D cinemachineConfiner = cmCam.GetComponent<CinemachineConfiner2D>();

        cinemachineConfiner.m_BoundingShape2D = boundary;
        cinemachineConfiner.InvalidateCache();
    }
}
