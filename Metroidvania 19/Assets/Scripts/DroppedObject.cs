using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DroppedObject : MonoBehaviour
{
    public void Drop(float mass, float gavityScale, float drag, float dropForce)
    {
        Rigidbody2D rBdoy = GetComponent<Rigidbody2D>();

        rBdoy.mass = mass;
        rBdoy.gravityScale = gavityScale;
        rBdoy.drag = drag;
        rBdoy.AddForce(Random.insideUnitCircle.normalized * dropForce, ForceMode2D.Impulse);
    }
}
