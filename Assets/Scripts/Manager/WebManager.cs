using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blophy.Chart;
using UnityEngine.UI;

public class WebManager : MonoBehaviourSingleton<WebManager>
{
    public ChartData ChartData
    {
        get => AssetManager.Instance.chartData;
        set
        {
            AssetManager.Instance.chartData = value;
            //ScoreManager.Instance.NoteCount
            GlobalData.Instance.score.Reset();
            GlobalData.Instance.score.tapCount = value.globalData.tapCount;
            GlobalData.Instance.score.holdCount = value.globalData.holdCount;
            GlobalData.Instance.score.dragCount = value.globalData.dragCount;
            GlobalData.Instance.score.flickCount = value.globalData.flickCount;
            GlobalData.Instance.score.fullFlickCount = value.globalData.fullFlickCount;
            GlobalData.Instance.score.pointCount = value.globalData.pointCount;
        }

    }
    public AudioClip MusicClip
    {
        get => AssetManager.Instance.musicPlayer.clip;
        set => AssetManager.Instance.musicPlayer.clip = value;
    }
    public Image Background
    {
        get => AssetManager.Instance.background;
        set => AssetManager.Instance.background = value;
    }
}
