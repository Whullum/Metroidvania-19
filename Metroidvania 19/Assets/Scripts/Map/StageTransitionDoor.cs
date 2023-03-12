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
            Debug.Log("OldMap: " + mapManager.CurrentMap);
            mapManager.CurrentMap = newMapData;
            Debug.Log("NewMap: " + mapManager.CurrentMap);
        }

        base.OnCollisionEnter2D(collision);
    }
}
