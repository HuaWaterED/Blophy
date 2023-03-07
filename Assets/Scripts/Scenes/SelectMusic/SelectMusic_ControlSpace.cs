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
