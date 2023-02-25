using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class MapNode : MonoBehaviour
{
    [SerializeField] private string nodeName;
    [SerializeField] private List<Door> doors;

    public string NodeName { get => nodeName; set => nodeName = value; }
    public List<Door> Doors { get => doors; set => doors = value; }

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
