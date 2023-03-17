using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;

    public GameObject pauseCanvasBackground;
    public GameObject pauseCanvasForeground;
    public GameObject pauseCanvasGroup;
    public GameObject settingsCanvasGroup;
    public GameObject controlsCanvasGroup;
    public GameObject gameHUDCanvasGroup;

    public AK.Wwise.Event uiHover;
    public AK.Wwise.Event uiConfirm;
    public AK.Wwise.Event uiCancel;

    private void Start() {
        StartCoroutine(FadeInAnim());
    }

    private IEnumerator FadeInAnim() {
        yield return new WaitForEndOfFrame();
        LeanTween.alpha(pauseCanvasForeground.GetComponent<RectTransform>(), 1f, 0f);
        yield return new WaitForEndOfFrame();
        LeanTween.alpha(pauseCanvasForeground.GetComponent<RectTransform>(), 0f, 2f).setEaseInExpo();
        yield return new WaitForSeconds(2f);
        LeanTween.scale(gameHUDCanvasGroup, new Vector3(1.2f, 1.2f, 1.2f), 1f).setEaseOutQuad();
    }

    public IEnumerator FadeOutAnim() {
        yield return new WaitForEndOfFrame();
        PlayerController player = FindObjectOfType<PlayerController>();
        LeanTween.alpha(pauseCanvasForeground.GetComponent<RectTransform>(), 1f, 3f);
        LeanTween.scale(gameHUDCanvasGroup, new Vector3(2f, 2f, 2f), 3f).setEaseOutQuad();
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayHover()
    {
        uiHover.Post(gameObject);
    }

    public void PlayCancel()
    {
        uiCancel.Post(gameObject);
    }

    public void PlayConfirm()
    {
        uiConfirm.Post(gameObject);
    }
}
