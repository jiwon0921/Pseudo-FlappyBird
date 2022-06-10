using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

using PFB;
using PFB.Database;
using System;
using PFB.Log;

namespace PFB.Database
{

    public class PFBDBManager : MonoBehaviour
    {
        public static PFBDBManager Instance;

        private DbAccessInfo currentAccess;
        public DbAccessInfo CurrentAccess => currentAccess;
        public PFBDBRequestHelper request { get; private set; } = null;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            Init();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Init()
        {
            currentAccess = new DbAccessInfo();
            currentAccess.client = CreateClient();
            currentAccess.database = currentAccess.client.GetDatabase(DbString.DB_PseudoFlappyBirdInGame);

            request = new PFBDBRequestHelper(currentAccess);
        }
        private void Start()
        {
            // Task.Run(() => ProcessDummy());
        }

        public async Task ProcessDummy()
        {
            PFBLog.LogInfo("함수 호출 시작");

            await CreateDummyUsers();


            //List<PFBUserData> userDataList = null;

            //await Task.Run(() => request.GetUserDataListForRankingAsync(10,
            //    (list) =>
            //    {
            //        userDataList = list;
            //    }));


            //foreach (var item in userDataList)
            //{
            //    PFBLog.Log($"{item.userName} : {item.bestScoreData.score}");
            //}
        }

        public async Task CreateDummyUsers()
        {

            PFBLog.LogInfo("Start Create Dummy Users");

            PFBUserData user1 = new PFBUserData("Honoka", 2);
            PFBUserData user2 = new PFBUserData("Kotori", 3);
            PFBUserData user3 = new PFBUserData("Umi", 4);
            PFBUserData user4 = new PFBUserData("Maki", 2);
            PFBUserData user5 = new PFBUserData("Hanayo", 2);
            PFBUserData user6 = new PFBUserData("Rin", 2);
            PFBUserData user7 = new PFBUserData("Nico", 2);
            PFBUserData user8 = new PFBUserData("Eli", 2);
            PFBUserData user9 = new PFBUserData("Nozomi", 3);

            var users = new PFBUserData[] { user1, user2, user3, user4, user5, user6, user7, user8, user9 };

            foreach (var item in users)
            {
                var isExist = await request.IsExistUserNameAsync(item.userName);
                if (!isExist)
                {
                    await request.CreateUserAsync(item, currentAccess);
                }
                else
                {
                    PFBLog.Log("Try Update " + item.userName);
                    await request.UpdateUserBestScoreAsync(item, new PFBScoreData(1, DateTime.Now));
                }

            }

            PFBLog.Log("End Test process");
        }

        //public async Task TestProcessAsync()
        //{


        //    await Task.Delay(GetMillisecond(1));

        //    PFBDBRequestHelper helper = new PFBDBRequestHelper(currentClient, "admin");

        //    var tempData = new Database.PFBUserData { userId = 25252, userName = "Jiwon" };
        //    await helper.UpdateData("UserDatas", "userId", tempData.userId, tempData, null);

        //    Debug.Log("END!");
        //    //ReplaceOneResult replaceOneResult = await helper.UpdateData("UserDatas", "userId", tempData.userId, tempData, null);
        //}
        //public IEnumerator CoInit()
        //{
        //    CreateClient();
        //    yield break;

        //}
        public MongoClient CreateClient()
        {
            // string cs = "mongodb://PFBUser:password@211.250.174.224:27017/";

            var cr = MongoCredential.CreateCredential("admin", DbString.PFBUser, DbString.password);

            MongoClientSettings clientSettings = new MongoClientSettings
            {
                Credential = cr,
                Server = new MongoServerAddress("172.30.1.59", 27017)
            };



            return new MongoClient(clientSettings);
        }


        public int GetMillisecond(int second) => second * 1000;


        public PFBLog PFBLog = new PFBLog("DBManager");

    }
}