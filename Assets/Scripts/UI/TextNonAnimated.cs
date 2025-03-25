using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextNonAnimated : MonoBehaviour
{
    #region Base Vars
    public bool isEnabled;
    public GameObject parentOfText;
    public GameObject parentObject;
    public TextMeshProUGUI textObject;
    public string textValue;
    public TMPro.TMP_FontAsset font;
    public Color textColor;
    #endregion
    #region Unity Funcs
    void Start()
    {        
        Invoke("Setup",1);
    }
    void Setup()
    {
        RectTransform canvasRect = parentObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1000, 1000);
    }
    #endregion
    #region Primary 
    public void DestroyParent()
    {
        Destroy(transform.gameObject);
    }
    #endregion
}