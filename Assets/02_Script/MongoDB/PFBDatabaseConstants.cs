using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFB.Database
{
    [System.Serializable]
    public class PFBUserData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId;

        public string userName;

        public PFBScoreData bestScoreData = new PFBScoreData();

        public PFBUserData() { }

        public PFBUserData(string name, uint bestScore)
        {
            userName = name;
            bestScoreData = new PFBScoreData(bestScore, DateTime.Now);
        }

        public static PFBUserData CreateDefault(string userName)
        {
            return new PFBUserData(userName, 0);
        }
    }

    [System.Serializable]
    public class PFBScoreData
    {
        public uint score;
        public DateTime recordedDate;

        public PFBScoreData() { }

        public PFBScoreData(uint newScore, DateTime newRecordedDate)
        {
            score = newScore;
            recordedDate = newRecordedDate;

        }
        public PFBScoreData(PFBScoreData data)
        {
            score = data.score;
            recordedDate = data.recordedDate;

        }

        public void Copy(PFBScoreData data)
        {
            score = data.score;
            recordedDate = data.recordedDate;
        }

        public void Set(uint newScore, DateTime dateTime)
        {
            score = newScore;
            recordedDate = dateTime;
        }
    }

    [System.Serializable]
    public class DbAccessInfo
    {
        public MongoClient client;
        public IMongoDatabase database;
    }

    public class DbString
    {
        public static string Col_UserDatas => "UserDatas";

        public static string DB_PseudoFlappyBirdInGame => "PseudoFlappyBirdInGame";
        public static string userId => "userId";
        public static string userIdKey => "_id";
        public static string userName => "userName";

        public static string bestScoreData => "bestScoreData";
        public static string bestScoreDataScore => "bestScoreData.score";

        public static string PFBUser => "PFBUser";

        public static string password => "password";

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd-HH:mm");
        }
    }
}

