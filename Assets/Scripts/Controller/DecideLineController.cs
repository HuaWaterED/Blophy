using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Blophy.Extension;
using UnityEngine.Pool;
using System.Drawing.Printing;
/*
* 声明：
* StartTime简称ST
* EndTime简称ET
* StartValue简称SV
* EndValue简称EV
* 
* 百分比解释：
* 因为Unity的AnimationCurve的Key的InWeight和OutWeight的表示
* InWeight就是这个点距离上一个点为单位，自身为0，到上一个点的距离为1的表示法
* OutWeight就是这个点距离下一个点为单位，自身为0，到下一个点的距离为1的表示法
* 这里说为百分比是觉得百分比也是0-1之间的标准数值
*/
public class DecideLineController : MonoBehaviour
{
    public float lineDistance;//线的距离，根据每帧计算，生成这个范围内的所有Note

    public AnimationCurve canvasLocalOffset;//这个用来表示的是某个时间，画布的Y轴应该是多少
    public AnimationCurve canvasSpeed;//这个用来表示这根线的所有速度总览
    public Transform onlineNote;//判定线上边的音符
    public Transform offlineNote;//判定线下边的音符

    public LineNoteController lineNoteController;

    public List<ObjectPoolQueue<NoteController>> onlineNotes;
    public List<ObjectPoolQueue<NoteController>> offlineNotes;

    public Line thisLine;
    public Line ThisLine
    {
        get => thisLine;
        set
        {
            thisLine = value;
            Init();
        }
    }//这跟线的谱面元数据

    /// <summary>
    /// 初始化方法
    /// </summary>
    void Init()
    {
        InitNotesObjectPool();
        Debug.LogWarning("请注意我的袁术局的隐私性");
        List<Keyframe> keyframes = GameUtility.CalculatedSpeedCurve(ThisLine.speed);//将获得到的Key列表全部赋值
        canvasSpeed = new() { keys = keyframes.ToArray(), preWrapMode = WrapMode.ClampForever, postWrapMode = WrapMode.ClampForever };//把上边获得到的点转换为速度图
        canvasLocalOffset = GameUtility.CalculatedOffsetCurve(canvasSpeed, keyframes);//吧速度图转换为位移图
        CalculatedNoteFloorPosition(ThisLine.onlineNotes);
        CalculatedNoteFloorPosition(ThisLine.offlineNotes);
    }

    private void InitNotesObjectPool()
    {
        onlineNotes = new()
        {
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Tap],5,onlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Hold],2,onlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Drag],5,onlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Flick],0, onlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Point],2, onlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.FullFlickPink],2, onlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.FullFlickBlue],2, onlineNote)
        };
        offlineNotes = new()
        {
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Tap],5,offlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Hold],2,offlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Drag],5,offlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Flick],0, offlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.Point],2, offlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.FullFlickPink],2, offlineNote),
            new ObjectPoolQueue<NoteController>(AssetManager.Instance.noteControllers[(int)NoteType.FullFlickBlue],2, offlineNote)
        };
    }
    public NoteController GetNote(NoteType noteType, bool isOnlineNote)
    {
        return isOnlineNote switch
        {
            true => onlineNotes[(int)noteType].PrepareObject(),
            false => offlineNotes[(int)noteType].PrepareObject(),
        };
    }
    public void ReturnNote(NoteController note, NoteType noteType, bool isOnlineNote)
    {
        switch (isOnlineNote)
        {
            case true:
                onlineNotes[(int)noteType].ReturnObject(note);
                break;
            case false:
                offlineNotes[(int)noteType].ReturnObject(note);
                break;
        }
    }

    void CalculatedNoteFloorPosition(Note[] notes)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].hitFloorPosition = (float)Math.Round(canvasLocalOffset.Evaluate(ThisLine.onlineNotes[i].hitTime), 3);//根据打击时间获取到打击距离

            List<Keyframe> speedKeyframes = GameUtility.CalculatedSpeedCurve(notes[i].speed);//获得到速度图的Key列表
            notes[i].localVelocity = new() { keys = speedKeyframes.ToArray(), preWrapMode = WrapMode.ClampForever, postWrapMode = WrapMode.ClampForever };//生成速度图

            notes[i].localDisplacement = GameUtility.CalculatedOffsetCurve(notes[i].localVelocity, speedKeyframes);//根据速度图生成位移图

            notes[i].ariseFloorPosition = (float)Math.Round(notes[i].hitFloorPosition + notes[i].localDisplacement.keys[^1].value, 3);//计算出现距离

            notes[i].ariseTime = notes[i].hitTime + notes[i].localDisplacement.keys[^1].time;//计算出现时间
        }
    }
    private void Update()
    {
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        float currentValue = canvasLocalOffset.Evaluate((float)ProgressManager.Instance.CurrentTime);
        onlineNote.localPosition = Vector3.down * currentValue;
        offlineNote.localPosition = Vector3.up * currentValue;
    }
}