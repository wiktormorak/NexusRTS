using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerMM : MonoBehaviour
{
    #region Settings
    public GameObject audioMenu;
    public GameObject graphicsMenu;
    #endregion
    #region Settings Menu - Graphics
    public void EnterGraphicsSettings()
    {
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(true);
    }
    #endregion
    #region Settings Menu - Audio
    public void EnterAudioSettings()
    {
        audioMenu.SetActive(true);
        graphicsMenu.SetActive(false);
    }
    #endregion
} 