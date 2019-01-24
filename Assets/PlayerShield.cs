using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour {
     
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyProjectile" || other.gameObject.tag == "Enemy")
        { 
            Destroy(other.gameObject);
        }
    }
}
