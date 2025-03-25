using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndHandler : MonoBehaviour
{
    #region External Vars
    public GameObject tileManager;
    public TileController tileController;
    #endregion
    #region Unity Funcs
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    void GetExternalVars()
    {
        tileController = tileManager.GetComponent<TileController>();
    }
}