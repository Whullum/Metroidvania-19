using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public GameObject enemyTest;
    private GameObject currentEnemyTest;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentEnemyTest = Instantiate(enemyTest);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Destroy(currentEnemyTest);
        }
    }
}
