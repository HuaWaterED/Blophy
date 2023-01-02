using Blophy.Chart;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public List<SpriteRenderers> spriteRenderers;//渲染层级
    public Note thisNote;//负责储存这个音符的一些数据
    public DecideLineController decideLineController;//判定线引用

    public bool isOnlineNote;//是否事判定线上方的音符

    public Transform noteCanvas;//音符画布的引用

    public float PointNoteCurrentOffset => 1 + decideLineController.canvasLocalOffset.Evaluate((float)ProgressManager.Instance.CurrentTime);//根据当前速度计算出按照当前的位移再+1就是现在的Point应该在的位置

    public virtual void Init() { }//初始化方法
    /// <summary>
    /// 判定音符
    /// </summary>
    public void Judge(double currentTime)
    {
        //这里放分数之类的
        UIManager.Instance.DebugTextString = $"我是{thisNote.noteType},应该在{thisNote.hitTime}的时候被打击,传递给我的时间是{currentTime},我自行获取到的时间是{ProgressManager.Instance.CurrentTime}";
        Debug.Log($"我是{thisNote.noteType},应该在{thisNote.hitTime}的时候被打击,传递给我的时间是{currentTime},我自行获取到的时间是{ProgressManager.Instance.CurrentTime}");
        //**************
        CompleteJudge(currentTime);
    }
    /// <summary>
    /// 这里是被子类重写的方法，用于执行某些音符判定后特性的方法
    /// </summary>
    protected virtual void CompleteJudge(double currentTime) { }
    /// <summary>
    /// 音符出现的时候每帧调用
    /// </summary>
    public virtual void NoteHoldArise() { }
    public virtual bool IsinRange(Vector2 currentPosition)
    {
        //float onlineJudge = ValueManager.Instance.onlineJudgeRange;
        //float offlineJudge = ValueManager.Instance.offlineJudgeRange;

        float inThisLine = transform.InverseTransformPoint(currentPosition).x;

        if (inThisLine <= ValueManager.Instance.noteRightJudgeRange &&
            inThisLine >= ValueManager.Instance.noteLeftJudgeRange)
        {
            //UIManager.Instance.DebugTextString = $"onlineJudge:{onlineJudge}||offlineJudge:{offlineJudge}||Result:true ||inThisLine:{inThisLine}";
            return true;
        }
        else
        {
            //UIManager.Instance.DebugTextString = $"onlineJudge:{onlineJudge}||offlineJudge:{offlineJudge}||Result:false||inThisLine:{inThisLine}";
            return false;
        }
    }

}
[Serializable]
public class SpriteRenderers
{
    public List<SpriteRenderer> spriteRenderers;
}