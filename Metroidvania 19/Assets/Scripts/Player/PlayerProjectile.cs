using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(GameObject))]
public class PlayerProjectile : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletObject;


    [SerializeField] float bulletSpeed = 20;
    [SerializeField] float lifeTime = 3;
    [SerializeField] float bulletSize = 1;
    void Update()
    {
        //if(Input.GetMouseButton(1))
        if(Input.GetMouseButtonDown(1))
        {
            var bulletInstance = Instantiate(bulletObject, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bulletInstance.GetComponent<Bullet>().bulletLifeTime = lifeTime;
            bulletInstance.transform.localScale = new Vector3(bulletSize,bulletSize,0);

            bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletSpeed;
        }
    }
}
