using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Movimiento rudimentario de la cámara
public class CameraMovement : MonoBehaviour {

    public float speed = 1f; //Lo cambias según el tamaño que quieras ver
    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z + speed);
        }
    }
}


