using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMusic_Return : Public_Button
{
    // Start is called before the first frame update
    void Start()
    {
        thisButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("SelectChapter", LoadSceneMode.Single));
    }
}
