using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private string nodeName;
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private List<MapNode> connectingRooms;
    [SerializeField] private List<Door> doors;

    public string NodeName { get => nodeName; set => nodeName = value; }
    public GameObject MapPrefab { get => mapPrefab; set => mapPrefab = value; }
    public List<MapNode> ConnectingRooms { get => connectingRooms; set => connectingRooms = value; }
    public List<Door> Doors { get => doors; set => doors = value; }
}
