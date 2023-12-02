using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public int maxLevel;

    public SaveData (Frog f){
        maxLevel = Math.Max(maxLevel, f.currentLevel);
    }
}
