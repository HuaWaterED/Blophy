using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Blophy.Chart
{

    [Serializable]
    public struct ChartData
    {
        public MetaData metaData;
        public List<Box> boxes;
        public GlobalData globalData;
        public Text text;
    }

    [Serializable]
    public class MetaData
    {
        public string musicName = "";
        public string musicWriter = "";
        public string musicBPMText = "";
        public string artWriter = "";
        public string chartWriter = "";
        public string chartLevel = "";
        public string description = "";
    }
    [Serializable]
    public struct Box
    {
        public BoxType type;
        public BoxEvents boxEvents;
        public Line[] lines;
    }
    [Serializable]
    public struct GlobalData
    {
        public float offset;
        public float musicLength;
    }
    [Serializable]
    public class Text
    {
        //未来要补文字演示的json
    }
    #region 下面都是依赖

    [Serializable]
    public enum BoxType
    {
        free = 0,
        square = 1
    }

    [Serializable]
    public struct Line
    {
        public Note[] onlineNotes;
        public Note[] offlineNotes;
        public Event[] speed;
    }
    [Serializable]
    public struct Note
    {
        public NoteType noteType;
        public float hitTime;//打击时间
        public float holdTime;
        public BoxEffect boxEffect;
        public float positionX;
        public bool isClockwise;//是逆时针
        [JsonIgnore] public Event[] speed;
        [JsonIgnore] public float hitFloorPosition;//打击地板上距离
        [JsonIgnore] public float ariseFloorPosition;//从什么地方出现
        [JsonIgnore] public float ariseTime;//出现时间
        [JsonIgnore] public bool isArise;
        [JsonIgnore] public AnimationCurve localDisplacement;//这个用来表示的是某个时间，Note应该在画布的Y轴应该是多少
        [JsonIgnore] public AnimationCurve localVelocity;//这个用来表示这个Note的所有速度总览
    }
    [Serializable]
    public enum NoteType
    {
        Tap = 0,
        Hold = 1,
        Drag = 2,
        Flick = 3,
        Point = 4,
        FullFlickPink = 5,
        FullFlickBlue = 6
    }
    [Flags]
    [Serializable]
    public enum BoxEffect
    {
        None = 0,
        Ripple = 1,
        FullLine = 2,
        FullBox = 4
    }
    [Serializable]
    public struct BoxEvents
    {
        public Event[] moveX;
        public Event[] moveY;
        public Event[] rotate;
        public Event[] alpha;
        public Event[] scaleX;
        public Event[] scaleY;
        public Event[] centerX;
        public Event[] centerY;
        public Event[] lineAlpha;
    }
    [Serializable]
    public struct Event
    {
        public float startTime;
        public float endTime;
        public float startValue;
        public float endValue;
        public AnimationCurve curve;
    }
    #endregion
}