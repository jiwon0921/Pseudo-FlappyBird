using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ColumnPool columnPool;
    public StageManager stageManager;
    public BirdController bird;

    public Vector2 originalBirdPosition;

    public int score;
    public int bestScore;

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
        UIManager.Instance.uiInGame.Toggle(false);
        UIManager.Instance.uiResult.Toggle(false);
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
        Debug.Log("Game Over");
        StopCoroutine(spawnAndMoveColumn);
        StopCoroutine(moveFloor);
        UIManager.Instance.uiResult.UpdateScore();
        IEnumerator uiResult = ResultUIControl();
        UIManager.Instance.uiInGame.flash.SetTrigger("Flash");
        StartCoroutine(uiResult);

    }

    public void ReStart()
    {
        initialize();
        GameReady();
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
}