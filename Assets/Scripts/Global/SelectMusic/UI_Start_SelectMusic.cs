using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Start_SelectMusic : MonoBehaviour
{
    public Button start_button;
    private void Start()
    {
        start_button.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Single);
        });
    }
}
