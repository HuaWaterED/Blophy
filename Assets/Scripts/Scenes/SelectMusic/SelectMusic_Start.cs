using Blophy.Chart;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMusic_Start : MonoBehaviour
{
    public Button start_button;
    private void Start()
    {
        start_button.onClick.AddListener(() => SceneManager.LoadSceneAsync("GamePlay", LoadSceneMode.Single).completed += (AsyncOperation obj) =>
        {
            WebManager.Instance.ChartData = GlobalData.Instance.chartData;
            WebManager.Instance.MusicClip = GlobalData.Instance.clip;
        });
    }
}
