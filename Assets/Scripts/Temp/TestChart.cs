using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blophy.Chart;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Newtonsoft.Json;
public class TestChart : MonoBehaviourSingleton<TestChart>
{
    public ChartData chartData;
    public AudioClip clip;

    private void Start()
    {
        ChartData c = JsonConvert.DeserializeObject<ChartData>(Resources.Load<TextAsset>("MusicPack/Chapter_I/FruitySpace/ChartFile/Chart").text);
        AssetManager.Instance.chartData = c;
        AssetManager.Instance.musicPlayer.clip = clip;
        Debug.Log(JsonConvert.SerializeObject(chartData));
    }
    private void Update()
    {
    }
}
