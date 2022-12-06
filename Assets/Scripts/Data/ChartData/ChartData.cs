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
        public List<Event> speed;
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
        public float hitTime;
        public float holdTime;
        public BoxEffect boxEffect;
        public float positionX;
        public Event[] speed;
        public bool isClockwise;//是逆时针
    }
    [Serializable]
    public enum NoteType
    {
        tap = 0,
        hold = 1,
        drag = 2,
        flick = 3,
        fullFlick = 4,
        point = 5
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