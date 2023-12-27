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
        [Header("Passports Prefabs")] 
        [SerializeField] private GameObject antegriaPrefab;
        [SerializeField] private GameObject arstotzkaPrefab;
        [SerializeField] private GameObject imporPrefab;
        [SerializeField] private GameObject kolechiaPrefab;
        [SerializeField] private GameObject obristanPrefab;
        [SerializeField] private GameObject republiaPrefab;
        [SerializeField] private GameObject unitedFederationPrefab;
        
        [Header("Other documents Prefabs")] 
        [SerializeField] private GameObject entryPermitPrefab;
        [SerializeField] private GameObject entryTicketPrefab;
        [SerializeField] private GameObject IDCardPrefab;
        [SerializeField] private GameObject entrantBody;

        [Header("TextsAssets")] [SerializeField]
        private TextAsset surNamesTextAsset;

        [SerializeField] private TextAsset firstNameMaleTextAsset;
        [SerializeField] private TextAsset firstNameFemaleTextAsset;

        [Header("RuntimeButtons")] 

        private int maxAge = 75;
        private int minAge = 18;

        

        private IDictionary<GameObject, EntrantData> entrantDict;
        private Random random;


        private void Awake()
        {
            random = new Random();
            //entrantDict = new Dictionary<GameObject, EntrantManager.EntrantData>();

        }

        public GameObject SummonEntrantBody()
        {
            GameObject entrant = Instantiate(entrantBody);
            return entrant;
        }

        public GameObject SummonPassport(EntrantData entrantData, Vector3 offset)
        {
            GameObject passport;
            switch (entrantData.originCountry)
            {
                case Country.IMPOR:
                    passport = Instantiate(imporPrefab, transform);
                    break;
                case Country.ANTEGRIA:
                    passport = Instantiate(antegriaPrefab, transform);
                    break;
                case Country.KOLECHIA:
                    passport = Instantiate(kolechiaPrefab, transform);
                    break;
                case Country.OBRISTAN:
                    passport = Instantiate(obristanPrefab, transform);
                    break;
                case Country.REPUBLIA:
                    passport = Instantiate(republiaPrefab, transform);
                    break;
                case Country.ARSTOTZKA:
                    passport = Instantiate(arstotzkaPrefab, transform);
                    break;
                case Country.UNITED_FEDERATION:
                    passport = Instantiate(unitedFederationPrefab, transform);
                    break;
                default:
                    passport = Instantiate(arstotzkaPrefab, transform);
                    break;
            }
            passport.transform.position += offset;
            PassportScript passScript = passport.GetComponent<PassportScript>();
            passScript.SetData(entrantData.GetPassportData());
            return passport;
        }

        public GameObject SummonEntryPermit(EntrantData entrantData, Vector3 offset)
        {

            GameObject entryPermit = Instantiate(entryPermitPrefab, transform);
            entryPermit.transform.position += offset;
            EntryPermitScript entryPermitScript = entryPermit.GetComponent<EntryPermitScript>();
            entryPermitScript.SetData(entrantData.GetEntryPermitData());
            return entryPermit;
        }

        public GameObject SummonEntryTicket(EntrantData entrantData, Vector3 offset)
        {
            GameObject entryTicket = Instantiate(entryTicketPrefab, transform);
            entryTicket.transform.position += offset;
            EntryTicketScript entryTicketScript = entryTicket.GetComponent<EntryTicketScript>();
            entryTicketScript.SetData(entrantData.GetEntryTicketData());
            return entryTicket;
        }

        public GameObject SummonIDCard(EntrantData entrantData, Vector3 offset)
        {
            GameObject idCard = Instantiate(IDCardPrefab, transform);
            idCard.transform.position += offset;
            IDCardScript idCardScript = idCard.GetComponent<IDCardScript>();
            idCardScript.SetData(entrantData.GetIDCardData());
            return idCard;
        }

        public EntrantData GenerateRandomEntrantData()
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