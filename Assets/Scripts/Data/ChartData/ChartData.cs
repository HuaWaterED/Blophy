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
        public Box[] boxes;
        public GlobalData globalData;
        public Text[] texts;
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
        public float startTime;
        public float endTime;
        public float size;
        public string text;
        public Event moveX;
        public Event moveY;
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
        public int onlineNotesLength = -1;
        public int OnlineNotesLength
        {
            get
            {
                if (onlineNotesLength < 0) onlineNotesLength = onlineNotes.Length;
                return onlineNotesLength;
            }
        }
        public Note[] offlineNotes;
        public int offlineNotesLength = -1;
        public int OfflineNotesLength
        {
            get
            {
                if (offlineNotesLength < 0) offlineNotesLength = offlineNotes.Length;
                return offlineNotesLength;
            }
        }
        public Event[] speed;
        public AnimationCurve far;//画布偏移绝对位置，距离
        public AnimationCurve career;//速度
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
        public NoteEffect effect;
        public float positionX;
        public bool isClockwise;//是逆时针
        public bool hasOther;//还有别的Note和他在统一时间被打击，简称多押标识（（
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
    public enum NoteEffect
    {
        Ripple = 1,
        FullLine = 2,
        CommonEffect = 4
    }
    [Serializable]
    //public struct BoxEvents
    public class BoxEvents
    {
        public Event[] moveX;
        public int length_moveX = -1;
        public int Length_moveX
        {
            get
            {
                if (length_moveX < 0) length_moveX = moveX.Length;
                return length_moveX;
            }
        }
        public Event[] moveY;
        public int length_moveY = -1;
        public int Length_moveY
        {
            get
            {
                if (length_moveY < 0) length_moveY = moveY.Length;
                return length_moveY;
            }
        }
        public Event[] rotate;
        public int length_rotate = -1;
        public int Length_rotate
        {
            get
            {
                if (length_rotate < 0) length_rotate = rotate.Length;
                return length_rotate;
            }
        }
        public Event[] alpha;
        public int length_alpha = -1;
        public int Length_alpha
        {
            get
            {
                if (length_alpha < 0) length_alpha = alpha.Length;
                return length_alpha;
            }
        }
        public Event[] scaleX;
        public int length_scaleX = -1;
        public int Length_scaleX
        {
            get
            {
                if (length_scaleX < 0) length_scaleX = scaleX.Length;
                return length_scaleX;
            }
        }
        public Event[] scaleY;
        public int length_scaleY = -1;
        public int Length_scaleY
        {
            get
            {
                if (length_scaleY < 0) length_scaleY = scaleY.Length;
                return length_scaleY;
            }
        }
        public Event[] centerX;
        public int length_centerX = -1;
        public int Length_centerX
        {
            get
            {
                if (length_centerX < 0) length_centerX = centerX.Length;
                return length_centerX;
            }
        }
        public Event[] centerY;
        public int length_centerY = -1;
        public int Length_centerY
        {
            get
            {
                if (length_centerY < 0) length_centerY = centerY.Length;
                return length_centerY;
            }
        }
        public Event[] lineAlpha;
        public int length_lineAlpha = -1;
        public int Length_lineAlpha
        {
            get
            {
                if (length_lineAlpha < 0) length_lineAlpha = lineAlpha.Length;
                return length_lineAlpha;
            }
        }
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