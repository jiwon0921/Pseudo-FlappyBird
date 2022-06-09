using MongoDB.Bson;

using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MongoDBFirst
{
    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            db = client.GetDatabase(database);
        }


        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }


        public T LoadRecordById<T>(string table, Guid id)

        {
            var col = db.GetCollection<T>(table);
            var fil = Builders<T>.Filter.Eq("id", id);
            return col.Find(fil).First();
        }


        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var col = db.GetCollection<T>(table);
            var result = col.ReplaceOne(
                new BsonDocument("_id",new BsonBinaryData(id, GuidRepresentation.Standard)),
                record,
                new ReplaceOptions { IsUpsert = true}
                //new UpdateOptions { IsUpsert = true}
                );
        }


        public void DeleteRecord<T>(string table, Guid id)
        {
            var col = db.GetCollection<T>(table);
            var fil = Builders<T>.Filter.Eq("id", id);
            col.DeleteOne(fil);
        }
    }
}
