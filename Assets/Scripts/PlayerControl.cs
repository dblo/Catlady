using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public float movementSpeed = 0.01f;

    void Start () {
		
	}
	
	void Update () {
        if (Input.GetKey("w"))
        {
            transform.position += new Vector3(0, movementSpeed);
        }
        if (Input.GetKey("s"))
        {
            transform.position += new Vector3(0, -movementSpeed);
        }
        if (Input.GetKey("a"))
        {
            transform.position += new Vector3(-movementSpeed, 0);
        }
        if (Input.GetKey("d"))
        {
            transform.position += new Vector3(movementSpeed, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
        }
	}
}
