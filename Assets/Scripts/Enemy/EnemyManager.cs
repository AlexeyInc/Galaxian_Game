using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get { return _instance; }
    }
     
    public GameObject redBoss;
    public GameObject greenEnemy;

    public Transform enemyHolder;
     
    public Transform[] targetPoints;
     
    List<Enemy> enemiesList = new List<Enemy>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }  
     
    public void CreateEnemies(EnemyGridCreator enemyGrid)
    {
        for (int row = 0; row < enemyGrid.GridRows; row++)
        {
            for (int col = 0; col < enemyGrid.GridColumns; col++)
            {
                if (enemyGrid[row, col] != EnemyType.None)
                {
                    GameObject newEnemy = CreateEnemyByType(enemyGrid[row, col], row, col); 
                    newEnemy.transform.SetParent(enemyHolder);

                    AddEnemyToList(newEnemy);
                }
            }
        }
    }

    GameObject CreateEnemyByType(EnemyType enemyType, int row, int col)
    {
        GameObject enemy = null; 
        switch (enemyType)
        {
            case EnemyType.Boss:
                enemy = Instantiate(redBoss, new Vector2(col, row), Quaternion.identity);
                break;
            case EnemyType.Standard:
                enemy = Instantiate(greenEnemy, new Vector2(col, row), Quaternion.identity);
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

    public void UpdateEnemyLogic(int numLevel)
    { 
        StartCoroutine(RandomEnemyFire());

        if(numLevel == 2)
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
        yield return new WaitForSeconds(0.5f); 

        while (enemiesList.Count > 0 && !GameManager.Instance.IsGameOver)
        {
            int randIndex = Random.Range(0, enemiesList.Count);

            if (enemiesList[randIndex] != null)
                enemiesList[randIndex].MoveToPlayerAndAttack();

            yield return new WaitForSeconds(5f);
        }
    } 

    public Vector3 GetTarget(int index)
    {
        return targetPoints[index].position;
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        enemiesList.Remove(enemy);
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
