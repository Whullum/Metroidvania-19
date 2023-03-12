using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoadData", menuName = "ScriptableObjects/SceneLoadData", order = 3)]
public class SceneLoadData : ScriptableObject
{
    public string sceneName;
    public int startingDoorNumber;
    public GameObject startingLevelPrefab;

    public void SpawnPlayer(int doorNumber)
    {
        Door[] doorsInLevel =  startingLevelPrefab.GetComponentsInChildren<Door>();
        Door connectingDoor = null;

        foreach (Door door in doorsInLevel)
        {
            if (door.DoorNumber == doorNumber)
            {
                connectingDoor = door;
                break;
            }
        }

        // Spawn the player in front of the door with the matching doorNumber in the connected level/node
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (connectingDoor != null)
        {
            // Spawn the player in the correct direction
            Vector3 startingDisplacement = new Vector3();
            switch (connectingDoor.PlayerSpawnDirection)
            {
                // Doors are rotated, so use the door's local X axis to get the correct starting direction
                case DoorDirection.NORTH:
                    startingDisplacement = new Vector3(0, -Door.PlayerSpawnDistance);
                    break;
                case DoorDirection.EAST:
                    startingDisplacement = new Vector3(-Door.PlayerSpawnDistance, 0);
                    break;
                case DoorDirection.SOUTH:
                    startingDisplacement = new Vector3(0, Door.PlayerSpawnDistance);
                    break;
                case DoorDirection.WEST:
                    startingDisplacement = new Vector3(Door.PlayerSpawnDistance, 0);
                    break;
            }

            player.transform.position = connectingDoor.transform.position + startingDisplacement;
        }
        else
        {
            player.transform.position = new Vector2(16, 20);
        }
    }
}
