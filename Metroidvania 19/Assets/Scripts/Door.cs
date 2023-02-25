using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Direction that the player will be looking in when they are spawned into a new room
public enum DoorDirection
{
    NORTH,
    SOUTH,
    EAST,
    WEST
}

[Serializable]
public class Door : MonoBehaviour
{
    [SerializeField] private int doorNumber;
    [SerializeField] private string connectingMapName;
    [SerializeField] private MapNode parentMapNode;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private DoorDirection playerSpawnDirection;

    public int DoorNumber { get => doorNumber; set => doorNumber = value; }

    private void Start()
    {
        mapManager = GameObject.FindObjectOfType<MapManager>();

        if (parentMapNode == null)
            parentMapNode = GetComponentInParent<MapNode>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Access the correct MapNode through the mapLevels dictionary
            MapNode connectingNode = mapManager.CurrentMap.mapLevels[connectingMapName];

            Instantiate(connectingNode.gameObject);

            // Spawn the player in the correct direction
            Vector3 startingDisplacement = new Vector3();
            switch (playerSpawnDirection)
            {
                // Doors are rotated, so use the door's local X axis to get the correct starting direction
                case DoorDirection.NORTH:
                case DoorDirection.WEST:
                    startingDisplacement = new Vector3(3, 0);
                    break;
                case DoorDirection.SOUTH:
                case DoorDirection.EAST:
                    startingDisplacement = new Vector3(-3, 0);
                    break;
            }

            // Recenter the minimap to the correct location
            mapManager.RecenterMinimap(playerSpawnDirection);

            // Spawn the player in front of the door with the matching doorNumber in the connected level/node
            Door connectingDoor = connectingNode.GetConnectingDoorNumber(this.doorNumber);
            collision.gameObject.transform.position = connectingDoor.transform.position + startingDisplacement;
            Destroy(this.parentMapNode.gameObject);
        }
    }
}