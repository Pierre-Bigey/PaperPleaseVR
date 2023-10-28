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
    [SerializeField] private GameObject entrantBody;

    [SerializeField] private TextAsset surNamesTextAsset;
    [SerializeField] private TextAsset firstNameMaleTextAsset;
    [SerializeField] private TextAsset firstNameFemaleTextAsset;

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
        CreatePassport(GenerateEntrant());
        // CreatePassport(GenerateEntrant());
        // CreatePassport(GenerateEntrant());
        // CreatePassport(GenerateEntrant());
        //SummonEntrant(0);
    }

    private void SummonEntrant(int index)
    {
        GameObject entrant = Instantiate(entrantBody);
        EntrantManager.EntrantData entrantData = EntrantManager.LoadEntrant(index);
        entrantDict.Add(entrant,entrantData);
        CreatePassport(entrantData);
    }

    private void CreatePassport(EntrantManager.EntrantData entrantData)
    {
        GameObject passport = Instantiate(passportPrefab,transform);
        PassportScript passScript = passport.GetComponent<PassportScript>();
        passScript.SetData(entrantData.GetPassportData());
    }

    private EntrantManager.EntrantData GenerateEntrant()
    {
        string surName = getRandomName(surNamesTextAsset);
        Array sexs = Enum.GetValues(typeof(EntrantManager.Sex));
        EntrantManager.Sex sex  = (EntrantManager.Sex)sexs.GetValue(random.Next(Enum.GetValues(typeof(EntrantManager.Sex)).Length));
        string firstName;
        if(sex == EntrantManager.Sex.Male) firstName = getRandomName(firstNameMaleTextAsset);
        else firstName = getRandomName(firstNameFemaleTextAsset);
        string dateOfBirth = getRandomDateOfBirth();
        string id = GetId();
        return new EntrantManager.EntrantData(surName, firstName, id, sex, dateOfBirth, "Paris");
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

    private string getRandomDateOfBirth()
    {
        var longMonth = new List<int> {1, 2, 5, 7, 8, 10, 12};
        
        int year = random.Next(1914, 1965);
        int month = random.Next(1,13);
        
        int day;
        if (month == 2)  day = random.Next(1,32);
        else if (longMonth.Contains(month)) day = random.Next(1,30);
        else day = random.Next(1,31);

        string monthString = month.ToString();
        if (month < 10) monthString = "0" + month.ToString();

        string dayString = day.ToString();
        if (day < 10) dayString = "0" + day.ToString();
        return year.ToString()+"."+monthString+"."+dayString;
    }
    
    private string GetId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
