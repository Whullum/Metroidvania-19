using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] protected MapManager mapManager;
    [SerializeField] private DoorLock doorLock;
    [SerializeField] private DoorDirection playerSpawnDirection;
    [SerializeField] private const int playerSpawnDistance = 5;

    public static Action LevelLoaded;

    public static Action<DoorDirection> PlayerTraveled;
    
    public int DoorNumber { get => doorNumber; set => doorNumber = value; }

    public DoorDirection PlayerSpawnDirection { get => playerSpawnDirection; set => playerSpawnDirection = value; }

    public static int PlayerSpawnDistance => playerSpawnDistance;

    public GameObject foregroundObj;

    private void Start()
    {
        mapManager = GameObject.FindObjectOfType<MapManager>();
        foregroundObj = GameObject.Find("FG");

        if (parentMapNode == null)
            parentMapNode = GetComponentInParent<MapNode>();
    }

    // private void Update() { // Used SOLELY for testing reasons
    //     if (doorLock != null && doorLock.isLocked) {
    //         GetComponent<SpriteRenderer>().color = Color.clear;
    //     } else {
    //         if (playerSpawnDirection == DoorDirection.NORTH) {
    //             GetComponent<SpriteRenderer>().color = Color.cyan;
    //         }
    //         else if (playerSpawnDirection == DoorDirection.SOUTH) {
    //             GetComponent<SpriteRenderer>().color = Color.blue;
    //         }
    //         else if (playerSpawnDirection == DoorDirection.EAST) {
    //             GetComponent<SpriteRenderer>().color = Color.green;
    //         }
    //         else if (playerSpawnDirection == DoorDirection.WEST) {
    //             GetComponent<SpriteRenderer>().color = Color.black;
    //         }
    //     }
    // }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && CheckDoorLockStatus())
        {
            StartCoroutine(transitionAnim(collision));
            // // Access the correct MapNode through the mapLevels dictionary
            // MapNode connectingNode = null;

            // if (mapManager.CurrentMap.mapLevels[connectingMapName] != null)
            // {
            //     connectingNode = mapManager.CurrentMap.mapLevels[connectingMapName];
            //     MinimapMovement.instance.RecenterMinimap(connectingMapName);

            //     Instantiate(connectingNode.gameObject);

            //     // Spawn the player in the correct direction
            //     Vector3 startingDisplacement = new Vector3();
            //     switch (playerSpawnDirection)
            //     {
            //         // Doors are rotated, so use the door's local X axis to get the correct starting direction
            //         case DoorDirection.NORTH:
            //             startingDisplacement = new Vector3(0, -playerSpawnDistance);
            //             break;
            //         case DoorDirection.EAST:
            //             startingDisplacement = new Vector3(-playerSpawnDistance, 0);
            //             break;
            //         case DoorDirection.SOUTH:
            //             startingDisplacement = new Vector3(0, playerSpawnDistance);
            //             break;
            //         case DoorDirection.WEST:
            //             startingDisplacement = new Vector3(playerSpawnDistance, 0);
            //             break;
            //     }

            //     // Recenter the minimap to the correct location
            //     //mapManager.RecenterMinimap(playerSpawnDirection);

            //     // Spawn the player in front of the door with the matching doorNumber in the connected level/node
            //     Door connectingDoor = connectingNode.GetConnectingDoorNumber(this.doorNumber);
            //     collision.gameObject.transform.position = connectingDoor.transform.position + startingDisplacement;
            //     collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

            //     LevelLoaded?.Invoke();

            //     Destroy(this.parentMapNode.gameObject);
            // }
            // else
            // {
            //     Debug.LogError("Warning: No map level with the name of " + connectingMapName + " located in mapLevels dictionary.");
            // }
        }
    }

    private IEnumerator transitionAnim(Collision2D collision) {
        yield return new WaitForEndOfFrame();
        LeanTween.alpha(foregroundObj.GetComponent<RectTransform>(), 1f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        // Access the correct MapNode through the mapLevels dictionary
        MapNode connectingNode = null;

        if (mapManager.CurrentMap.mapLevels[connectingMapName] != null)
        {
            connectingNode = mapManager.CurrentMap.mapLevels[connectingMapName];
            MinimapMovement.instance.RecenterMinimap(connectingMapName);

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
            //mapManager.RecenterMinimap(playerSpawnDirection);

            // Spawn the player in front of the door with the matching doorNumber in the connected level/node
            Door connectingDoor = connectingNode.GetConnectingDoorNumber(this.doorNumber);
            collision.gameObject.transform.position = connectingDoor.transform.position + startingDisplacement;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

            LevelLoaded?.Invoke();

            Destroy(this.parentMapNode.gameObject);
            foreach (PlayerHealthRestore meat in GameObject.FindObjectsOfType<PlayerHealthRestore>()) {
                Destroy(meat.gameObject);
            }

            GetTarget getTarget = GameObject.FindObjectOfType<GetTarget>();
            getTarget.targets = new List<Transform>();
        }
        else
        {
            Debug.LogError("Warning: No map level with the name of " + connectingMapName + " located in mapLevels dictionary.");
        }

        
        LeanTween.alpha(foregroundObj.GetComponent<RectTransform>(), 0f, 0.3f);
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