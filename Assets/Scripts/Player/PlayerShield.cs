using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour {
     
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyProjectile")
        { 
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            EnemyHelth enemyScript = other.gameObject.GetComponent<EnemyHelth>();
            enemyScript.TakeDamage(kill: true);
        }
    }
}
