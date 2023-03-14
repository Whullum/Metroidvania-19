using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(BodySegment))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour, IDamageable
{
    #region variables

    /// <summary>
    /// Invoked when the player recovers/adds a new segment to the body.
    /// </summary>
    public static Action SegmentAdded;

    /// <summary>
    /// Invoked when the player losses a segment from the body.
    /// </summary>
    public static Action SegmentRemoved;

    public static List<BodySegment> BodyParts { get { return bodyParts; } }
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int SegmentHealth { get { return segmentHealth; } set { segmentHealth = value; } }
    public int StartingBodySize { get { return startingBodySize; } set { startingBodySize = value; } }

    private static List<BodySegment> bodyParts;
    private HealthShader shader;
    private int minimumBodySize = 2;
    public int currentHealth;
    private int maxHealth;

    [Header("Player Stats")]

    [Tooltip("Amount of body segments the player starts with.")]
    [Range(2, 15)] [SerializeField] private int startingBodySize;

    [Header("Body Segments")]

    [Tooltip("Amount of health each body segment has.")]
    [Range(1, 100)] [SerializeField] private int segmentHealth;

    [Header("Body Segments")]

    [Tooltip("Default body segment used to extend the player body.")]
    [SerializeField] private GameObject bodySegment;
    [Tooltip("Base body segment, created after the head when the body is > than 2 segments.")]
    [SerializeField] private GameObject baseBodySegment;
    [Tooltip("Tail segment of the player.")]
    [SerializeField] private GameObject tailSegment;
    [Tooltip("Particle effect used when a new segment is added to the body.")]
    [SerializeField] private ParticleSystem segmentAddedEffect;
    [Tooltip("Particle effect used when a segment is decoupled from the body.")]
    [SerializeField] private ParticleSystem segmentDecoupledEffect;

    #endregion

    private void Start()
    {
        bodyParts = new List<BodySegment>();
        shader = FindObjectOfType<HealthShader>();
        CreateInitialBody();
    }

    /// <summary>
    /// Initializes the body with the minimum shape it will have: HEAD - BASE BODY - BODY SEGMENT - TAIL.
    /// </summary>
    public void CreateInitialBody()
    {
        bodyParts.Add(GetComponent<BodySegment>());

        for (int i = 1; i < startingBodySize; i++)
        {
            Debug.LogError("Bing bing " + i);
            AddBodySegment();
        }
        maxHealth = segmentHealth * bodyParts.Count;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Creates and adds a body segment to the player. Calculates the new health of the player.
    /// </summary>
    /// <param name="partToAdd">Prefab of the new part to be created.</param>
    /// <param name="bodyIndex">Body position where this part will go.</param>
    private void AddBodySegment()
    {
        GameObject segmentPrefab = bodySegment;
        int bodyIndex = bodyParts.Count - 1;

        // If only the head is present, we create the tail segment
        if (bodyParts.Count == 1)
        {
            segmentPrefab = tailSegment;
            bodyIndex = bodyParts.Count;
        } else {
            Debug.LogError("POOP");
            shader.ToggleSegment(bodyIndex - 1, true);
        }
        // If the head and the tail are present, we create the base body segment
        if (bodyParts.Count == 2) 
        {
            segmentPrefab = baseBodySegment;
        }

        // Creates a new body segment depending on the actual body size. If the body size is large enough, the default body segment will be created
        BodySegment newSegment = Instantiate(segmentPrefab, bodyParts[bodyParts.Count - 1].transform.position, bodyParts[bodyParts.Count - 1].transform.rotation, transform).GetComponent<BodySegment>();
        newSegment.Player = this;
        bodyParts.Insert(bodyIndex, newSegment);
        

        // Calculate the new maxHealth with the new body size
        CalculateMaxHealth();
        currentHealth = maxHealth;

        // Effect for adding a new body part
        Instantiate(segmentAddedEffect, newSegment.transform.position, Quaternion.identity, newSegment.transform);

        CalculateSortingOrder();

        SegmentAdded?.Invoke();
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
    }

    /// <summary>
    /// Removes the last body segment from the player body.
    /// </summary>
    private void RemoveBodySegment(int bodyIndex)
    {
        BodySegment segmentToRemove = bodyParts[bodyIndex];
        bodyParts.RemoveAt(bodyIndex);
        shader.ToggleSegment(bodyIndex - 1, false);

        CalculateSortingOrder();

        Instantiate(segmentDecoupledEffect, segmentToRemove.transform.position, Quaternion.identity, segmentToRemove.transform);
        segmentToRemove.Decouple();

        CalculateMaxHealth();
        SegmentRemoved?.Invoke();
    }

    private void CalculateMaxHealth()
    {
        maxHealth = segmentHealth * bodyParts.Count;
    }

    /// <summary>
    /// Restores the specified amount of health and regrows the body segments acordingly.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void RestoreHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        float remainingSegmentHealth = (currentHealth % segmentHealth) * (100/segmentHealth);

        if (currentHealth > segmentHealth * minimumBodySize) {
            remainingSegmentHealth = (remainingSegmentHealth <= 0) ? 100 : remainingSegmentHealth;
            shader.DepleteSegment(bodyParts.Count - minimumBodySize, remainingSegmentHealth);
        } else {
            shader.DepleteHeadAndTail((currentHealth * 100f / segmentHealth));
        }
    }

    public void GrowBody()
    {
        AddBodySegment();
    }

    public bool Damage(int amount)
    {
        currentHealth -= amount;

        CameraController.ShakeCamera?.Invoke();

        // If the player has no health, they die
        if (currentHealth <= 0) {
            shader.DepleteHeadAndTail(0);
            return true;
        }
            
        // Calculate the amount of segments to destroy
        float segmentsLeft = (float)currentHealth / (float)segmentHealth;
        float segmentsToDestroy = Mathf.FloorToInt(bodyParts.Count - segmentsLeft);

        if (segmentsToDestroy > 0)
        {
            for (int i = 0; i < segmentsToDestroy; i++)
            {
                // If the amount of segments to destroy is more than the actual minimum size of the body, we can't destroy those
                if (bodyParts.Count > minimumBodySize)
                    RemoveBodySegment(bodyParts.Count - minimumBodySize);
                
            }
        } 

        float remainingSegmentHealth = (currentHealth % segmentHealth) * (100/segmentHealth);

        if (currentHealth > segmentHealth * minimumBodySize) {
            remainingSegmentHealth = (remainingSegmentHealth <= 0) ? 100 : remainingSegmentHealth;
            shader.DepleteSegment(bodyParts.Count - minimumBodySize, remainingSegmentHealth);
        } else {
            shader.DepleteHeadAndTail((currentHealth * 100f / segmentHealth));
        }

        return false;
    }

    public void Death()
    {
        for (int i = 1; i < bodyParts.Count; i++)
        {
            RemoveBodySegment(i);
        }
        GetComponent<PlayerMovement>().enabled = false;

        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.gravityScale = .2f;
        body.AddTorque(transform.up.x * body.velocity.magnitude);
        StartCoroutine(FindObjectOfType<PauseMenu>().FadeOutAnim());
    }
}
