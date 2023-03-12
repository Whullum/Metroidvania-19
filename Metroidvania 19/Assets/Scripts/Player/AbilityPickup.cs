using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    private enum AbilityUnlock
    {
        MELEE,
        PROJECTILE,
        DASH
    }

    [SerializeField] private AbilityUnlock abilityToUnlock;
    [SerializeField] private DoorLock abilityPickupLock;

    private void Start()
    {
        this.gameObject.SetActive(!abilityPickupLock.isLocked);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AbilitiesManager abilityManager = collision.gameObject.GetComponent<AbilitiesManager>();

            switch (abilityToUnlock)
            {
                case AbilityUnlock.MELEE:
                    abilityManager.MeleeAbility = true;
                    break;
                case AbilityUnlock.PROJECTILE:
                    abilityManager.MeleeAbility = true;
                    break;
                case AbilityUnlock.DASH:
                    abilityManager.MeleeAbility = true;
                    break;
            }

            abilityPickupLock.isLocked = true;

            Destroy(this.gameObject);
        }
    }
}
