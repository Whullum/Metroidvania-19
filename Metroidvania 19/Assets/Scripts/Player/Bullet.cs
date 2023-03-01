using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLifeTime = 3;

    private void Awake()
    {

        Destroy(gameObject, bulletLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroys target
        //Destroy(collision.gameObject)
        if(collision.gameObject.tag != "Respawn" || collision.gameObject.tag != "Player")
            Destroy(gameObject);
    }
}
