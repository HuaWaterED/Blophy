using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Camera;
public class GlobalData : MonoBehaviourSingleton<GlobalData>
{
    public Chapter[] chapters;
    public string currentChapter;
    public int currentChapterIndex;
    public string currentMusic;
    public int currentMusicIndex;
    public string currentHard;
    public ChartData chartData;
    public AudioClip clip;
    public int ScreenWidth => main.pixelWidth;
    public int ScreenHeight => main.pixelHeight;
    protected override void OnAwake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        Application.targetFrameRate = 9999;
    }
}
[Serializable]
public class Chapter
{
    public string chapterName;
    public string[] musicPath;
}
