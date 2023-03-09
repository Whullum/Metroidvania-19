using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool Paused = false;

    public GameObject menuCanvasBackground;
    public GameObject menuCanvasForeground;
    public GameObject menuCanvasList;
    public GameObject mainCanvasGroup;
    public GameObject settingsCanvasGroup;
    public GameObject controlsCanvasGroup;
    public GameObject creditsCanvasGroup;

    // Update is called once per frame
    private void Start() {
        StartCoroutine(FadeInAnim());
    }

    private IEnumerator FadeInAnim() {
        yield return new WaitForEndOfFrame();
        LeanTween.alpha(menuCanvasForeground.GetComponent<RectTransform>(), 0f, 3f);
        yield return new WaitForSeconds(3f);
        LeanTween.moveLocalX(menuCanvasList, -600f, 0.5f).setEaseOutExpo();
        yield return new WaitForSeconds(1f);
        menuCanvasForeground.SetActive(false);
    }

    public void FadeOut() {
        StartCoroutine(FadeOutAnim());
    }

    private IEnumerator FadeOutAnim() {
        yield return new WaitForEndOfFrame();
         menuCanvasForeground.SetActive(true);
        LeanTween.alpha(menuCanvasForeground.GetComponent<RectTransform>(), 1f, 3f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Stage1_Containment");
    }

    public void ShowSettings() {
        settingsCanvasGroup.SetActive(true);
        mainCanvasGroup.SetActive(false);
    }

    public void ShowControls() {
        controlsCanvasGroup.SetActive(true);
        mainCanvasGroup.SetActive(false);
    }

    public void ShowCredits() {
        creditsCanvasGroup.SetActive(true);
        mainCanvasGroup.SetActive(false);
    }

    public void Back() {
        settingsCanvasGroup.SetActive(false);
        controlsCanvasGroup.SetActive(false);
        creditsCanvasGroup.SetActive(false);
        mainCanvasGroup.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
