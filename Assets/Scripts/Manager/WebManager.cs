using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blophy.Chart;
public class WebManager : MonoBehaviourSingleton<WebManager>
{
    public ChartData ChartData
    {
        get => AssetManager.Instance.chartData;
        set
        {
            AssetManager.Instance.chartData = value;
            //ScoreManager.Instance.NoteCount
            ScoreManager.Instance.tapCount = value.globalData.tapCount;
            ScoreManager.Instance.holdCount = value.globalData.holdCount;
            ScoreManager.Instance.dragCount = value.globalData.dragCount;
            ScoreManager.Instance.flickCount = value.globalData.flickCount;
            ScoreManager.Instance.fullFlickCount = value.globalData.fullFlickCount;
            ScoreManager.Instance.pointCount = value.globalData.pointCount;
        }

    }
    public AudioClip MusicClip
    {
        get => AssetManager.Instance.musicPlayer.clip;
        set => AssetManager.Instance.musicPlayer.clip = value;
    }
}
