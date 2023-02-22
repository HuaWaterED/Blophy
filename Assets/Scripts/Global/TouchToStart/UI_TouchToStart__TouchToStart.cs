using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_TouchToStart__TouchToStart : MonoBehaviour
{
    public Button start_button;
    private void Start()
    {
        start_button.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("SelectChapter", LoadSceneMode.Single);
        });
    }
}
