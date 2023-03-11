using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransitionDoor : Door
{
    [SerializeField] private Map newMapData;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mapManager.CurrentMap = newMapData;

        base.OnCollisionEnter2D(collision);
    }
}
