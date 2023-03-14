using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransitionDoor : Door
{
    [SerializeField] private Map newMapData;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(MinimapMovement.instance.gameObject);

            mapManager.CurrentMap = newMapData;

            Instantiate(mapManager.CurrentMap.minimapObject, mapManager.gameObject.transform);

            MinimapMovement.instance.RecenterMinimap(newMapData.minimapObject.GetComponent<MinimapMovement>().startingRoomName);
        }

        base.OnCollisionEnter2D(collision);
    }
}
