using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMusicProximity : MonoBehaviour
{
    public AK.Wwise.RTPC enemyProximity;
    public bool isInCombat = false;
    public float musicProximity;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isInCombat = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MusicManager.instance.combat.SetValue();
            isInCombat = !isInCombat;
            Debug.Log("Collide");
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.transform.position, this.gameObject.transform.position);

        if (distanceToPlayer < musicProximity && !isInCombat)
        {
            MusicManager.instance.approaching.SetValue();
            enemyProximity.SetValue(gameObject, distanceToPlayer);
            Debug.Log("Approaching Enemy");
        }
        else if (isInCombat == false)
        {
            MusicManager.instance.normal.SetValue();
        }
    }

    private void OnDestroy()
    {
        MusicManager.instance.normal.SetValue();
    }
}
