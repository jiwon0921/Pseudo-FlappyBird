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
        /// �ش� userName�� ���� �����Ͱ� �ֳ� ã�ƺ��ϴ�.
        /// </summary>
        public async Task<bool> IsExistUserNameAsync(string userName)
        {
            PFBLog.LogDebug("���� �̸� ���� ���� �˻� : " + userName);
            var filter = Builders<PFBUserData>.Filter.Eq("userName", userName);
            var test = await GetUserDataCollection().Find(filter).AnyAsync();
            return test;
        }


        /// <summary>
        /// DB�� �ش� ���� �����͸� ����մϴ�.
        /// </summary>
        public async Task CreateUserAsync(PFBUserData data, DbAccessInfo accessInfo, UnityAction onEnd = null)
        {

            await GetUserDataCollection().InsertOneAsync(data);
            PFBLog.LogInfo($"���� ������ ���� : {data.userName}");
            onEnd?.Invoke();
        }

        public async Task GetUserDataByNameAsync(string userName, UnityAction<PFBUserData> onEnd)
        {
            var filter = Builders<PFBUserData>.Filter.Eq("userName", userName);
            var result = await GetUserDataCollection().Find(filter).FirstOrDefaultAsync();

            onEnd?.Invoke(result);
        }

        /// <summary>
        /// �⺻�����δ� <see href="GetUserDataByNameAsync"/>�� �Ȱ����ϴ�. ������, ������ üũ���� �ϰ� ������ ������شٴ� ���� �ٸ��ϴ�.
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
                PFBLog.LogDebug($"���ο� �����Դϴ� :{userName}");
                await CreateUserAsync(PFBUserData.CreateDefault(userName), access);

                result = await GetUserDataCollection().Find(filter).FirstOrDefaultAsync();
            }

            onEnd?.Invoke(result);
        }


        public async Task UpdateUserBestScoreAsync(PFBUserData user, PFBScoreData newBestScore)
        {
            PFBLog.LogDebug($"{user.userName}�� �ְ����� ���� �õ�...���� ����: {user.bestScoreData.score} | ���ŵ� ����: {newBestScore.score}");
            user.bestScoreData.Copy(newBestScore);

            var filterDef = Builders<PFBUserData>.Filter.Eq(DbString.userName, user.userName);
            var updateDef = Builders<PFBUserData>.Update.Set(x => x.bestScoreData, newBestScore);



            var result = await GetUserDataCollection().UpdateOneAsync(filterDef, updateDef);

            // PFBLog.Log($"{result.userName}:{result.bestScoreData.score}");
        }

        /// <summary>
        /// maxUserCount��ŭ, score�� ���ĵ� ���� �����͸� �����ɴϴ�.
        /// </summary>
        public async Task GetUserDataListForRankingAsync(int maxUserCount, UnityAction<List<PFBUserData>> onEnd)
        {
            var emptyFilter = Builders<PFBUserData>.Filter.Empty;
            var sortFilter = Builders<PFBUserData>.Sort.Descending(DbString.bestScoreDataScore);

            var result = await GetUserDataCollection()
                .Find(emptyFilter) //.Find(_=>true)
                .Sort(sortFilter)
                .Limit(maxUserCount).ToListAsync<PFBUserData>();


            PFBLog.LogDebug($"{maxUserCount}���� ��ŷ�� �����͸� �����Խ��ϴ�.");
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
