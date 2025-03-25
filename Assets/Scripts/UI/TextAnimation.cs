using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    #region Base Vars
    public bool isEnabled;
    public GameObject parentOfText;
    public GameObject parentObject;
    public TextMeshProUGUI textObject;
    public float textObjectAlpha;
    public string textValue;
    public Vector3 goalPosition;
    private float goalAlpha;
    public TMPro.TMP_FontAsset font;
    public Color textColor;
    #endregion
    #region Unity Funcs
    void Start()
    {        
        Invoke("Setup",1);
    }
    void Update()
    {
        if(isEnabled)
        {
            StartCoroutine(AnimateTextPositionAndAlpha(parentObject, goalPosition, goalAlpha));
        }
    }
    void Setup()
    {
        goalAlpha = 0f;
        RectTransform canvasRect = parentObject.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1000, 1000);
        isEnabled = true;
    }
    #endregion
    #region Primary 
    IEnumerator AnimateTextPositionAndAlpha(GameObject parentObject, Vector3 goalPosition, float goalAlpha)
    {
        RectTransform canvasRect = parentObject.GetComponent<RectTransform>();
        float textObjectAlpha = textObject.color.a;
        bool isRunning = false;
        if(!isRunning && textObjectAlpha >= 0f && canvasRect.localPosition.y <= goalPosition.y)
        {
            Color newColor = textObject.color;
            newColor.a -= Time.deltaTime;
            textObject.color = newColor;
            canvasRect.localPosition += new Vector3(0f, Time.deltaTime * 40f, 0f);
            isRunning = true;
        }
        else if(!isRunning && textObjectAlpha >= 0f && canvasRect.localPosition.y >= goalPosition.y)
        {
            Color newColor = textObject.color;
            newColor.a -= Time.deltaTime;
            parentObject.GetComponent<TextMeshProUGUI>().color = newColor;
            canvasRect.localPosition -= new Vector3(0f, Time.deltaTime * 40f, 0f);
            isRunning = true;
        }
        if(textObjectAlpha <= 0f)
        {
            DestroyParent();
        }
        yield return null;
    }
    void DestroyParent()
    {
        Destroy(transform.gameObject);
    }
    #endregion
}