using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Direction that the player will be looking in when they are spawned into a new room
    private enum DoorDirection
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }

    [SerializeField] private int doorNumber;
    [SerializeField] private Door connectingDoor;
    [SerializeField] private MapNode parentMapNode;
    [SerializeField] private DoorDirection playerSpawnLocation;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LoadNewNodePrefab();

            Vector3 startingDisplacement = new Vector3();
            switch (playerSpawnLocation)
            {
                case DoorDirection.NORTH:
                    startingDisplacement = new Vector3(0, 3);
                    break;
                case DoorDirection.SOUTH:
                    startingDisplacement = new Vector3(0, -3);
                    break;
                case DoorDirection.EAST:
                    startingDisplacement = new Vector3(3, 0);
                    break;
                case DoorDirection.WEST:
                    startingDisplacement = new Vector3(-3, 0);
                    break;
            }
            collision.gameObject.transform.position = connectingDoor.transform.position + startingDisplacement;
        }
    }

    private void LoadNewNodePrefab()
    {
        Instantiate(connectingDoor.parentMapNode.MapPrefab);
        Destroy(this);
    }
}
