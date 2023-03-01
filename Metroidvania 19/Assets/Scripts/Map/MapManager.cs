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

    private const float miniMapXIncrement = 100f;
    private const float miniMapYIncrement = 50f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            fullMapImage.SetActive(!fullMapImage.activeSelf);
            miniMapImage.SetActive(!miniMapImage.activeSelf);
        }
    }

    public void RecenterMinimap(DoorDirection direction)
    {
        RectTransform maskImageRect = maskImage.GetComponent<RectTransform>();

        switch (direction)
        {
            case DoorDirection.NORTH:
                maskImageRect.position = new Vector3(maskImage.transform.position.x, maskImage.transform.position.y + miniMapYIncrement, 0);                
                break;
            case DoorDirection.SOUTH:
                maskImageRect.position = new Vector3(maskImage.transform.position.x, maskImage.transform.position.y - miniMapYIncrement, 0);
                break;
            case DoorDirection.EAST:
                maskImageRect.position = new Vector3(maskImage.transform.position.x + miniMapXIncrement, maskImage.transform.position.y, 0);
                break;
            case DoorDirection.WEST:
                maskImageRect.position = new Vector3(maskImage.transform.position.x - miniMapXIncrement, maskImage.transform.position.y, 0);
                break;
        }
    }
}
