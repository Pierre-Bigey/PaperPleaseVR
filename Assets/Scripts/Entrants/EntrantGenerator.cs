using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
using UnityEngine;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Random = System.Random;

namespace Entrants
{
    public class EntrantGenerator : MonoBehaviour
    {
        [Header("Documents Prefabs")] [SerializeField]
        private GameObject passportPrefab;

        [SerializeField] private GameObject entryPermitPrefab;
        [SerializeField] private GameObject entryTicketPrefab;
        [SerializeField] private GameObject IDCardPrefab;
        [SerializeField] private GameObject entrantBody;

        [Header("TextsAssets")] [SerializeField]
        private TextAsset surNamesTextAsset;

        [SerializeField] private TextAsset firstNameMaleTextAsset;
        [SerializeField] private TextAsset firstNameFemaleTextAsset;

        [Header("RuntimeButtons")] [SerializeField]
        private bool summon;

        int maxAge = 75;
        int minAge = 18;

        private EntrantPhotographer _entrantPhotographer;

        private IDictionary<GameObject, EntrantData> entrantDict;
        private Random random;


        private void Awake()
        {
            random = new Random();
            //entrantDict = new Dictionary<GameObject, EntrantManager.EntrantData>();

        }

        private void Start()
        {

            _entrantPhotographer = FindObjectOfType<EntrantPhotographer>();
        }

        private void Update()
        {
            if (summon)
            {
                summon = false;
                SummonEntrant();
            }
        }

        private void SummonEntrant()
        {
            GameObject entrant = Instantiate(entrantBody, transform);
            entrant.transform.Rotate(Vector3.up, 180);
            EntrantData entrantData = GenerateEntrant();


            int currentDay = (int) (GameManager.Instance.date - GameManager.Instance.startDate).TotalDays +1 ;
            List<RuleDocToPresent> ruleDocToPresents = RulesManager.DocToPresentEachDay[currentDay];

            foreach (var rule in ruleDocToPresents)
            {
                if (rule.subject.IsConcerned(entrantData.originCountry, entrantData.type))
                {
                    switch (rule.docToPresent)
                    {
                        case DocumentType.PASSPORT:
                            SummonPassport(entrantData, new Vector3(0f, 0.9f, -0.5f));
                            break;
                        case DocumentType.ID_CARD:
                            SummonIDCard(entrantData, new Vector3(0, 0.9f, -0.6f));
                            break;
                        case DocumentType.WORK_PASS:
                            //TODO Summon WorkPass
                            break;
                        case DocumentType.DIPLO_AUTH:
                            //TODO
                            break;
                        case DocumentType.ENTRY_PERMIT:
                            SummonEntryPermit(entrantData, new Vector3(0.2f, 0.9f, -0.5f));
                            break;
                        case DocumentType.ENTRY_TICKET:
                            SummonEntryTicket(entrantData, new Vector3(-0.2f, 0.9f, -0.5f));
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
            
            
            
            _entrantPhotographer.PhotoEntrant(entrantBody);

        }

        private void SummonPassport(EntrantData entrantData, Vector3 offset)
        {
            GameObject passport = Instantiate(passportPrefab, transform);
            passport.transform.position += offset;
            PassportScript passScript = passport.GetComponent<PassportScript>();
            passScript.SetData(entrantData.GetPassportData());
        }

        private void SummonEntryPermit(EntrantData entrantData, Vector3 offset)
        {

            GameObject entryPermit = Instantiate(entryPermitPrefab, transform);
            entryPermit.transform.position += offset;
            EntryPermitScript entryPermitScript = entryPermit.GetComponent<EntryPermitScript>();
            entryPermitScript.SetData(entrantData.GetEntryPermitData());
        }

        private void SummonEntryTicket(EntrantData entrantData, Vector3 offset)
        {
            GameObject entryTicket = Instantiate(entryTicketPrefab, transform);
            entryTicket.transform.position += offset;
            EntryTicketScript entryTicketScript = entryTicket.GetComponent<EntryTicketScript>();
            entryTicketScript.SetData(entrantData.GetEntryTicketData());
        }

        private void SummonIDCard(EntrantData entrantData, Vector3 offset)
        {
            GameObject idCard = Instantiate(IDCardPrefab, transform);
            idCard.transform.position += offset;
            IDCardScript idCardScript = idCard.GetComponent<IDCardScript>();
            idCardScript.SetData(entrantData.GetIDCardData());
        }

        private EntrantData GenerateEntrant()
        {
            Sex sex = GetRandomSex();
            string surName = getRandomSurName(surNamesTextAsset, sex);
            string firstName;
            if (sex == Sex.Male) firstName = getRandomName(firstNameMaleTextAsset);
            else firstName = getRandomName(firstNameFemaleTextAsset);
            DateTime dateOfBirth = getRandomDateOfBirth();
            string id = GetId();
            Country originCountry = GetRandomCountry();
            EntrantType type;
            if (originCountry != Country.ARSTOTZKA) type = EntrantType.CITIZEN;
            else type = GetRandomForeignerType();

            string issuincity = GetRandomIssuingCity(originCountry);

            return new EntrantData(surName, firstName, id, sex, dateOfBirth, issuincity, originCountry,
                type);
        }

        public static List<string> getData(TextAsset textAsset)
        {
            List<string> data = new List<string>();
            StringReader reader = new StringReader(textAsset.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                data.Add(line);
            }

            return data;
        }

        private string getRandomName(TextAsset textAsset)
        {
            List<string> nameList = getData(textAsset);
            //List<string> nameList = getData("Assets/Ressources/"+fileName+".csv");
            return nameList[random.Next(nameList.Count)];
        }

        private string getRandomSurName(TextAsset textAsset, Sex sex)
        {
            List<string> surname = getData(textAsset);
            //List<string> nameList = getData("Assets/Ressources/"+fileName+".csv");
            string name = surname[random.Next(surname.Count)];
            if (name.Contains("/"))
            {
                string[] names = name.Split("/");
                if (sex == Sex.Male) return names[0];
                return names[1];
            }

            return name;
        }

        private DateTime getRandomDateOfBirth()
        {
            int age = random.Next(minAge, maxAge);
            int daySinceBirthday = random.Next(366);
            DateTime birthDay = GameManager.Instance.date.AddYears(-age).AddDays(-daySinceBirthday);

            return birthDay;
        }

        private string GetId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private Sex GetRandomSex()
        {
            Array sexs = Enum.GetValues(typeof(Sex));
            return (Sex)sexs.GetValue(random.Next(Enum.GetValues(typeof(Sex)).Length));
        }

        private Country GetRandomCountry()
        {
            Array countries = Enum.GetValues(typeof(Country));
            return (Country)countries.GetValue(random.Next(Enum.GetValues(typeof(Country))
                .Length));
        }

        private EntrantType GetRandomForeignerType()
        {
            Array entrantTypes = Enum.GetValues(typeof(EntrantType));
            return (EntrantType)entrantTypes.GetValue(random.Next(1,
                Enum.GetValues(typeof(EntrantType)).Length));
        }

        private string GetRandomIssuingCity(Country country)
        {
            string[] issuingCities = RulesManager.IssuingCities[country];
            return (string)issuingCities.GetValue(random.Next(issuingCities.Length));
        }
    }
}