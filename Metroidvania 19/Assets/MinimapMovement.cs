using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class StringTransformDictionary : SerializableDictionary<string, Transform> { }

public class MinimapMovement : MonoBehaviour
{
    public static MinimapMovement instance;

    [SerializeField]
    public StringTransformDictionary minimapPoints;
    public Transform startingTransform;
    public string startingRoomName;
    public GameObject minimapCameraGameObject;
    public GameObject playerPositionMarker;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        GetComponentInParent<MapManager>().cameraCeneteredName = startingRoomName;
        minimapCameraGameObject = GameObject.FindGameObjectWithTag("MiniMapCamera");
        minimapCameraGameObject.gameObject.transform.position = startingTransform.position;
        playerPositionMarker.transform.position = new Vector3(startingTransform.position.x, startingTransform.position.y);
    }

    public void RecenterMinimap(string mapName)
    {
        GetComponentInParent<MapManager>().cameraCeneteredName = mapName;
        minimapCameraGameObject.transform.position = minimapPoints[mapName].position;
        playerPositionMarker.transform.position = new Vector3(minimapPoints[mapName].position.x, minimapPoints[mapName].position.y);
    }
}
