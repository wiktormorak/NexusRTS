using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [Header("Assignables")]
    public HostManager hostManager;
    public TMP_InputField joinCodeInputField;

    public void ButtonClient()
    {
        hostManager.StartClient(joinCodeInputField.text);
    }
}