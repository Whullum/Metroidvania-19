using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(GameObject))]
public class PlayerProjectile : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletObject;

    [Tooltip("Speed of bullet")]
    [SerializeField] float bulletSpeed = 20;

    [Tooltip("Life time of bullet in seconds")]
    [SerializeField] float lifeTime = 3;

    
    [SerializeField] float bulletSize = 1;


    [Tooltip("Range for the angles each bullet can randomly fire in")]
    [SerializeField] float spreadAngle = 15f;

    [Tooltip("Length of cooldown")]
    public float timeInactive = 0f;

    [Tooltip("Number of bullets that can be fired before cooldown")]
    [SerializeField] int bulletLimit = 5;

    [Tooltip("The time between eachshot fired in seconds")]
    [SerializeField] float fireRate = 1f;


    float cooldown = 0f;
    int bulletCount = 0;
    bool firingEnabled = true;
    

    void Update()
    {
        //if(Input.GetMouseButton(1))
        if(Input.GetKeyDown(KeyCode.X) && bulletCount < bulletLimit )
        {
            if(firingEnabled)
                StartCoroutine("Fire");

        }
        else if(bulletCount >= bulletLimit && cooldown == 0f)
        {
            Debug.Log(Time.time);
            cooldown = Time.time + timeInactive;
            Debug.Log(cooldown);
        }else if(Time.time > cooldown && bulletCount >= bulletLimit)
        {
            Debug.Log("Reloaded");
            cooldown = 0f;
            bulletCount= 0;
            
        }
    }


    /// <summary>
    /// <header>Couroutine for firing bullets </header>
    /// Creates bullet based on parameters and pausing firing between each shot.
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator Fire()
   {
        
        firingEnabled = false;
        bulletCount++;
        Debug.Log("Bullet num: " + bulletCount);
        bulletSpawnPoint.localRotation = Quaternion.Euler(bulletSpawnPoint.localRotation.x, bulletSpawnPoint.localRotation.y, Random.Range(-90f - spreadAngle, -90f + spreadAngle));

        
        
        var bulletInstance = Instantiate(bulletObject, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bulletInstance.GetComponent<Bullet>().bulletLifeTime = lifeTime;
        bulletInstance.transform.localScale = new Vector3(bulletSize, bulletSize, 0);

        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletSpawnPoint.up * bulletSpeed;
        
        yield return WaitFire();
    }

    IEnumerator WaitFire()
    {
        yield return new WaitForSeconds(fireRate);
        firingEnabled = true;
    }
}
