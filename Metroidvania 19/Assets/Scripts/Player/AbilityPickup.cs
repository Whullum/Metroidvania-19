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

    public AK.Wwise.Event abilityPickupEvent;
    [SerializeField] private AbilityUnlock abilityToUnlock;
    [SerializeField] private DoorLock abilityPickupLock;
    [SerializeField] private Sprite abilitySprite;

    private void Start()
    {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + 1f, 1f).setLoopPingPong().setEaseInOutQuad();
        if (abilitySprite != null) 
            GetComponent<SpriteRenderer>().sprite = abilitySprite;

        ParticleSystem.MainModule particles = GetComponent<ParticleSystem>().main;
        AbilitiesManager abManager = GameObject.FindObjectOfType<AbilitiesManager>();

        switch (abilityToUnlock)
            {
                case AbilityUnlock.MELEE:
                    particles.startColor = Color.red;
                    this.gameObject.SetActive(!abManager.MeleeAbility);
                    break;
                case AbilityUnlock.PROJECTILE:
                    particles.startColor = new Color(0f, 255f, 255f, 255f);
                    this.gameObject.SetActive(!abManager.ProjectileAbility);
                    break;
                case AbilityUnlock.DASH:
                    particles.startColor = Color.yellow;
                    this.gameObject.SetActive(!abManager.DashAbility);
                    break;
            }
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
                    abilityManager.ProjectileAbility = true;
                    break;
                case AbilityUnlock.DASH:
                    abilityManager.DashAbility = true;
                    break;
            }

            if (abilityPickupLock != null) 
                abilityPickupLock.isLocked = true;

            abilityPickupEvent.Post(gameObject);

            Destroy(this.gameObject);
        }
    }
}
