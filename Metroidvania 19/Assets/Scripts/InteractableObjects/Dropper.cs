using System.Collections;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [Header("Drop Properties")]
    [SerializeField] private Drop[] drop;
    [Header("Drop GameObject")]
    [SerializeField] private float dropForce = 5f;
    [SerializeField] private float dropDelay = 0f;
    [SerializeField] private float dropPrefabMass = 1f;
    [SerializeField] private float dropPrefabGravityScale = .2f;
    [SerializeField] private float dropPrefabDrag = 2f;

    public void Drop(bool destroyGameObject)
    {
        for (int i = 0; i < drop.Length; i++)
        {
            int dropCount = Random.Range(drop[i].MinDropAmount, drop[i].MaxDropAmount);

            for (int j = 0; j < dropCount; j++)
            {
                DroppedObject newDrop = Instantiate(drop[i].DropPrefab, transform.position, drop[i].DropPrefab.transform.rotation);
                newDrop.Drop(dropPrefabMass, dropPrefabGravityScale, dropPrefabDrag, dropForce);
            }
        }
        if (destroyGameObject)
            Destroy(gameObject);
    }
}

[System.Serializable]
public struct Drop
{
    public DroppedObject DropPrefab;
    public int MinDropAmount;
    public int MaxDropAmount;
}
