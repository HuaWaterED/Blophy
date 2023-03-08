using Blophy.Chart;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectMusic_ControlSpace : Public_ControlSpace
{
    public static SelectMusic_ControlSpace Instance;
    private void Awake() => Instance = this;
    public string[] musics;
    public Image musicPrefab;
    public override void Send()
    {
        GlobalData.Instance.currentMusicIndex = currentElementIndex;
        GlobalData.Instance.currentMusic = musics[currentElementIndex];
        GlobalData.Instance.currentCP = Resources.Load<Sprite>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.currentMusic}/Background/CP");
        GlobalData.Instance.currentCPH = Resources.Load<Sprite>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.currentMusic}/Background/CPH");
        GlobalData.Instance.clip = Resources.Load<AudioClip>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.currentMusic}/Music/Music");
        string rawChart = Resources.Load<TextAsset>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.currentMusic}/ChartFile/{GlobalData.Instance.currentHard}/Chart").text;
        //GlobalData.Instance. = JsonConvert.DeserializeObject<ChartData>(chart);
        ChartData chart = JsonConvert.DeserializeObject<ChartData>(rawChart);
        GlobalData.Instance.chartData = chart;
        SelectMusic_UIManager.Instance.SelectMusic(chart.metaData.musicName, chart.metaData.musicWriter, chart.metaData.chartWriter, chart.metaData.artWriter);
    }
    protected override void OnStart()
    {
        musics = GlobalData.Instance.chapters[GlobalData.Instance.currentChapterIndex].musicPath;
        int currentChapterMusicCount = GlobalData.Instance.chapters[GlobalData.Instance.currentChapterIndex].musicPath.Length;
        elementCount = currentChapterMusicCount;
        currentElement = 1;
        for (int i = 0; i < currentChapterMusicCount; i++)
        {
            Instantiate(musicPrefab, transform).sprite = Resources.Load<Sprite>($"MusicPack/{GlobalData.Instance.currentChapter}/{GlobalData.Instance.chapters[GlobalData.Instance.currentChapterIndex].musicPath[i]}/Background/CPH");
        }
    }

    Vector2 startPoint;
    Vector2 endPoint;
    protected override void LargeImageUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPoint = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                endPoint = touch.position;
                float deltaY = (endPoint - startPoint).y;
                if (deltaY > 300 && currentElementIndex + 1 < elementCount)
                {
                    currentElement = allElementDistance[elementCount - 1 - ++currentElementIndex];
                }
                else if (deltaY < -300 && currentElementIndex - 1 >= 0)
                {
                    currentElement = allElementDistance[elementCount - 1 - --currentElementIndex];
                }
                Send();
                StartCoroutine(Lerp());
            }
        }
    }
}
