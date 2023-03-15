using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AK.Wwise.Event biteSound;

    public void PlayBiteSound()
    {
        biteSound.Post(gameObject);
    }
}
