using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;

    public GameObject pauseCanvasBackground;
    public GameObject pauseCanvasForeground;
    public GameObject pauseCanvasGroup;
    public GameObject settingsCanvasGroup;
    public GameObject controlsCanvasGroup;
    public GameObject gameHUDCanvasGroup;

    private void Start() {
        StartCoroutine(FadeInAnim());
    }

    private IEnumerator FadeInAnim() {
        yield return new WaitForEndOfFrame();
        LeanTween.alpha(pauseCanvasForeground.GetComponent<RectTransform>(), 0f, 1f);
        yield return new WaitForSeconds(1f);
        pauseCanvasForeground.SetActive(false);
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Paused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        gameHUDCanvasGroup.SetActive(true);
        pauseCanvasBackground.SetActive(false);
        pauseCanvasGroup.SetActive(false);
        settingsCanvasGroup.SetActive(false);
        controlsCanvasGroup.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void Pause() {
        gameHUDCanvasGroup.SetActive(false);
        pauseCanvasBackground.SetActive(true);
        pauseCanvasGroup.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void ShowSettings() {
        settingsCanvasGroup.SetActive(true);
        pauseCanvasGroup.SetActive(false);
    }

    public void ShowControls() {
        controlsCanvasGroup.SetActive(true);
        pauseCanvasGroup.SetActive(false);
    }

    public void Back() {
        settingsCanvasGroup.SetActive(false);
        controlsCanvasGroup.SetActive(false);
        pauseCanvasGroup.SetActive(true);
    }
}
