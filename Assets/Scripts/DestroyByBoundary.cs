using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemyScript = other.gameObject.GetComponent<Enemy>();
            EnemyManager.Instance.RemoveEnemyFromList(enemyScript);

            GameManager.Instance.CheckLevelComplete(); 
        }

        Destroy(other.gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }

    //downlod PaintNet
}
