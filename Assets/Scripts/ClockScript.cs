using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    [SerializeField] private Transform Hours;
    [SerializeField] private Transform Minutes;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.MinuteAdded += UpdateClock;
    }

    public void UpdateClock()
    {
        
        (float hour, float minute) = GameManager.Instance.inGameTime;
        Hours.localRotation = Quaternion.Euler (0, 0, (hour + minute/60) / 12 * 360);
        Minutes.localRotation = Quaternion.Euler (0, 0, minute / 60 * 360);
    }
}
