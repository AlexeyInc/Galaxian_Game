using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using EnemyGrid; 

public class EnemyManager : MonoBehaviour {

    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get { return _instance; }
    }
     
    public GameObject SimpleEnemy;
    public GameObject EnemyBoss; 
    public GameObject enemyExplosion;

    public Transform enemyHolder;
     
    public Transform[] targetPoints;

    EnemyGridCreator enemyGridCreator;

    List<Enemy> enemiesList = new List<Enemy>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }  
    
    public void InitEnemyGrid(int height, int width)
    {
        enemyGridCreator = new EnemyGridCreator(height, width);

        enemyGridCreator.InitLevel(GameManager.Instance.CurLevel);

        CreateEnemies();
    }

    void CreateEnemies()
    {
        for (int r = 0; r < enemyGridCreator.GridRows; r++)
        {
            for (int c = 0; c < enemyGridCreator.GridColumns; c++)
            {
                if (enemyGridCreator[r, c].EnemyType != EnemyType.None)
                {
                    GameObject newEnemy = CreateEnemyByType(enemyGridCreator[r, c].Row, enemyGridCreator[r, c].Col, enemyGridCreator[r, c].EnemyType); 
                    newEnemy.transform.SetParent(enemyHolder);

                    AddEnemyToList(newEnemy);
                }
            }
        }
    }

    GameObject CreateEnemyByType(float row, float col, EnemyType enemyType)
    {
        GameObject enemy = null; 
        switch (enemyType)
        {
            case EnemyType.Boss:
                enemy = Instantiate(EnemyBoss, new Vector2(col, row), Quaternion.identity);
                break;
            case EnemyType.Standard:
                enemy = Instantiate(SimpleEnemy, new Vector2(col, row), Quaternion.identity);
                break;
            default:
                Debug.Log("Error type enemy.");
                break;
        }
        return enemy;
    }

    void AddEnemyToList(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemiesList.Add(enemyScript);
    }

    public void UpdateEnemyLogic()
    { 
        StartCoroutine(RandomEnemyFire());

        if(GameManager.Instance.CurLevel == 2)
            StartCoroutine(RandomEnemyPlayerAttack());
    }

    IEnumerator RandomEnemyFire()
    {
        while (enemiesList.Count > 0 && !GameManager.Instance.IsGameOver)
        {  
            int randIndex = Random.Range(0, enemiesList.Count);

            if (enemiesList[randIndex] != null) 
                enemiesList[randIndex].Fire(); 

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator RandomEnemyPlayerAttack()
    {
        yield return new WaitForSeconds(1.5f); 

        while (enemiesList.Count > 0 && !GameManager.Instance.IsGameOver)
        {
            int randIndex = Random.Range(0, enemiesList.Count);

            if (enemiesList[randIndex] != null)
                enemiesList[randIndex].MoveToPlayerAndAttack();

            yield return new WaitForSeconds(6f);
        }
    } 

    public Vector3 GetTarget(int index)
    {
        return targetPoints[index].position;
    }

    public void KillEnemy(Enemy enemy)
    {
        enemiesList.Remove(enemy);

        Destroy(enemy.gameObject);

        Instantiate(enemyExplosion, enemy.gameObject.transform.position, Quaternion.identity);
    }

    public int GetEnemyCount()
    {
        if (enemiesList.Count == 0)
        {
            StopAllCoroutines();
        }

        return enemiesList.Count;
    }
}
