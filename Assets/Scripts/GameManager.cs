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

    [Header("UI")]
    public Text playerScoreText;
    public Text playerHelthText;
    public Text gameLevelText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Bonuses")]
    public GameObject[] bonuses;
    public ObjectSpawn objectSpawn;

    [Header("Other")]
    public int EnemyGridHeight;
    public int EnemyGridWidth;
     
    public int countLevels;

    private int _playerScore = 0;
    private Queue<int> _nextScoreToBonus;
    private int _scoreStepToBonus = 50;
    private int _countBonuses = 5;

    private bool _isGameOver;

    public bool IsGameOver
    {
        get { return _isGameOver; }
    }

    private int _numOfLevel = 1;

    public int CurLevel
    {
        get { return _numOfLevel; }
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
        _nextScoreToBonus = new Queue<int>(_countBonuses);
        InitScoreBonusPerSteps();

        StartCoroutine(InitNewLevel());
    }

    void InitScoreBonusPerSteps()
    {
        for (int i = _scoreStepToBonus; i <= _countBonuses * _scoreStepToBonus; i += _scoreStepToBonus)
            _nextScoreToBonus.Enqueue(i);
    }

    private IEnumerator InitNewLevel()
    {
        gameLevelText.text = "Level " + _numOfLevel.ToString();
        gameLevelText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        EnemyManager.Instance.InitEnemyGrid(EnemyGridHeight, EnemyGridWidth);
        EnemyManager.Instance.UpdateEnemyLogic();

        gameLevelText.gameObject.SetActive(false);
    }

    private void CheckBonus()
    {
        if (_nextScoreToBonus.Count > 0 && _playerScore > _nextScoreToBonus.Peek())
        {
            _nextScoreToBonus.Dequeue();

            GameObject bonus = bonuses[Random.Range(0, bonuses.Length)];
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
            EnemyManager.Instance.CreateBossEnemy();  
        }
        else
        { 
            _numOfLevel++;
            StartCoroutine(InitNewLevel());
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
        playerHelthText.text = helth.ToString();

        if (helth <= 0)
        {
            _isGameOver = true;
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator KillPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Suicide();

        yield return new WaitForSeconds(3f);
        gameOverPanel.SetActive(true);
    } 

    public void PlayerWin() 
    {
        Invoke("ActiveWinPanel", 2f);
    }

    private void ActiveWinPanel()
    { 
        winPanel.SetActive(true);
    }
}
