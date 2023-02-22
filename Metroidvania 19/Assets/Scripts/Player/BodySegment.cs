using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    private Coroutine coroutine;
    private Rigidbody2D rBody;
    public List<SegmentTransform> Path = new List<SegmentTransform>();

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        AddPath();
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.IsMoving)
            AddPath();
    }

    private void AddPath() => Path.Add(new SegmentTransform(transform.position, transform.rotation));

    public void Move(SegmentTransform previousPath, SegmentTransform nextPath, float distanceToHead)
    {
        Vector3 startPosition = previousPath.Position;
        Vector3 newPos = nextPath.Position;
        Quaternion startRotation = previousPath.Rotation;
        Quaternion newRot = nextPath.Rotation;

        Vector2 movePos = Vector2.Lerp(startPosition, newPos, distanceToHead);
        Quaternion moveRot = Quaternion.Slerp(startRotation, newRot, distanceToHead);
        rBody.MovePosition(movePos);
        //rBody.MoveRotation(moveRot);

        /*if (coroutine == null)
            coroutine = StartCoroutine(Follow(previousPath, nextPath, distanceToHead));*/


    }

    private IEnumerator Follow(SegmentTransform previousPath, SegmentTransform nextPath, float distanceToHead)
    {
        Vector3 startPosition = previousPath.Position;
        Vector3 newPos = nextPath.Position;
        Quaternion startRotation = previousPath.Rotation;
        Quaternion newRot = nextPath.Rotation;
        float distance = distanceToHead;

        while (distance > 0)
        {
            distance -= Time.deltaTime;
            Vector2 movePos = Vector2.Lerp(startPosition, newPos, distance / distanceToHead);
            Quaternion moveRot = Quaternion.Slerp(startRotation, newRot, distance / distanceToHead);
            rBody.MovePosition(movePos);
            rBody.MoveRotation(moveRot);

            yield return null;
        }

        coroutine = null;
    }
}

public struct SegmentTransform
{
    public Vector3 Position;
    public Quaternion Rotation;

    public SegmentTransform(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}