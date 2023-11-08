using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryPermitScript : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text purposeText;
    [SerializeField] private TMP_Text durationText;
    [SerializeField] private TMP_Text enterByDateText;


    //(string name, string id, string purpose, string duration, string enterByDate)
    public void SetData((string, string, string, string, string) data)
    {
        nameText.SetText(data.Item1);
        idText.SetText(data.Item2);
        purposeText.SetText(data.Item3);
        durationText.SetText(data.Item4);
        enterByDateText.SetText(data.Item5);
    }

}
