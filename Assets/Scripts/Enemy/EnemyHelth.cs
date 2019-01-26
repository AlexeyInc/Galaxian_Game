using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHelth : MonoBehaviour {
      
    [SerializeField]
    private int scoreValue;
    [SerializeField] 
    private int helth;

    private void Start()
    {
        helth = GameManager.Instance.CurrentLevel + helth;
    }

    public void TakeDamage(bool kill = false)
    {
        helth--;

        if (helth <= 0 || kill)
        {
            EnemyManager.Instance.KillEnemy(this.gameObject.GetComponent<EnemyBehavior>());

            GameManager.Instance.AddPlayerScore(scoreValue);
            GameManager.Instance.CheckLevelComplete();
        }
    }
}
