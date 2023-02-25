using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
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
    [SerializeField] private string connectingMapName;
    [SerializeField] private MapNode parentMapNode;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private DoorDirection playerSpawnLocation;

    private const float mapXIncrement = 100f;
    private const float mapYIncrement = 60f;

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
            MapNode connectingNode = GetConnectingNode();

            Instantiate(connectingNode.gameObject);

            Vector3 startingDisplacement = new Vector3();
            Vector3 maskImagePosition = mapManager.maskImage.transform.position;
            switch (playerSpawnLocation)
            {
                case DoorDirection.NORTH:
                    mapManager.maskImage.transform.position = new Vector3(maskImagePosition.x, maskImagePosition.y + mapYIncrement, 0);
                    startingDisplacement = new Vector3(3, 0);
                    break;
                case DoorDirection.SOUTH:
                    mapManager.maskImage.transform.position = new Vector3(maskImagePosition.x, maskImagePosition.y - mapYIncrement, 0);
                    startingDisplacement = new Vector3(-3, 0);
                    break;
                case DoorDirection.EAST:
                    mapManager.maskImage.transform.position = new Vector3(maskImagePosition.x + mapXIncrement, maskImagePosition.y, 0);
                    startingDisplacement = new Vector3(-3, 0);
                    break;
                case DoorDirection.WEST:
                    mapManager.maskImage.transform.position = new Vector3(maskImagePosition.x - mapXIncrement, maskImagePosition.y, 0);
                    startingDisplacement = new Vector3(3, 0);
                    break;
            }

            Door connectingDoor = connectingNode.GetConnectingDoorNumber(this.doorNumber);
            collision.gameObject.transform.position = connectingDoor.transform.position + startingDisplacement;

            Destroy(this.parentMapNode.gameObject);
        }
    }

    private MapNode GetConnectingNode()
    {
        return mapManager.CurrentMap.mapLevels[connectingMapName];
    }
}
