using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            foreach (PlayerHealthRestore meat in GameObject.FindObjectsOfType<PlayerHealthRestore>()) {
                Destroy(meat.gameObject);
            }
            MusicManager.instance.StopMusic();
            SceneManager.LoadScene("Start Menu");
        }

        
    }
}
