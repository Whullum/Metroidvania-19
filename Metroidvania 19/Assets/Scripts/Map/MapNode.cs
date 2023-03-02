using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class MapNode : MonoBehaviour
{
    [SerializeField] private List<Door> doors;

    public List<Door> Doors { get => doors; set => doors = value; }

    private void Start()
    {
        Door[] doorsInScene = FindObjectsOfType<Door>();

        // If the list of doors is empty, add all doors in the current prefab to the doors list
        if (doors == null || doors.Count == 0)
            doors.AddRange(doorsInScene);
        else
        {
            // Otherwise, check to see if all doors in scene are added to the doors list. If the door is not included, add it to the list
            foreach (Door door in doorsInScene)
            {
                if (!(doors.Contains(door)))
                {
                    doors.Add(door);
                }
            }
        }
    }

    public Door GetConnectingDoorNumber(int doorNumber)
    {
        foreach (Door door in Doors)
        {
            if (doorNumber == door.DoorNumber)
            {
                Debug.Log("Door found.");
                return door;
            }
        }

        Debug.LogError("No corresponding door number found in level.");
        return null;
    }
}
