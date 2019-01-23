using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float directionSpeed;

	void Start () {
        GetComponent<Rigidbody2D>().velocity = transform.up * directionSpeed;
    } 
}
