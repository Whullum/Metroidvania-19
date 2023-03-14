using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Map currentMap;
    public Map CurrentMap { get => currentMap; set => currentMap = value; }

    public Camera mapCamera;
    public GameObject minimapCenter;
    private bool cameraIsZoomed = true;
    public string cameraCeneteredName;
    private int cameraZoomNear = 3;
    private int cameraZoomFar = 10;

    public GameObject fullmapImage;
    public GameObject miniMapImage;

    private void Start()
    {
        Instantiate(CurrentMap.minimapObject,gameObject.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            cameraIsZoomed = !cameraIsZoomed;

            if (cameraIsZoomed)
            {
                mapCamera.orthographicSize = cameraZoomNear;
                mapCamera.gameObject.transform.position = MinimapMovement.instance.minimapPoints[cameraCeneteredName].transform.position;
                minimapCenter.SetActive(true);

                fullmapImage.SetActive(false);
                miniMapImage.SetActive(true);
            }
            else
            {
                mapCamera.orthographicSize = cameraZoomFar;
                mapCamera.gameObject.transform.position = MinimapMovement.instance.minimapPoints["Center"].transform.position;
                minimapCenter.SetActive(false);

                fullmapImage.SetActive(true);
                miniMapImage.SetActive(false);
            }
        }
    }
}
