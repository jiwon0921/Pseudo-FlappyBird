using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData playerData;

    public ColumnPool columnPool;
    public StageManager stageManager;
    public BirdController bird;

    public Vector2 originalBirdPosition;

    public string playerName;

    public int score;
    public int bestScore = 0;

    public bool isGameover;
    public bool isReady;

    IEnumerator spawnAndMoveColumn;
    IEnumerator moveFloor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        // PlayerPrsfs에 저장한 데이터 삭제
        PlayerPrefs.DeleteAll();

        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        columnPool.initilaze();
        GameReady();
        originalBirdPosition = bird.transform.position;
    }

    public void initialize()
    {
        score = 0;
        isGameover = true;
        isReady = true;
        bird.rigidBody.MovePosition(originalBirdPosition);
        bird.rigidBody.velocity = Vector2.zero;
        bird.transform.rotation = Quaternion.Euler(Vector3.zero);
        columnPool.DespawnAll();
        UIManager.Instance.uiInGame.UpdateScore();
    }

    public void GameReady()
    {
        isReady = true;
        score = 0;
        UIManager.Instance.uiReady.Toggle(true);
        UIManager.Instance.uiReady.OffStartButton();
        UIManager.Instance.uiReady.namePanel.SetActive(true);
        UIManager.Instance.uiInGame.Toggle(false);
        UIManager.Instance.uiResult.Toggle(false);
        UIManager.Instance.uiRanking.Toggle(false);
        moveFloor = stageManager.MoveFloor();
        StartCoroutine(moveFloor);
    }

    public void GameStart()
    {
        isReady = false;
        isGameover = false;
        stageManager.initialize();
        UIManager.Instance.uiInGame.Toggle(true);
        UIManager.Instance.uiReady.Toggle(false);
        spawnAndMoveColumn = stageManager.SpawnAndMoveColumn();
        StartCoroutine(spawnAndMoveColumn);
    }

    public void GameOver()
    {
        isGameover = true;
        CheckBestScore();
        Debug.Log("Game Over");
        StopCoroutine(spawnAndMoveColumn);
        StopCoroutine(moveFloor);
        PlayerData.Instance.UpdateRanking();
        UIManager.Instance.uiResult.UpdateScore();
        UIManager.Instance.uiResult.UpdateBestScore();
        UIManager.Instance.uiRanking.LoadRankingData();
        IEnumerator uiResult = ResultUIControl();
        UIManager.Instance.uiInGame.flash.SetTrigger("Flash");
        StartCoroutine(uiResult);

    }

    public void ReStart()
    {
        initialize();
        GameReady();
    }

    public void CheckBestScore()
    {
        int currentScore = score;
        int currentBestScore = bestScore;

        if(currentScore > bestScore)
        {
            bestScore = currentScore;
        }
        else
        {
            bestScore = currentBestScore;
        }

        PlayerData.Instance.SetBestScore();
    }

    public void OpenRanking(bool val)
    {
        if(val)
        {
            UIManager.Instance.uiRanking.Toggle(true);
        }
        else
        {
            UIManager.Instance.uiRanking.Toggle(false);
        }
    }

    IEnumerator ResultUIControl()
    {
        yield return new WaitForSeconds(1f);

        UIManager.Instance.uiInGame.Toggle(false);
        UIManager.Instance.uiResult.Toggle(true);
    }

    public int GetScore()
    {
        return score;
    }

    public int GetBestScore()
    {
        return bestScore;
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}