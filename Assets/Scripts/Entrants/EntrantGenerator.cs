using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
using UnityEngine;
using System.IO;
using System.Linq;
using Random = System.Random;

public class EntrantGenerator : MonoBehaviour
{
    [SerializeField] private GameObject passportPrefab;
    [SerializeField] private GameObject entryPermitPrefab;
    [SerializeField] private GameObject entrantBody;

    [SerializeField] private TextAsset surNamesTextAsset;
    [SerializeField] private TextAsset firstNameMaleTextAsset;
    [SerializeField] private TextAsset firstNameFemaleTextAsset;
    
    int maxAge = 75;
    int minAge = 18;

    private IDictionary<GameObject, EntrantManager.EntrantData> entrantDict;
    private Random random;


    private void Awake()
    {
        random = new Random();
        entrantDict = new Dictionary<GameObject, EntrantManager.EntrantData>();
    }

    private void Start()
    {
        
        //EntrantManager.SaveEntrant("Bigey","Pierre","45df78gh", EntrantManager.Sex.Male);
        //EntrantManager.EntrantData entrantData = EntrantManager.LoadEntrant(0);
        //Debug.Log(entrantData.ToJSon());
        //EntrantManager.EntrantData entrantData1 = EntrantManager.LoadEntrant(1);
        //for(int i = 0; i<10;i++) Debug.Log(getRandomDateOfBirth());
        EntrantManager.EntrantData entrantData = GenerateEntrant();
        SummonPassport(entrantData);
        SummonEntryPermit(entrantData);
        // SummonPassport(GenerateEntrant());
        // SummonPassport(GenerateEntrant());
        // SummonPassport(GenerateEntrant());
        //SummonEntrant(0);
    }

    private void SummonEntrant(int index)
    {
        GameObject entrant = Instantiate(entrantBody);
        EntrantManager.EntrantData entrantData = EntrantManager.LoadEntrant(index);
        entrantDict.Add(entrant,entrantData);
        SummonPassport(entrantData);
    }

    private void SummonPassport(EntrantManager.EntrantData entrantData)
    {
        GameObject passport = Instantiate(passportPrefab,transform);
        PassportScript passScript = passport.GetComponent<PassportScript>();
        passScript.SetData(entrantData.GetPassportData());
    }

    private void SummonEntryPermit(EntrantManager.EntrantData entrantData)
    {
        GameObject entryPermit = Instantiate(entryPermitPrefab, transform);
        EntryPermitScript entryPermitScript = entryPermit.GetComponent<EntryPermitScript>();
        entryPermitScript.SetData(entrantData.GetEntryPermitData());
    }

    private EntrantManager.EntrantData GenerateEntrant()
    {
        EntrantManager.Sex sex = GetRandomSex();
        string surName = getRandomSurName(surNamesTextAsset,sex);
        string firstName;
        if(sex == EntrantManager.Sex.Male) firstName = getRandomName(firstNameMaleTextAsset);
        else firstName = getRandomName(firstNameFemaleTextAsset);
        DateTime dateOfBirth = getRandomDateOfBirth();
        string id = GetId();
        EntrantManager.Country originCountry = GetRandomCountry();
        EntrantManager.EntrantType type;
        if (originCountry != EntrantManager.Country.ARSTOTKKA) type = EntrantManager.EntrantType.CITIZEN;
        else type = GetRandomForeignerType();

        string issuincity = GetRandomIssuingCity(originCountry);

        return new EntrantManager.EntrantData(surName, firstName, id, sex, dateOfBirth, issuincity, originCountry, type);
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
    
    private string getRandomSurName(TextAsset textAsset, EntrantManager.Sex sex)
    {
        List<string> surname = getData(textAsset);
        //List<string> nameList = getData("Assets/Ressources/"+fileName+".csv");
        string name = surname[random.Next(surname.Count)];
        if (name.Contains("/"))
        {
            string[] names = name.Split("/");
            if (sex == EntrantManager.Sex.Male) return names[0];
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

    private EntrantManager.Sex GetRandomSex()
    {
        Array sexs = Enum.GetValues(typeof(EntrantManager.Sex));
        return (EntrantManager.Sex) sexs.GetValue(random.Next(Enum.GetValues(typeof(EntrantManager.Sex)).Length));
    }
    
    private EntrantManager.Country GetRandomCountry()
    {
        Array countries = Enum.GetValues(typeof(EntrantManager.Country));
        return (EntrantManager.Country) countries.GetValue(random.Next(Enum.GetValues(typeof(EntrantManager.Country)).Length));
    }
    
    private EntrantManager.EntrantType GetRandomForeignerType()
    {
        Array entrantTypes = Enum.GetValues(typeof(EntrantManager.EntrantType));
        return (EntrantManager.EntrantType) entrantTypes.GetValue(random.Next(1,Enum.GetValues(typeof(EntrantManager.EntrantType)).Length));
    }

    private string GetRandomIssuingCity(EntrantManager.Country country)
    {
        string[] issuingCities = EntrantManager.IssuingCities[country];
        return (string) issuingCities.GetValue(random.Next(issuingCities.Length));
    }
}
