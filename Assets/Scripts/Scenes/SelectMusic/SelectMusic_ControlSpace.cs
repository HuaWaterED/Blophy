using Blophy.Chart;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SelectMusic_ControlSpace : Public_ControlSpace
{
    public static SelectMusic_ControlSpace Instance;
    private void Awake() => Instance = this;
    public string[] musics;
    public override void Send()
    {
        GlobalData.Instance.currentMusicIndex = currentElementIndex;
        GlobalData.Instance.currentMusic = musics[currentElementIndex];
        GlobalData.Instance.clip = Resources.Load<AudioClip>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.currentMusic}/Music/Music");
        string chart = Resources.Load<TextAsset>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.currentMusic}/ChartFile/{GlobalData.Instance.currentHard}/Chart").text;
        //GlobalData.Instance. = JsonConvert.DeserializeObject<ChartData>(chart);
        GlobalData.Instance.chartData = JsonConvert.DeserializeObject<ChartData>(chart);
    }
}
