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

        public enum EntrantType
        {
            CITIZEN,
            IMMIGRANT,
            TRANSIENT,
            TOURIST,
            WORKER,
            ASYLUM_SEEKER,
            DIPLOMAT,
            JOURNALIST
        }
        
        public enum WorkField
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

        public enum Country
        {
            ANTEGRIA,
            ARSTOTKKA,
            IMPOR,
            KOLECHIA,
            OBRISTAN,
            REPUBLIA,
            UNITED_FEDERATION,
        }

        public enum District
        {
            ALTAN,
            VESCILLO,
            BURNTON,
            OCTOVALIS,
            GENNISTORA,
            LENDIFORMA,
            WOZENFIELD,
            FARDESTO,
        }

        public enum DocumentType
        {
            PASSPORT,
            ENTRY_TICKET,
            ID_CARD,
            DIPLO_AUTH,
            ENTRY_PERMIT,
            ACCESS_PERMIT,
            WORK_PASS,
            GRANT_OF_ASYLUM,
            ID_SUPPLEMENT,
            CERTIF_OF_VACCINATION
        }

        public enum InfoType
        {
            NAME,
            DOB, 
            PASS_ISS,
            EXP_DATE,
            PHOTO,
            SEX,
            ID,
            DISTRICT,
            ENTRYPURPOSE,
            ACCESS_DURATION,
            HEIGHT,
            WEIGHT,
            DESCRIPTION,
            WORK_SEAL,
            WORK_END_DATE,
            ENTER_BY_DATE,
            VALID_ON_DATE,
            COUNTRY,
            ALIAS,
            COUNTRY_ACCESS,
            VACCINE,
        }
        
        public static Dictionary<Country, string[]> IssuingCities = new()
        {
            {Country.ANTEGRIA, new string[] {"St. Marmero", "Glorian", "Outer Grouse"} },
            {Country.ARSTOTKKA, new string[] {"Orvech Vonor", "East Grestin", "Paradizna"} },
            {Country.IMPOR, new string[] {"Enkyo", "Haihan", "Tsunkeido"} },
            {Country.KOLECHIA, new string[] {"Yurko City", "Vedor", "West Grestin"} },
            {Country.OBRISTAN, new string[] {"Skal", "Lorndaz", "Mergerous"} },
            {Country.REPUBLIA, new string[] {"True Glorian", "Lesrenadi", "Bostan"} },
            {Country.UNITED_FEDERATION, new string[] {"Great Rapid", "Shingleton", "Korista City"} },
        };


        public static double incorrectRate = 0.45;

        protected static int EntrantCount;
        
        [System.Serializable]
        public class EntrantData
        {
            [SerializeField] internal int index;
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal DateTime dateOfBirth;
            [SerializeField] internal string issuingCity;
            [SerializeField] internal string iD;
            [SerializeField] internal Sex sex;
            [SerializeField] internal Country originCountry;
            [SerializeField] internal EntrantType type;
            
            
            [SerializeField] internal int height;
            [SerializeField] internal int weight;
            
            
            internal bool isCorrect = true;
            private List<Document> documentsList;
            
            [SerializeField] internal PassportData passport;
            [SerializeField] internal EntryPermitData entryPermit;
            [SerializeField] internal WorkPassData workPass;
            [SerializeField] internal EntryTicketData entryTicket;
            [SerializeField] internal IDCardData idCard;

            public EntrantData(string surName, string firstName, string iD, Sex sex, DateTime dateOfBirth, string issuingCity, Country originCountry, EntrantType type)
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
                this.originCountry = originCountry;
                this.type = type;
                
                this.height = 180;
                this.weight = 70;
                
                
                passport =  new PassportData(this);
                Debug.Log("[EM] Type of passport is " + passport.GetType());
                workPass = new WorkPassData(this);
                entryPermit = new EntryPermitData(this);
                entryTicket = new EntryTicketData(this);
                idCard = new IDCardData(this);

                

            }

            //(string name, string exp, string iss, string sex, string dob, string id)
            public (string, string, string, string, string, string) GetPassportData()
            {
                PassportData ps = this.passport;
                string sexString = ps.sex.ToString();
                //Return Name, EXP, ISS, SEX, DOB, ID 
                return (ps.firstName + ", " + ps.surName, ps.expirationDate.ToShortDateString(), ps.issuingCity,  ps.sex.ToString()[0].ToString(),
                    ps.dateOfBirth.ToShortDateString(), ps.iD);
            }
            
            //(string name, string id, string purpose, string duration, string entryByDate)
            public (string, string, string, string, string) GetEntryPermitData()
            {
                EntryPermitData ep = this.entryPermit;
                //Return Name, EXP, ISS, SEX, DOB, ID 
                return (ep.firstName + ", " + ep.surName, ep.iD, ep.purpose.ToString(),  ep.duration,
                    ep.enterByDate.ToShortDateString());
            }

            public string GetEntryTicketData()
            {
                return this.entryTicket.validOnDate.ToShortDateString();
            }

            //(string districtName, string name, string dob, string height, string weight)
            public (string, string, string, string, string) GetIDCardData()
            {
                IDCardData idc = this.idCard;
                return (idc.district.ToString(), idc.firstName + ", " + idc.surName,
                    idc.dateOfBirth.ToShortDateString(), idc.height.ToString(), idc.weight.ToString());
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
            [SerializeField] internal DateTime dateOfBirth;
            [SerializeField] internal string iD;
            [SerializeField] internal Sex sex;
            [SerializeField] internal DateTime expirationDate;
            [SerializeField] internal string issuingCity;
            public PassportData(EntrantData entrantData)
            {
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                this.dateOfBirth = entrantData.dateOfBirth;
                this.iD = entrantData.iD;
                this.sex = entrantData.sex;
                int dayLeft = random.Next(10, 3000);
                DateTime expDate = GameManager.Instance.date.AddDays(dayLeft);
                this.expirationDate = expDate;
                this.issuingCity = entrantData.issuingCity;
            }
        }
        
        [System.Serializable]
        public class WorkPassData : Document
        {
            

            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal WorkField field;
            [SerializeField] internal string expirationDate;
            
            public WorkPassData(EntrantData entrantData)
            {
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                
                Array fields = Enum.GetValues(typeof(WorkField));
                this.field = (WorkField)fields.GetValue(random.Next(Enum.GetValues(typeof(WorkField)).Length));
                
                this.expirationDate = DateTime.Today.ToString();
            }
        }

        public class Speech : Document
        {
            
        }

        [System.Serializable]
        public class EntryPermitData : Document
        {
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal string iD;
            [SerializeField] internal EntrantType purpose;
            [SerializeField] internal string duration;
            [SerializeField] internal DateTime enterByDate;
            public EntryPermitData(EntrantData entrantData)
            {
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                this.iD = entrantData.iD;
                this.purpose = entrantData.type;
                string duration = "";

                List<string> limitedDurations = new List<string>() { "2 days", "14 days", "1 month", "2 month", "3 month", "6 month", "1 year" };
                
                switch (purpose)
                {
                    case EntrantType.TRANSIENT:
                        duration = limitedDurations[random.Next(2)];
                        break;
                    case EntrantType.TOURIST:
                        duration = limitedDurations[random.Next(1, 5)];
                        break;
                    case EntrantType.WORKER:
                        duration = limitedDurations[random.Next(1, 7)];
                        break;
                    case EntrantType.IMMIGRANT:
                        duration = "forever";
                        break;
                }
                this.duration = duration;

                int daysLeft = random.Next(8);
                this.enterByDate = GameManager.Instance.date.AddDays(daysLeft);
            }
        }


        [System.Serializable]
        public class EntryTicketData : Document
        {
            
            [SerializeField] public DateTime validOnDate;

            public EntryTicketData(EntrantData entrantData)
            {
                this.validOnDate = GameManager.Instance.date;
            }
        }

        [System.Serializable]
        public class IDCardData : Document
        {
            [SerializeField] internal District district;
            [SerializeField] internal string surName;
            [SerializeField] internal string firstName;
            [SerializeField] internal DateTime dateOfBirth;
            [SerializeField] internal int height;
            [SerializeField] internal int weight;

            public IDCardData(EntrantData entrantData)
            {
                Array districts = Enum.GetValues(typeof(District));
                this.district = (District)districts.GetValue(random.Next(Enum.GetValues(typeof(District)).Length));
                this.surName = entrantData.surName;
                this.firstName = entrantData.firstName;
                this.dateOfBirth = entrantData.dateOfBirth;
                this.height = entrantData.height;
                this.weight = entrantData.weight;
            }
        }

        public static void SaveEntrant(EntrantData entrantData)
        {
            string json = entrantData.ToJSon();
            File.WriteAllText("Assets/Ressources/Entrants/" +entrantData.index+"Entrant.json", json);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
#endif
        }
        
        /*public static void SaveEntrant(string surName, string firstName, string ID, Sex sex, string dateOfBirth, string issuingCity, Country origin, EntrantType type)
        {
            EntrantData entrantData = new EntrantData(surName, firstName, ID, sex,dateOfBirth., issuingCity, origin, type);
            SaveEntrant(entrantData);
        }*/
        
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
