using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine.Events;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using PFB.Log;

namespace PFB.Database
{
    public class PFBDBRequestHelper
    {
        private DbAccessInfo access;
        private PFBLog PFBLog = new PFBLog("DBRequest", Color.magenta);

        public PFBDBRequestHelper(DbAccessInfo accessInfo)
        {
            access = accessInfo;
        }

        public PFBDBRequestHelper(MongoClient c, string databaseName)
        {
            access = new DbAccessInfo
            {
                client = c,
                database = c.GetDatabase(databaseName)
            };
        }


        public IMongoCollection<PFBUserData> GetUserDataCollection()
        {
            return access.database.GetCollection<PFBUserData>(DbString.Col_UserDatas);
        }

        /// <summary>
        /// 해당 userName을 가진 데이터가 있나 찾아봅니다.
        /// </summary>
        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            PFBLog.LogDebug("유저 이름 존재 여부 검사 : " + userName);
            var filter = Builders<PFBUserData>.Filter.Eq("userName", userName);
            var test = await GetUserDataCollection().Find(filter).AnyAsync();
            return test;
        }


        /// <summary>
        /// DB에 해당 유저 데이터를 등록합니다.
        /// </summary>
        public async Task CreateUserAsync(PFBUserData data, DbAccessInfo accessInfo, UnityAction onEnd = null)
        {

            await GetUserDataCollection().InsertOneAsync(data);
            PFBLog.LogInfo($"유저 데이터 생성 : {data.userName}");
            onEnd?.Invoke();
        }

        public async Task GetUserDataByNameAsync(string userName, UnityAction<PFBUserData> onEnd)
        {
            var filter = Builders<PFBUserData>.Filter.Eq("userName", userName);
            var result = await GetUserDataCollection().Find(filter).FirstOrDefaultAsync();

            onEnd?.Invoke(result);
        }

        /// <summary>
        /// 기본적으로는 <see href="GetUserDataByNameAsync"/>과 똑같습니다. 하지만, 없는지 체크까지 하고 없으면 만들어준다는 점이 다릅니다.
        /// </summary>
        public async Task GetUserDataByNameAsyncSafe(string userName, UnityAction<PFBUserData> onEnd)
        {
            PFBUserData result = null;
            var filter = Builders<PFBUserData>.Filter.Eq(DbString.userName, userName);

            bool isExist = await IsExistUserNameAsync(userName);
            if (isExist)
            {
                result = await GetUserDataCollection().Find(filter).FirstOrDefaultAsync();

            }
            else
            {
                PFBLog.LogDebug($"새로운 유저입니다 :{userName}");
                await CreateUserAsync(PFBUserData.CreateDefault(userName), access);

                result = await GetUserDataCollection().Find(filter).FirstOrDefaultAsync();
            }

            onEnd?.Invoke(result);
        }


        public async Task UpdateUserBestScoreAsync(PFBUserData user, PFBScoreData newBestScore)
        {
            PFBLog.LogDebug($"{user.userName}의 최고점수 갱신 시도...기존 점수: {user.bestScoreData.score} | 갱신될 점수: {newBestScore.score}");
            user.bestScoreData.Copy(newBestScore);

            var filterDef = Builders<PFBUserData>.Filter.Eq(DbString.userName, user.userName);
            var updateDef = Builders<PFBUserData>.Update.Set(x => x.bestScoreData, newBestScore);



            var result = await GetUserDataCollection().UpdateOneAsync(filterDef, updateDef);

            // PFBLog.Log($"{result.userName}:{result.bestScoreData.score}");
        }

        /// <summary>
        /// maxUserCount만큼, score로 정렬된 유저 데이터를 가져옵니다.
        /// </summary>
        public async Task GetUserDataListForRankingAsync(int maxUserCount, UnityAction<List<PFBUserData>> onEnd)
        {
            var emptyFilter = Builders<PFBUserData>.Filter.Empty;
            var sortFilter = Builders<PFBUserData>.Sort.Descending(DbString.bestScoreDataScore);

            var result = await GetUserDataCollection()
                .Find(emptyFilter) //.Find(_=>true)
                .Sort(sortFilter)
                .Limit(maxUserCount).ToListAsync<PFBUserData>();


            PFBLog.LogDebug($"{maxUserCount}개의 랭킹용 데이터를 가져왔습니다.");
            onEnd?.Invoke(result);
        }


        //public IMongoCollection<T> GetCollection<T>(string collectionName)
        //{
        //    return access.database.GetCollection<T>(collectionName);
        //}

        //public async Task<ReplaceOneResult> UpdateDataAsync<T>(string collectionName, string keyName, string key, T data, UnityAction onRequestEnd)
        //{
        //    var collection = GetCollection<T>(collectionName);
        //    var filter = Builders<T>.Filter.Eq(keyName, key);
        //    var request = await collection.ReplaceOneAsync(filter, data, new ReplaceOptions { IsUpsert = true });

        //    await Task.Delay(3000);
        //    Debug.Log("Hi!");
        //    onRequestEnd?.Invoke();
        //    return request;
        //}


        //public async Task<ReplaceOneResult> UpdateDataAsync<T>(string collectionName, string userId, T data, UnityAction onRequestEnd)
        //{
        //    var collection = GetCollection<T>(collectionName);
        //    FilterDefinition<T> filter = Builders<T>.Filter.Eq(DbString.userId, userId);

        //    var request = await collection.ReplaceOneAsync(filter, data, new ReplaceOptions { IsUpsert = true });
        //    Debug.Log("Hi!");
        //    onRequestEnd?.Invoke();
        //    return request;
        //}

        //public Task<ReplaceOneResult> UpdateData<T>(string collectionName, string keyName, string key, T data, UnityAction onRequestEnd)
        //{
        //    var collection = GetCollection<T>(collectionName);
        //    var filter = Builders<T>.Filter.Eq(keyName, key);
        //    var request = collection.ReplaceOneAsync(filter, data, new ReplaceOptions { IsUpsert = true });

        //    onRequestEnd?.Invoke();
        //    Debug.Log("UpdateData End!");
        //    return request;
        //}

        //private async void UpdatedataAsync<T>(string collectionName, int userId)
        //{
        //    var collection = currentDatabase.GetCollection<T>(collectionName);

        //    await Task.Run(() => collection.ReplaceOneAsync<T>(

        //                       new BsonDocument("_userId", new BsonBinaryData(userId))),
        //        ));
        //}

    }

}
