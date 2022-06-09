using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace MongoDBFirst
{
    public class ProgramTester : MonoBehaviour
    {

        private void Start()
        {
            OnStart();
            Debug.Log("End....!!");
        }
        public void OnStart()
        {
            PersonModel person = new PersonModel
            {
                firstName = "Maki",
                lastName = "Nishikino",
                testModel = new TestModel
                {
                    testString = "Love YAZAWA"
                }
            };


            MongoCRUD db = new MongoCRUD("TesterTwoRERE");
            db.InsertRecord("Users", person);


            //var recs = db.LoadRecords<PersonModel>("Users");

            //foreach (var item in recs)
            //{
            //    Console.WriteLine($"{item.id}: {item.firstName}");

            //    if (item.testModel != null)
            //    {
            //        Console.WriteLine(item.testModel);
            //    }
            //    Console.WriteLine("--------------------");
            //}

           //  var oneRec = db.LoadRecordById<PersonModel>("Users", new Guid("383a60f3-952d-499b-9ba6-c593d3408771"));

            //oneRec.DateOfBirth = new DateTime(2013, 04, 19, 0, 0, 0, DateTimeKind.Utc);

            //db.UpsertRecord("Users", oneRec.id, oneRec);
            // db.DeleteRecord<PersonModel>("Users", oneRec.id);
            Console.ReadLine();
        }
    }

    public class PersonModel
    {
        [BsonId]
        public Guid id;
        public string firstName;
        public string lastName;

        [BsonElement("dob")]
        public DateTime DateOfBirth;

        public TestModel testModel;
    }


    public class TestModel
    {
        public string testString;
    }
}
