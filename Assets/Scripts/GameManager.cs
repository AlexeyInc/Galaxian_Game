using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
[System.Serializable]
public struct ObjectSpawn
{
    public float xMin, xMax, y; 
}

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
      
    public Text playerScoreText;
    public Text playerHelthText;
    public Text gameLevelText;
     
    public GameObject[] bonuses; 
    public ObjectSpawn objectSpawn;

    public int countLevels;
     
    EnemyGridCreator enemyGrid;

    private int _numOfLevel = 1;
    private int _playerScore = 0;
    private Queue<int> _nextScoreToBonus;
    private int _scoreStepToBonus = 70;
    private int _countBonuses = 5;
     
    private bool _isGameOver;

    public bool IsGameOver
    {
        get { return _isGameOver; }
    }

    private void Awake()
    {
        if (_instance == null)
        { 
            _instance = this;
        }
    }

    private void Start()
    { 
        enemyGrid = new EnemyGridCreator(5, 13);

        _nextScoreToBonus = new Queue<int>(_countBonuses); 
        InitScoreBonusPerSteps();

        StartCoroutine(InitLevel(_numOfLevel)); 
    }

    void InitScoreBonusPerSteps()
    {
        for (int i = _scoreStepToBonus; i <= _countBonuses * _scoreStepToBonus; i += _scoreStepToBonus)
            _nextScoreToBonus.Enqueue(i);
    }
     
    private IEnumerator InitLevel(int numOfLevel)
    {
        gameLevelText.text = "Level" + numOfLevel.ToString();
        gameLevelText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        enemyGrid.InitLevel(numOfLevel); 

        EnemyManager.Instance.CreateEnemies(enemyGrid);
        EnemyManager.Instance.UpdateEnemyLogic(numOfLevel);

        gameLevelText.gameObject.SetActive(false); 
    }
     
    private void CheckBonus()
    {
        if (_nextScoreToBonus.Count > 0 && _playerScore > _nextScoreToBonus.Peek())
        {
            _nextScoreToBonus.Dequeue();

            GameObject bonus = bonuses[0];//Random.Range(0, bonuses.Length)
            Vector2 spawnPosition = new Vector2(Random.Range(objectSpawn.xMin, objectSpawn.xMax), objectSpawn.y);
            Instantiate(bonus, spawnPosition, Quaternion.identity);
        }
    }

    public void CheckLevelComplete()
    {
        int countEnemies = EnemyManager.Instance.GetEnemyCount();

        if (countEnemies == 0) 
            Invoke("StartNextLevel", 2f); 
    }

    private void StartNextLevel()
    {
        if (countLevels == _numOfLevel)
        {
            Debug.Log("Player win!!");
        }
        else
        { 
            _numOfLevel++; 
            StartCoroutine(InitLevel(_numOfLevel));
        }
    }
     
    public void AddPlayerScore(int countScore)
    {
        _playerScore += countScore;
        playerScoreText.text = "Score: " + _playerScore;

        CheckBonus();
    }

    public void SetPlayerHelth(int helth)
    {
        playerHelthText.text = "Helth: " + helth;

        if (helth <= 0)
        {
            _isGameOver = true;
            Debug.Log("Game is over");
        }
    }
}
