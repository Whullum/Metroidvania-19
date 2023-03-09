using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Map currentMap;

    // Start is called before the first frame update
    void Start()
    {
        currentMap.mapMusic.Post(gameObject);
    }
}
