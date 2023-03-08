using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Next : Public_Button
{
    private void Start()
    {
        thisButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("SelectMusic", LoadSceneMode.Single));
    }
}
