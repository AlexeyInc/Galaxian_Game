using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other)
    {  
        if (other.gameObject.tag == "EnemyProjectile")
                Destroy(other.gameObject); 
        else
        if (other.gameObject.tag == "Enemy")
        {  
            Enemy enemyScript = other.gameObject.GetComponent<Enemy>();
            enemyScript.TakeDamage(); 
        }  

        Destroy(this.gameObject);
    } 
}
