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
public class Door : InteractableObject
{
    [SerializeField] private int doorNumber;
    [SerializeField] private string connectingMapName;
    [SerializeField] private MapNode parentMapNode;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private DoorLock doorLock;
    [SerializeField] private DoorDirection playerSpawnDirection;
    [SerializeField] private const int playerSpawnDistance = 5;

    public int DoorNumber { get => doorNumber; set => doorNumber = value; }

    private void Start()
    {
        mapManager = GameObject.FindObjectOfType<MapManager>();

        if (parentMapNode == null)
            parentMapNode = GetComponentInParent<MapNode>();


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && CheckDoorLockStatus())
        {
            // Access the correct MapNode through the mapLevels dictionary
            MapNode connectingNode = null;

            if (mapManager.CurrentMap.mapLevels[connectingMapName] != null)
            {
                connectingNode = mapManager.CurrentMap.mapLevels[connectingMapName];

                Instantiate(connectingNode.gameObject);

                // Spawn the player in the correct direction
                Vector3 startingDisplacement = new Vector3();
                switch (playerSpawnDirection)
                {
                    // Doors are rotated, so use the door's local X axis to get the correct starting direction
                    case DoorDirection.NORTH:
                        startingDisplacement = new Vector3(0, -playerSpawnDistance);
                        break;
                    case DoorDirection.EAST:
                        startingDisplacement = new Vector3(-playerSpawnDistance, 0);
                        break;
                    case DoorDirection.SOUTH:
                        startingDisplacement = new Vector3(0, playerSpawnDistance);
                        break;
                    case DoorDirection.WEST:
                        startingDisplacement = new Vector3(playerSpawnDistance, 0);
                        break;
                }

                // Recenter the minimap to the correct location
                mapManager.RecenterMinimap(playerSpawnDirection);

                // Spawn the player in front of the door with the matching doorNumber in the connected level/node
                Door connectingDoor = connectingNode.GetConnectingDoorNumber(this.doorNumber);
                collision.gameObject.transform.position = connectingDoor.transform.position + startingDisplacement;
                Destroy(this.parentMapNode.gameObject);
            }
            else
            {
                Debug.LogError("Warning: No map level with the name of " + connectingMapName + " located in mapLevels dictionary.");
            }
        }
    }

    private bool CheckDoorLockStatus()
    {
        if (doorLock == null)
            return true;
        else
            return !doorLock.isLocked;
    }

    public override void OnActivation()
    {
        if (doorLock != null)
        {
            doorLock.isLocked = false;
        }
    }

    public override void OnDeactivation()
    {
        
    }
}