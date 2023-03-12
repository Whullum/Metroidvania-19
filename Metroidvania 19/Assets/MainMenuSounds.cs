using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour
{
    [Header("Sounds Effect Events")]
    public AK.Wwise.Event uiHover;
    public AK.Wwise.Event uiConfirm;
    public AK.Wwise.Event uiSliderChange;
    public AK.Wwise.Event uiCancel;
    public AK.Wwise.Event uiStartGame;

    [Header("Music Events")]
    public AK.Wwise.Event menuMusic;
    public AK.Wwise.Event stopMenuMusic;

    private void Start()
    {
        menuMusic.Post(gameObject);
    }

    public void PlayHoverSound()
    {
        uiHover.Post(gameObject);
    }

    public void PlayConfirmSound()
    {
        uiConfirm.Post(gameObject);
    }

    public void PlaySliderChangeSound()
    {
        uiSliderChange.Post(gameObject);
    }

    public void PlayCancelSound()
    {
        uiCancel.Post(gameObject);
    }

    public void PlayStartGameSound()
    {
        uiStartGame.Post(gameObject);
    }

    public void StopMenuMusic()
    {
        stopMenuMusic.Post(gameObject);
    }
}
