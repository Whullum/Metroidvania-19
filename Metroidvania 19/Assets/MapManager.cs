using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Map currentMap;
    public Map CurrentMap { get => currentMap; set => currentMap = value; }

    public GameObject fullMapImage;
    public GameObject miniMapImage;
    public GameObject maskImage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            fullMapImage.SetActive(!fullMapImage.activeSelf);
            miniMapImage.SetActive(!miniMapImage.activeSelf);
        }
    }
}
