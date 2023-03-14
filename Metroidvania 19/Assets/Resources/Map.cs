using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class StringMapNodeDictionary : SerializableDictionary<string, MapNode> { }

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class Map : ScriptableObject
{
    public GameObject minimapObject;

    [SerializeField]
    public StringMapNodeDictionary mapLevels;
    public List<Door> mapDoors;
    public AK.Wwise.Event mapMusic;
}
