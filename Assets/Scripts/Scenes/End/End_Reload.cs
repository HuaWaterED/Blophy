using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_Reload : Public_Button
{
    private void Start()
    {
        thisButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Single).completed += (AsyncOperation obj) =>
        {
            WebManager.Instance.ChartData = GlobalData.Instance.chartData;
            WebManager.Instance.MusicClip = GlobalData.Instance.clip;
            WebManager.Instance.Background.sprite = GlobalData.Instance.currentCP;
        });
    }
}
