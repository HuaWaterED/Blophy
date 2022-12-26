using Blophy.Chart;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public List<SpriteRenderers> spriteRenderers;
    public Note thisNote;//负责储存这个音符的一些数据
    public DecideLineController decideLineController;

    public bool isOnlineNote;

    public Transform noteCanvas;

    public float PointNoteCurrentOffset => 1 + decideLineController.canvasLocalOffset.Evaluate((float)ProgressManager.Instance.CurrentTime);//根据当前速度计算出按照当前的位移再+1就是现在的Point应该在的位置

    public virtual void Init() { }

    public void JudgeNote()
    {
        //这里放分数之类的

        //**************
        CompleteJudge();
    }
    /// <summary>
    /// 这里是被子类重写的方法，用于执行某些音符判定后特性的方法
    /// </summary>
    protected virtual void CompleteJudge() { }
}
[Serializable]
public class SpriteRenderers
{
    public List<SpriteRenderer> spriteRenderers;
}