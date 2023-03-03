using System.Collections;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [Header("Drop Properties")]
    [SerializeField] private int minDropAmount;
    [SerializeField] private int maxDropAmount;
    [SerializeField] private float dropForce;
    [SerializeField] private float dropDelay;
    [Header("Drop GameObject")]
    [SerializeField] private DroppedObject dropPrefab;
    [SerializeField] private float dropPrefabMass;
    [SerializeField] private float dropPrefabGravityScale;
    [SerializeField] private float dropPrefabDrag;

    public void Drop(bool destroyGameObject)
    {
        int dropAmount = Random.Range(minDropAmount, maxDropAmount + 1);

        StartCoroutine(ToggleDrop(dropAmount, destroyGameObject));
    }

    private IEnumerator ToggleDrop(int amount, bool destroyGameObject)
    {
        int count = amount;

        while (count > 0)
        {
            DroppedObject newDrop = Instantiate(dropPrefab, transform.position, dropPrefab.transform.rotation);
            newDrop.Drop(dropPrefabMass, dropPrefabGravityScale, dropPrefabDrag, dropForce);

            count--;
            yield return new WaitForSeconds(dropDelay);
        }
        if (destroyGameObject)
            Destroy(gameObject);
    }
}
