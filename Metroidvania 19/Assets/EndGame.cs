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
            FadeOut();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAnim());
    }

    private IEnumerator FadeOutAnim()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Start Menu");
    }
}
