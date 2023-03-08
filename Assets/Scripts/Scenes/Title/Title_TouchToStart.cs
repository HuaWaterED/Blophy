using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_TouchToStart : Public_Button
{
    private void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
        });
    }
}
