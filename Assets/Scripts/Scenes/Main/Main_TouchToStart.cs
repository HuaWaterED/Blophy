using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_TouchToStart : Public_Button
{
    public Button[] allOptions;
    private void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            for (int i = 0; i < allOptions.Length; i++)
            {
                allOptions[i].gameObject.SetActive(true);
            }
            gameObject.SetActive(false);
        });
    }
}
