using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWASD : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
    }
}
