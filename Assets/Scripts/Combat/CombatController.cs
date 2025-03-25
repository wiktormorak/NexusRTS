using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    #region External Vars
    public GameObject controllerParent;
    public UIController uiController;
    #endregion
    #region Scriptable Objects
    public Squadron squadronSO;
    public CombatUnit combatUnitSO;
    #endregion
    #region Text Parent Objects, Colors and Fonts
    private GameObject MoneyAnimate;
    private GameObject ResearchAnimate;
    private GameObject CivilianAnimate;
    private GameObject BottomAnimate;
    private Color moneyColor;
    private Color researchColor;
    private Color civilianColor;
    private Color positiveColor;
    private Color negativeColor;
    private Color normalColor;
    private TMPro.TMP_FontAsset UrbanistMedium;
    private TMPro.TMP_FontAsset UrbanistBold;
    #endregion
    #region Unity Funcs
    void Start()
    {
        Invoke("ExternalVars", 1);
        Invoke("SetColours", 1);
        Invoke("FindAnimateGameObjects", 1);
    }

    void Update()
    {
        
    }
    void ExternalVars()
    {
        controllerParent = GameObject.FindWithTag("UIController");
        uiController = controllerParent.GetComponent<UIController>();
    }
    void SetColours()
    {
        moneyColor = new Color(0.388f, 0.709f, 0.203f, 1f);
        researchColor = new Color(0f, 0.478f, 0.929f, 1f);
        civilianColor = new Color(0.929f, 0.603f, 0f, 1f);
        positiveColor = new Color(0.376f, 0.643f, 0.223f, 1f);
        negativeColor = new Color(1f, 0f, 0f, 1f);
        normalColor = new Color(0.941f, 0.921f, 0.898f, 1f);
    }
    void FindAnimateGameObjects()
    {
        MoneyAnimate = GameObject.FindWithTag("MoneyAnimate");
        ResearchAnimate = GameObject.FindWithTag("ResearchAnimate");
        CivilianAnimate = GameObject.FindWithTag("CivilianAnimate");
        BottomAnimate = GameObject.FindWithTag("BottomAnimate");
    }
    #endregion
    #region Squadron and Platoon Funcs
    public void AddCombatUnitToSquadron(Squadron squadron, CombatUnit unit, Building building) 
    {
        if (building.buildingID == squadron.parentBuilding.buildingID) 
        {
            if(squadron.allCombatUnits.Count >= 0) 
            {
                if(unit.unitID != squadron.unitID)
                {
                    uiController.GetComponent<UIController>().CreateTextObject("Text", BottomAnimate, negativeColor, new Vector3(0f,40f,0f), "negative", ("Unit does not match chosen Squadron Unit type."), UrbanistBold);
                }
                else if(unit.unitID == squadron.unitID)
                {
                    squadron.allCombatUnits.Add(unit);
                }
            }
            else if(squadron.allCombatUnits.Count == 12)
            {
                uiController.GetComponent<UIController>().CreateTextObject("Text", BottomAnimate, positiveColor, new Vector3(0f,40f,0f), "negative", (squadron.squadronName + " is full."), UrbanistBold);
            }
        }
    }
    public void RemoveCombatUnitFromSquadron(Squadron squadron, CombatUnit unit)
    {
        squadron.allCombatUnits.Remove(unit);
        uiController.GetComponent<UIController>().CreateTextObject("Text", BottomAnimate, negativeColor, new Vector3(0f,40f,0f), "negative", (unit.displayName + "(" + unit.unitID + ")" + " was removed from the " + squadron.squadronName + "."), UrbanistBold);
    }
    public void CreateSquadron(Platoon platoon, Building parentBuilding, CombatUnit unit, string squadronName) 
    {
        Squadron tempSquadron = ScriptableObject.CreateInstance<Squadron>();
        tempSquadron.name = squadronName;
        tempSquadron.parentBuilding = parentBuilding;
        tempSquadron.unitID = unit.unitID;
        platoon.allSquadrons.Add(tempSquadron);
        uiController.GetComponent<UIController>().CreateTextObject("Text", BottomAnimate, positiveColor, new Vector3(0f,40f,0f), "negative", (squadronName + " was created under " + platoon.platoonName + "."), UrbanistBold);
    }
    public void DeleteSquadron(Platoon platoon, string squadronName)
    {
        var selectedSquadron = platoon.allSquadrons.Find(Squadron => Squadron.squadronName == squadronName);
        platoon.allSquadrons.Remove(selectedSquadron);
        uiController.GetComponent<UIController>().CreateTextObject("Text", BottomAnimate, negativeColor, new Vector3(0f,40f,0f), "negative", (squadronName + " was deleted under " + platoon.platoonName + "."), UrbanistBold);
    }
    #endregion
}