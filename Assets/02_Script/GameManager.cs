using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PFB;
using PFB.Log;
using PFB.Database;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PFBUserData currentUserData;
    //public PlayerData playerData;

    public ColumnPool columnPool;
    public StageManager stageManager;
    public BirdController bird;

    public Vector2 originalBirdPosition;

    //public string currentPlayerName;

    public int score;
    public int bestScore => (int)currentUserData.bestScoreData.score;

    public bool isGameover;
    public bool isReady;

    IEnumerator spawnAndMoveColumn;
    IEnumerator moveFloor;

    public PFBLog PFBLog = new PFBLog("GameManager", Color.cyan);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PFBLogHelper.GetSafeCurrent();

        PFBLogHelper.SetSaveMode(ePFBLogSaveMode.OnExit);

        PFBLogHelper.SetCurrentLogLevel(ePFBLogLevel.All);

        PFBLog.LogDebug("�����");
        PFBLog.LogInfo("����");
        PFBLog.LogWarning("����");
        PFBLog.LogError("����");

        // PlayerPrsfs�� ������ ������ ����
        // PlayerPrefs.DeleteAll();

        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        columnPool.initilaze();
        GameReady();
        originalBirdPosition = bird.transform.position;

        PFBLog.LogInfo("Start Game");
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

        bird.Flappy();
    }

    public void GameOver()
    {
        isGameover = true;
        StartCoroutine(CoGameOver());
    }

    public void ReStart()
    {
        initialize();
        GameReady();
    }

    //public void CheckBestScore()
    //{
    //    int currentScore = score;
    //    int currentBestScore = bestScore;

    //    if (currentScore > bestScore)
    //    {
    //        bestScore = currentScore;
    //    }
    //    else
    //    {
    //        bestScore = currentBestScore;
    //    }

    //    PlayerData.Instance.SetBestScore();
    //}


    private IEnumerator CoGameOver()
    {
        PFBLog.LogDebug("Game Over");


        StopCoroutine(spawnAndMoveColumn);
        StopCoroutine(moveFloor);
        UIManager.Instance.uiInGame.flash.SetTrigger("Flash");

        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.uiDataLoading.Toggle(true);
        yield return null;
        yield return StartCoroutine(CoTryUpdateBestScore());

        //CheckBestScore();

        yield return StartCoroutine(CoTryUpdateRankingData());
        UIManager.Instance.uiDataLoading.Toggle(false);

        // PlayerData.Instance.UpdateRanking();

        UIManager.Instance.uiResult.UpdateScore();

        UIManager.Instance.uiResult.UpdateBestScore();

        //UIManager.Instance.uiRanking.LoadRankingData();

        IEnumerator coResultUIControl = ResultUIControl();


        StartCoroutine(coResultUIControl);
    }
    private IEnumerator CoTryUpdateBestScore()
    {
        //���� ������ �����ͻ��� ����Ʈ���ھ�� ���ų� ������ �ƹ��͵� ����
        if (score <= currentUserData.bestScoreData.score)
        {
            yield break;
        }

        //�ٵ� ���� ������ ������ ��ü �۾��� ������

        Task requestUpdateUserScore = PFBDBManager.Instance.request
            .UpdateUserBestScoreAsync(currentUserData,
            new PFBScoreData((uint)score, System.DateTime.Now));

        //��û �Ϸ���� ���
        yield return new WaitUntil(() => requestUpdateUserScore.IsCompleted);

        //bestScore = (int)currentUserData.bestScoreData.score;

        //yield return StartCoroutine(CoTryUpdateRankingData());
    }

    private IEnumerator CoTryUpdateRankingData()
    {
        UIManager.Instance.uiDataLoading.Toggle(true);
        yield return null;
        yield return StartCoroutine(UIManager.Instance.uiRanking.CoLoadRankingData());
        UIManager.Instance.uiDataLoading.Toggle(false);

        yield break;
    }

    public void OpenRanking(bool val)
    {
        if (val)
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
        return (int)currentUserData.bestScoreData.score;
    }

    public string GetPlayerName()
    {
        return currentUserData.userName;
    }
}