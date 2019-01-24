using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other)
    {  
        if (other.gameObject.tag == "EnemyProjectile")
                Destroy(other.gameObject); 

        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemyScript = other.gameObject.GetComponent<Enemy>();
            enemyScript.EnemyHelth--;

            if (enemyScript.EnemyHelth <= 0)
            {
                EnemyManager.Instance.RemoveEnemyFromList(enemyScript);
                 
                GameManager.Instance.AddPlayerScore(enemyScript.EnemyScoreValue);
                GameManager.Instance.CheckLevelComplete();

                Destroy(other.gameObject); 
            }
        }

        Destroy(this.gameObject);
    } 
}
