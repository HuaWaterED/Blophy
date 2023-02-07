using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Blophy.Chart
{

    [Serializable]
    //public struct ChartData
    public class ChartData
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
    //public struct GlobalData
    public class GlobalData
    {
        public float offset;
        public float musicLength;
        public int tapCount;
        public int holdCount;
        public int dragCount;
        public int flickCount;
        public int fullFlickCount;
        public int pointCount;
    }
    [Serializable]
    public class Text
    {
        //未来要补文字演示的json
    }
    #region 下面都是依赖

    [Serializable]
    //public struct Box
    public class Box
    {
        public BoxEvents boxEvents;
        public Line[] lines;
    }
    [Serializable]
    //public struct Line
    public class Line
    {
        public Note[] onlineNotes;
        public Note[] offlineNotes;
        public Event[] speed;
        public AnimationCurve canvasLocalOffset;
    }
    [Serializable]
    //public struct Note
    public class Note
    {
        public NoteType noteType;
        public float hitTime;//打击时间
        public float holdTime;
        [JsonIgnore]
        public float HoldTime
        {
            get => holdTime == 0 ? JudgeManager.bad : holdTime;
            set => holdTime = value;
        }
        public BoxEffect boxEffect;
        public float positionX;
        public bool isClockwise;//是逆时针
        [JsonIgnore] public float EndTime => hitTime + HoldTime;
        [JsonIgnore] public float hitFloorPosition;//打击地板上距离
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
        None = 1,
        Ripple = 2,
        FullLine = 4,
        FullBox = 8
    }
    [Serializable]
    //public struct BoxEvents
    public class BoxEvents
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
    //public struct Event
    public class Event
    {
        public float startTime;
        public float endTime;
        public float startValue;
        public float endValue;
        public AnimationCurve curve;
    }
    #endregion
}