using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Documents;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entrants
{
    #region Enums
    public enum Country
    {
        ANTEGRIA,
        ARSTOTZKA,
        IMPOR,
        KOLECHIA,
        OBRISTAN,
        REPUBLIA,
        UNITED_FEDERATION
    }
    
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
        CERTIF_OF_VACCINATION,
        CALENDAR,
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
        CALENDAR_DATE,
    }
    
    #endregion   
    
    #region Documents 
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
            int dayLeft = Random.Range(10, 3000);
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
            this.field = (WorkField)fields.GetValue(Random.Range(0,Enum.GetValues(typeof(WorkField)).Length));
            
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
                    duration = limitedDurations[Random.Range(0,2)];
                    break;
                case EntrantType.TOURIST:
                    duration = limitedDurations[Random.Range(1, 5)];
                    break;
                case EntrantType.WORKER:
                    duration = limitedDurations[Random.Range(1, 7)];
                    break;
                case EntrantType.IMMIGRANT:
                    duration = "forever";
                    break;
            }
            this.duration = duration;

            int daysLeft = Random.Range(0,8);
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
            this.district = (District)districts.GetValue(Random.Range(0,Enum.GetValues(typeof(District)).Length));
            this.surName = entrantData.surName;
            this.firstName = entrantData.firstName;
            this.dateOfBirth = entrantData.dateOfBirth;
            this.height = entrantData.height;
            this.weight = entrantData.weight;
        }
    }
    
    #endregion
    
    

    [System.Serializable]
    public class EntrantData
    {
        protected static int EntrantCount;
        
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
            index = EntrantCount;
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
            
            workPass = new WorkPassData(this);
            entryPermit = new EntryPermitData(this);
            entryTicket = new EntryTicketData(this);
            idCard = new IDCardData(this);

            

        }

        //(string name, DateTime exp, string iss, Sex sex, DateTime dob, string id)
        public (string, DateTime, string, Sex, DateTime, string) GetPassportData()
        {
            PassportData ps = this.passport;
            string sexString = ps.sex.ToString();
            //Return Name, EXP, ISS, SEX, DOB, ID 
            return (ps.firstName + ", " + ps.surName, ps.expirationDate, ps.issuingCity,  ps.sex,
                ps.dateOfBirth, ps.iD);
        }
        
        //(string name, string id, EntrantType purpose, string duration, DateTime entryByDate)
        public (string, string, EntrantType, string, DateTime) GetEntryPermitData()
        {
            EntryPermitData ep = this.entryPermit;
            //Return Name, EXP, ISS, SEX, DOB, ID 
            return (ep.firstName + ", " + ep.surName, ep.iD, ep.purpose,  ep.duration,
                ep.enterByDate);
        }

        public DateTime GetEntryTicketData()
        {
            return this.entryTicket.validOnDate;
        }

        //(District districtName, string name, DateTime dob, int height, int weight)
        public (District, string, DateTime, int, int) GetIDCardData()
        {
            IDCardData idc = this.idCard;
            return (idc.district, idc.firstName + ", " + idc.surName,
                idc.dateOfBirth, idc.height, idc.weight);
        }
        
        
        
        
        public string ToJSon()
        {
            string json = JsonUtility.ToJson(this);
            //Debug.Log(json);
            return json;
        }
        
        private void Errorate()
        {
            // bool goingToCreate  = incorrectRate<random.NextDouble();
            // if(goingToCreate) GenerateError();
        }

        private void GenerateError()
        {
            isCorrect = false;
            Document documentToErrorate = documentsList[Random.Range(0,documentsList.Count)];
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
    
    
    public class EntrantManager : MonoBehaviour
    {
        private static EntrantManager _instance;
        public static EntrantManager Instance
        {
            get { return _instance;  }
        }
        
        [Header("Script references")]
        [SerializeField] private EntrantGenerator _entrantGenerator;
        [SerializeField] private EntrantPhotographer _entrantPhotographer;

        
        [Header("World positions")] 
        [SerializeField] private Transform documentsSpawnPos;
        [SerializeField] private Transform entrantSpawnPos;
        [SerializeField] private Vector3 documentThrowForce;
        
        
        public static double incorrectRate = 0.65;

        private GameObject currentEntrantBody;
        private EntrantData currentEntrantData;

        private Dictionary<DocumentType, GameObject> documentDictionary;
        private bool accessGranted = false;
        private bool isReturningDocuments = false;


        private void Awake()
        {
            //Singleton instantiation
            if (_instance != null && _instance != this) Destroy(gameObject);
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            SummonEntrant();
        }

        public void SummonEntrant()
        {
            currentEntrantBody = _entrantGenerator.SummonEntrantBody();
            currentEntrantBody.transform.rotation = entrantSpawnPos.transform.rotation;
            currentEntrantBody.transform.position = entrantSpawnPos.position;
            
            currentEntrantData = _entrantGenerator.GenerateRandomEntrantData();

            SummonCurrentDocuments();
            _entrantPhotographer.PhotoEntrant(currentEntrantBody);
            foreach (var (type,document) in documentDictionary)
            {
                ThrowDocument(document);
            }

            isReturningDocuments = false;
            accessGranted = false;
        }

        public void SummonCurrentDocuments()
        {
            int currentDay = (int) (GameManager.Instance.date - GameManager.Instance.startDate).TotalDays +1 ;
            List<RuleDocToPresent> ruleDocToPresents = RulesManager.DocToPresentEachDay[currentDay];

            documentDictionary = new Dictionary<DocumentType, GameObject>();
            
            foreach (var rule in ruleDocToPresents)
            {
                if (rule.subject.IsConcerned(currentEntrantData.originCountry, currentEntrantData.type))
                {
                    switch (rule.docToPresent)
                    {
                        case DocumentType.PASSPORT:
                            GameObject passport = _entrantGenerator.SummonPassport(currentEntrantData, new Vector3(0f, 0.9f, -0.5f));
                            documentDictionary.Add(DocumentType.PASSPORT,passport);
                            break;
                        case DocumentType.ID_CARD:
                            GameObject idcard = _entrantGenerator.SummonIDCard(currentEntrantData, new Vector3(0, 0.9f, -0.6f));
                            documentDictionary.Add(DocumentType.ID_CARD,idcard);
                            break;
                        case DocumentType.WORK_PASS:
                            //TODO Summon WorkPass
                            break;
                        case DocumentType.DIPLO_AUTH:
                            //TODO
                            break;
                        case DocumentType.ENTRY_PERMIT:
                            GameObject entrypermit = _entrantGenerator.SummonEntryPermit(currentEntrantData, new Vector3(0.2f, 0.9f, -0.5f));
                            documentDictionary.Add(DocumentType.ENTRY_PERMIT,entrypermit);
                            break;
                        case DocumentType.ENTRY_TICKET:
                            GameObject entryticket = _entrantGenerator.SummonEntryTicket(currentEntrantData, new Vector3(-0.2f, 0.9f, -0.5f));
                            documentDictionary.Add(DocumentType.ENTRY_TICKET,entryticket);
                            break;
                        case DocumentType.ID_SUPPLEMENT:
                            //TODO
                            break;
                        case DocumentType.GRANT_OF_ASYLUM:
                            //TODO
                            break;
                        case DocumentType.ACCESS_PERMIT:
                            //TODO
                            break;
                        case DocumentType.CERTIF_OF_VACCINATION:
                            //TODO
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ThrowDocument(GameObject document)
        {
            Debug.Log("Throwing object : "+document.name);
            if (document.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(documentThrowForce,ForceMode.Impulse);
                document.transform.position = documentsSpawnPos.position;
                return;
            }
            
        }

        public void ProcessReturnedDocument(GameObject document)
        {
            try
            {
                PassportScript passportScript = document.transform.parent.gameObject.GetComponent<PassportScript>();
                
                Debug.Log("We recieve a passport, stamped : "+passportScript.isStamped);
                
                if (passportScript.isStamped)
                {
                    isReturningDocuments = true;
                    accessGranted = passportScript.accessGranted;
                    
                    Destroy(documentDictionary[DocumentType.PASSPORT]);
                    documentDictionary.Remove(DocumentType.PASSPORT);
                }
                else
                {
                    document.transform.position = documentsSpawnPos.position;
                    Rigidbody rb = document.transform.parent.gameObject.GetComponent<Rigidbody>();
                    rb.AddForce(documentThrowForce,ForceMode.Impulse);
                }
                
            }
            catch
            {
                Debug.Log("Cannot get passportScript");
            }

            if (!isReturningDocuments) ThrowDocument(document);
            else if (document.TryGetComponent(out DocumentScript documentScript))
            {
                DocumentType docType = documentScript.docType;
                Destroy(documentDictionary[docType],1);
                documentDictionary.Remove(docType);
            }

            if (documentDictionary.Count == 0)
            {
                Destroy(currentEntrantBody,1);
                Invoke("SummonEntrant",2);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ProcessReturnedDocument(other.gameObject);
        }


        public static void SaveEntrant(EntrantData entrantData)
        {
            string json = entrantData.ToJSon();
            File.WriteAllText("Assets/Ressources/Entrants/" +entrantData.index+"Entrant.json", json);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh ();
#endif
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
