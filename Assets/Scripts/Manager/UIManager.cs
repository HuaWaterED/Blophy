using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public TextMeshProUGUI debugText;
    string debugTextString = "";
    public string DebugTextString
    {
        get => debugTextString;
        set
        {
            debugTextString += $"{value}\n";
            debugText.text = debugTextString;
        }
    }
    private void Update()
    {
        debugTextString = "";
    }
}
