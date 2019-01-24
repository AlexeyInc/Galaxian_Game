using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{  
    [SerializeField]
    private int enemyHelth;
    [SerializeField] 
    private int enemyScoreValue;

    public GameObject projectile;
      
    [Header("Base_Move")]
    public float stepLength; 
    public int countFirstStep;
    public int countSteps; 
    public float timeBetweenStep;

    [Header("ForwardToPlayer_Move")]
    public float speedMove = 4.3f;
    public float speedForce = 10f;
    public float Radius = 4f; 
    public float _angle = 45; 
    public float timeManuver = 3.5f;

    private Coroutine baseMoveCoroutine;
     
    public int EnemyScoreValue
    {
        get { return enemyScoreValue; }
    }

    public int EnemyHelth {
        get { return enemyHelth; }
        set { enemyHelth = value; }
    }

    void Start()
    {
        baseMoveCoroutine = StartCoroutine(BaseMove()); 
    } 

    public void MoveToPlayerAndAttack()
    { 
        StopCoroutine(baseMoveCoroutine);
         
        StartCoroutine(ForwardMove());

        InvokeRepeating("Fire", 0, 1.5f);
    } 
     
    IEnumerator BaseMove()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < countFirstStep; i++)
        {
            transform.localPosition += new Vector3(stepLength, 0, 0);
            yield return new WaitForSeconds(timeBetweenStep);
        }

        while (true)
        {
            for (int i = 0; i < countSteps; i++)
            {
                transform.localPosition -= new Vector3(stepLength, 0, 0);
                yield return new WaitForSeconds(timeBetweenStep);
            }

            for (int i = 0; i < countSteps; i++)
            {
                transform.localPosition += new Vector3(stepLength, 0, 0);
                yield return new WaitForSeconds(timeBetweenStep);
            }
        }
    }

    IEnumerator ForwardMove()
    {
        float distance1 = Vector3.Distance(this.transform.position, EnemyManager.Instance.GetTarget(0));
        float distance2 = Vector3.Distance(this.transform.position, EnemyManager.Instance.GetTarget(1));

        Vector3 targetPoint = distance1 > distance2 ? EnemyManager.Instance.GetTarget(0) : EnemyManager.Instance.GetTarget(1);
        Vector2 centre = this.transform.position;
        bool circleMoveDirection = distance1 > distance2;

        timeManuver = Time.time + timeManuver;

        while (Time.time <= timeManuver)
        {
            _angle += Time.deltaTime;

            var offset = GetRotateDirectionVector(clockwise: circleMoveDirection) * Radius;

            transform.position = Vector3.MoveTowards(this.transform.position, centre + offset, speedMove * Time.deltaTime);

            yield return new WaitForSeconds(0.05f);
        }

        Vector3 direction = targetPoint - transform.position;  
        GetComponent<Rigidbody2D>().AddForce(direction * speedForce);
    }

    private Vector2 GetRotateDirectionVector(bool clockwise)
    {
        if (clockwise)
            return new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle));
        else
            return new Vector2(Mathf.Cos(_angle-5), Mathf.Sin(_angle-5));
    }
     
    public void Fire()
    {
        Instantiate(projectile, this.transform.position, Quaternion.identity);
    } 
}
