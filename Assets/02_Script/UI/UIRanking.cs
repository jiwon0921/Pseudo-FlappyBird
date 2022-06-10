using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PFB.Database;
using System.Threading.Tasks;

public class UIRanking : UIView
{

    #region 클래스들...
    [System.Serializable]
    public class UIRankingUserData
    {
        public string userName;
        public string bestScore;
        public string recordDate;
        public void SetDataByFBUserData(PFBUserData userData)
        {
            userName = userData.userName;
            bestScore = userData.bestScoreData.score.ToString();
            recordDate = DbString.DateTimeToString(userData.bestScoreData.recordedDate);
        }
    }

    [System.Serializable]
    public class UIRankingContent
    {
        public GameObject image_contentBG;
        public TextMeshProUGUI text_name;
        public TextMeshProUGUI text_bestscore;
        public TextMeshProUGUI text_recordDate;
        public TextMeshProUGUI text_rankingNum;

        public void UpdateContent(UIRankingUserData data, int rankingNum, bool isPlayer)
        {
            text_name.text = data.userName;
            text_bestscore.text = data.bestScore;
            text_recordDate.text = data.recordDate;
            text_rankingNum.text = rankingNum.ToString() + ".";

            if (isPlayer)
            {
                image_contentBG.SetActive(true);
            }
            else
            {
                image_contentBG.SetActive(false);
            }
        }
    }
    #endregion


    public TextMeshProUGUI firstName;
    public TextMeshProUGUI firstScore;

    public TextMeshProUGUI secondName;
    public TextMeshProUGUI secondScore;

    public TextMeshProUGUI thirdName;
    public TextMeshProUGUI thirdScore;

    [SerializeField]
    private List<GameObject> rankingContentRootObjectList = null;

    [SerializeField]
    private List<UIRankingContent> rankingContentList = null;


    public void Init()
    {
        rankingContentList = new List<UIRankingContent>();

        foreach (var item in rankingContentRootObjectList)
        {
            UIRankingContent content = new UIRankingContent();

            content.image_contentBG = item.transform.GetChild(0).gameObject;

            Transform textGroup = item.transform.GetChild(1);
            content.text_name = textGroup.GetChild(0).GetComponent<TextMeshProUGUI>();
            content.text_bestscore = textGroup.GetChild(1).GetComponent<TextMeshProUGUI>();
            content.text_recordDate = textGroup.GetChild(2).GetComponent<TextMeshProUGUI>();
            content.text_rankingNum = textGroup.GetChild(3).GetComponent<TextMeshProUGUI>();

            rankingContentList.Add(content);
        }
    }

    public IEnumerator CoLoadRankingData()
    {
        List<PFBUserData> userList = null;

        Task request = PFBDBManager.Instance.request.GetUserDataListForRankingAsync(5,
            (list) =>
            {
                userList = list;
            });

        Task.Run(() => request);

        yield return new WaitUntil(() => request.IsCompleted);


        if (userList == null)
        {
            PFBLog.LogError("userList is Null!");
            yield break;
        }
        bool isPlayer = false;
        for (int i = 0; i < userList.Count; i++)
        {
            isPlayer = userList[i].userName == GameManager.Instance.currentUserData.userName;

            UIRankingUserData curData = new UIRankingUserData();
            curData.SetDataByFBUserData(userList[i]);

            rankingContentList[i].UpdateContent(curData, i + 1, isPlayer);
        }
    }
    public void LoadRankingData()
    {
        firstScore.text = PlayerPrefs.GetInt("FirstScore").ToString();
        secondScore.text = PlayerPrefs.GetInt("SecondScore").ToString();
        thirdScore.text = PlayerPrefs.GetInt("ThirdScore").ToString();

        firstName.text = PlayerPrefs.GetString("FirstName");
        secondName.text = PlayerPrefs.GetString("SecondName");
        thirdName.text = PlayerPrefs.GetString("ThirdName");
    }


    private PFB.Log.PFBLog PFBLog = new PFB.Log.PFBLog("UIRanking", default, false);
}
