using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Entrants
{
    public enum Country
    {
        Antegria,
        Arstotzka,
        Impor,
        Kolechia,
        Obristan,
        Republia,
        UnitedFederation,
        Cobrastan,
    }
    
    public class EntrantManager
    {
        private static Random random;
        public enum Sex {Male, Female};

        public static double incorrectRate = 0.45;

        protected static int EntrantCount;
        
        [System.Serializable]
        public class EntrantData
        {
            [SerializeField] internal int index;
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal string dateOfBirth;
            [SerializeField] internal string issuingCity;
            [SerializeField] internal string iD;
            [SerializeField] internal Sex sex;
            
            internal bool isCorrect = true;
            private List<Document> documentsList;
            
            [SerializeField] internal PassportData passport;
            [SerializeField] internal WorkPassData workPass;

            public EntrantData(string surName, string firstName, string iD, Sex sex, string dateOfBirth, string issuingCity)
            {
                if (random == null)
                {
                    random = new Random();
                    EntrantCount = 0;
                }
                this.index = EntrantCount;
                EntrantCount++;
                
                this.surName = surName;
                this.firstName = firstName;
                this.dateOfBirth = dateOfBirth;
                this.issuingCity = issuingCity;
                this.iD = iD;
                this.sex = sex;
                
                passport =  new PassportData(this, DateTime.Today.ToString(new CultureInfo("ja-JP")));
                workPass = new WorkPassData(this, DateTime.Today.ToString(new CultureInfo("ja-JP")));
                
            }

            public (string, string, string, string, string, string) GetPassportData()
            {
                PassportData ps = this.passport;
                string sexString = ps.sex.ToString();
                //Return Name, EXP, ISS, SEX, DOB, ID 
                return (ps.firstName + "," + ps.surName, ps.expirationDate, ps.issuingCity,  ps.sex.ToString()[0].ToString(),
                    ps.dateOfBirth, ps.iD);
            }
            
            public string ToJSon()
            {
                string json = JsonUtility.ToJson(this);
                //Debug.Log(json);
                return json;
            }
            
            
            private void Errorate()
            {
                bool goingToCreate  = incorrectRate<random.NextDouble();
                if(goingToCreate) GenerateError();
            }

            private void GenerateError()
            {
                isCorrect = false;
                Document documentToErrorate = documentsList[random.Next(documentsList.Count)];
                switch (documentToErrorate)
                {
                    case PassportData passport:
                        
                        return;
                    
                    case WorkPassData workPass:
                        
                        return;
                    
                    case Speech speech:
                        
                        return;
                }
                
            }
            
        }

        public abstract class Document{}
        
        [System.Serializable]
        public class PassportData : Document
        {
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] public string dateOfBirth;
            [SerializeField] internal string iD;
            [SerializeField] public Sex sex;
            [SerializeField] public string expirationDate;
            [SerializeField] public string issuingCity;
            public PassportData(EntrantData entrantData, string expirationDate)
            {
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                this.dateOfBirth = entrantData.dateOfBirth;
                this.iD = entrantData.iD;
                this.sex = entrantData.sex;
                this.expirationDate = expirationDate;
                this.issuingCity = entrantData.issuingCity;
            }
        }
        
        [System.Serializable]
        public class WorkPassData : Document
        {
            public enum Field
            {
                Accounting,
                Agriculture,
                Architecture,
                Aviation,
                Construction,
                Dentistry,
                Drafting,
                Engineering,
                FineArts,
                Fishing,
                FoodService,
                GeneralLabor,
                Healthcare,
                Manufacturing,
                Research,
                Sports,
                Statistics,
                Surveying,
            }

            [SerializeField] public string surName;
            [SerializeField] public string firstName;
            [SerializeField] public Field field;
            [SerializeField] public string expirationDate;
            
            public WorkPassData(EntrantData entrantData, string expirationDate)
            {
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                
                Array fields = Enum.GetValues(typeof(Field));
                this.field = (Field)fields.GetValue(random.Next(Enum.GetValues(typeof(Field)).Length));
                
                this.expirationDate = expirationDate;
            }
        }

        public class Speech : Document
        {
            
        }











        



        public static void SaveEntrant(EntrantData entrantData)
        {
            string json = entrantData.ToJSon();
            File.WriteAllText("Assets/Ressources/Entrants/" +entrantData.index+"Entrant.json", json);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
#endif
        }
        
        public static void SaveEntrant(string surName, string firstName, string ID, Sex sex, string dateOfBirth, string issuingCity)
        {
            EntrantData entrantData = new EntrantData(surName, firstName, ID, sex,dateOfBirth, issuingCity);
            SaveEntrant(entrantData);
        }
        
        public static EntrantData LoadEntrant(int index)
        {
            string path = "Assets/Ressources/Entrants/" +index+"Entrant.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                EntrantData entrantData = JsonUtility.FromJson<EntrantData>(json);
                return entrantData;
            }

            throw new Exception("File doesn't contain entrant with index " + index);
        }
    
    }
}
