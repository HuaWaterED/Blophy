using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings_Return : Public_Button
{
    // Start is called before the first frame update
    void Start()
    {
        Loading_Controller.Instance.SetLoadSceneByName("Main");
        thisButton.onClick.AddListener(() => Loading_Controller.Instance.StartLoad());
    }
}
