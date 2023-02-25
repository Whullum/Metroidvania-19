using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(BodySegment))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour, IDamageable
{
    public static  List<BodySegment> BodyParts { get { return bodyParts; } }
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    private static List<BodySegment> bodyParts = new List<BodySegment>();
    private int currentHealth;

    [Range(3, 100)]
    [SerializeField] private int segmentHealth = 2;
    [SerializeField] private int bodySize = 3;
    [SerializeField] private GameObject bodySegment;
    [SerializeField] private GameObject baseBodySegment;
    [SerializeField] private GameObject tailSegment;
    [SerializeField] private ParticleSystem segmentAddedEffect;
    [SerializeField] private ParticleSystem segmentDestroyedEffect;

    private void Start()
    {
        CreateBody();
        currentHealth = segmentHealth * bodySize;
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
    }

    /// <summary>
    /// Removes the last body segment from the player body.
    /// </summary>
    private void RemoveBodySegment()
    {
        GameObject tail = bodyParts[bodyParts.Count - 1].gameObject;
        bodyParts.RemoveAt(bodyParts.Count - 1);
        GameObject previousTailSegment = bodyParts[bodyParts.Count - 1].gameObject;
        bodyParts.RemoveAt(bodyParts.Count - 1);

        BodySegment newTail = Instantiate(tailSegment, previousTailSegment.transform.position, previousTailSegment.transform.rotation, transform).GetComponent<BodySegment>();
        bodyParts.Add(newTail);

        CalculateSortingOrder();

        Destroy(tail);
        Destroy(previousTailSegment);
    }

    public bool Damage(int amount)
    {
        if (CurrentHealth - amount <= 0)
            return true;
        return false;
    }

    public void Death()
    {

    }
}
