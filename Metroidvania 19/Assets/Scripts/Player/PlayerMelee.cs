using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    GameObject HurtBox;
    
    public float hurtBoxTime = 2f;
    float cooldown = 0;
    void Start()
    {
        HurtBox = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            Debug.Log("Right MouseClick");
            cooldown = Time.time + hurtBoxTime;
            HurtBox.SetActive(true);
        }
        else if(Time.time > cooldown)
        {
            HurtBox.SetActive(false);
            
        }
    }
}
