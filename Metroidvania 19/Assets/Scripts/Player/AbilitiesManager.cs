using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour
{
    public bool Projectile = false;
    public bool Melee = false;
    public bool Dash = false;


    // Update is called once per frame
    void Update()
    {
        GetComponent<PlayerMelee>().enabled= Melee;
        GetComponent<PlayerProjectile>().enabled = Projectile;
        GetComponent<PlayerDash>().enabled = Dash;
    }


}
