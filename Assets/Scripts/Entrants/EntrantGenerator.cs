using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
using UnityEngine;

public class EntrantGenerator : MonoBehaviour
{
    private void Start()
    {
        EntrantManager.SaveEntrant("Bigey","Pierre","45df78gh", EntrantManager.Sex.Male);
    }
}
