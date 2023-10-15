using System;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Entrants
{
    
    public class EntrantManager
    {
        public enum Sex {Male, Female}; 
        [System.Serializable]
        public class EntrantData
        {
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal string iD;
            [SerializeField] internal Sex sex;

            [SerializeField] internal PassportData passport;
            [SerializeField] internal WorkPassData workPass;

            public EntrantData(string surName, string firstName, string iD, Sex sex)
            {
                this.surName = surName;
                this.firstName = firstName;
                this.iD = iD;
                this.sex = sex;
                this.passport = new PassportData(this, DateTime.Today.ToString(new CultureInfo("ja-JP")));
                this.workPass = new WorkPassData(this, DateTime.Today.ToString(new CultureInfo("ja-JP")));
            }

            public string getFileName()
            {
                return surName + "-" + firstName;
            }
            public string SaveToString()
            {
                string json = JsonUtility.ToJson(this);
                Debug.Log(json);
                return json;
            }
        
        }

        [System.Serializable]
        public class PassportData
        {
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal string iD;
            [SerializeField] public Sex sex;
            [SerializeField] public string expirationDate;
            public PassportData(EntrantData entrantData, string expirationDate)
            {
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                this.iD = entrantData.iD;
                this.sex = entrantData.sex;
                this.expirationDate = expirationDate;
            }
        }
        
        [System.Serializable]
        public class WorkPassData
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
                Array values = Enum.GetValues(typeof(Field));

                Random random = new Random();
                this.field = (Field)values.GetValue(random.Next(Enum.GetValues(typeof(Field)).Length));
                this.expirationDate = expirationDate;
            }
        }
        
        
        












        public static void SaveEntrant(EntrantData entrantData)
        {
            string json = entrantData.SaveToString();
            File.WriteAllText("Assets/Ressources/Entrants/" +entrantData.getFileName()+".json", json);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
#endif
        }
        
        public static void SaveEntrant(string surName, string firstName, string ID, Sex sex)
        {
            EntrantData entrantData = new EntrantData(surName, firstName, ID, sex);
            SaveEntrant(entrantData);
        }
    
    }
}
