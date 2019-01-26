using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject.tag == "EnemyProjectile")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            EnemyHelth enemyScript = other.gameObject.GetComponent<EnemyHelth>();
            enemyScript.TakeDamage(true);
        }
    } 
}
