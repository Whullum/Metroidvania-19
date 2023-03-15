using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(GameObject))]
public class PlayerProjectile : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletObject;
    private AbilitiesShader shader;

    [Tooltip("Speed of bullet")]
    [SerializeField] float bulletSpeed = 20;

    [Tooltip("Life time of bullet in seconds")]
    [SerializeField] float lifeTime = 3;

    
    [SerializeField] float bulletSize = 1;


    [Tooltip("Range for the angles each bullet can randomly fire in")]
    [SerializeField] float spreadAngle = 15f;

    [Tooltip("Length of cooldown")]
    public float timeInactive = 2f;

    [Tooltip("Number of bullets that can be fired before cooldown")]
    [SerializeField] int bulletLimit = 5;

    [Tooltip("The time between eachshot fired in seconds")]
    [SerializeField] float fireRate = 2f;


    float cooldown = 0f;
    int bulletCount = 0;
    bool firingEnabled = true;

    void Awake() {
        shader = FindObjectOfType<AbilitiesShader>();
    }
    
    void Update()
    {
        //if(Input.GetMouseButton(1))
        if(Input.GetKeyDown(KeyCode.E) && bulletCount < bulletLimit )
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

    private void OnEnable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(1, true)); }
    }

    private void OnDisable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(1, false)); }
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
        StartCoroutine(shader.ProjectileCooldown((int)timeInactive));
        
        var bulletInstance = Instantiate(bulletObject, bulletSpawnPoint.position, bulletSpawnPoint.rotation * Quaternion.Euler(0,0,90f));
        bulletInstance.GetComponent<Bullet>().BulletLifeTime = lifeTime;
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
