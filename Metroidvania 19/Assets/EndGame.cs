using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    public GameObject evilHead;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            foreach (PlayerHealthRestore meat in GameObject.FindObjectsOfType<PlayerHealthRestore>()) {
                Destroy(meat.gameObject);
            }
            LeanTween.moveLocalY(evilHead, evilHead.transform.position.y + 8, 1f).setEaseOutExpo();

            if (GameObject.FindObjectOfType<PauseMenu>() != null)
                GameObject.FindObjectOfType<PauseMenu>().endgame = true;
                if (collision.gameObject.GetComponent<IDamageable>().Damage(1000)) {
                    collision.gameObject.GetComponent<IDamageable>().Death();
            }
        }

        
    }
}
