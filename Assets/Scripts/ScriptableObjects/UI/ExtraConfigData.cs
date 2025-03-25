using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ExtraConfigData", menuName = "UI/ExtraConfigData")]
public class ExtraConfigData : ScriptableObject
{
    [Header("UI Elements")]
    public List<GameObject> extraInfoFrames;
    public List<TMPro.TMP_Text> extraInfoText;
}