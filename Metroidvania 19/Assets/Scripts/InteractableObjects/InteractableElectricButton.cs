using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableElectricButton : InteractableButton
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (!isActivated)
                OnActivation();
            else
                OnDeactivation();
        }
    }
}
