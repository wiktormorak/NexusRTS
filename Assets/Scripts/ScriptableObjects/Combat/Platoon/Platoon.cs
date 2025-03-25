using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Platoon", menuName = "Combat/Platoon")]
public class Platoon : ScriptableObject
{
    public string platoonName;
    public List<Squadron> allSquadrons;
}