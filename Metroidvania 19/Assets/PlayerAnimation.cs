using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator playerAnimator;

    public void TriggerBiteAnimaton()
    {
        playerAnimator.SetTrigger("BiteTrigger");
    }

    public void ResetTriggerBiteAnimation()
    {
        playerAnimator.ResetTrigger("BiteTrigger");
    }
}
