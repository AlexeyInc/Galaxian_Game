using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    public float moveSpeed;
    public float attackMoveSpeed;
    public int helth;
    public GameObject projectile;
    public Transform[] projectileSpawns;
    public GameObject explosion;
     
    public Vector2 retreatPos;

    private bool attackMode;
     
    void Start ()
    {
        StartCoroutine(MoveAttack());
        StartCoroutine(WeaponFire());
    }

    IEnumerator WeaponFire()
    {
        while (helth > 0)
        {
            yield return new WaitForSeconds(1f);

            if (!attackMode)
            { 
                for (int i = 0; i < projectileSpawns.Length; i++)
                {
                    Instantiate(projectile, projectileSpawns[i].position, Quaternion.identity);
                }
            }
        }
    }

    IEnumerator MoveAttack()
    {
        while (helth > 0)
        {
            attackMode = true;

            Vector3 playerTarget = GameObject.FindGameObjectWithTag("Player").transform.position;

            while (this.transform.position != playerTarget)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, playerTarget, attackMoveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.05f);
            }

            Vector3 retreatPosTarget = new Vector3(Random.Range(retreatPos.x, retreatPos.x * 3), 
                                                   Random.Range(retreatPos.y / 2, retreatPos.y));

            yield return new WaitForSeconds(0.5f);

            attackMode = false;

            while (this.transform.position != retreatPosTarget)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, retreatPosTarget, moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlayerProjectile") 
            TakeDamage(); 
    }

    private void TakeDamage()
    {
        helth--;

        if (helth <= 0)
        {
            GameManager.Instance.PlayerWin();

            Destroy(this.gameObject);

            Instantiate(explosion, this.transform.position, Quaternion.identity); 
        }
    }
}
