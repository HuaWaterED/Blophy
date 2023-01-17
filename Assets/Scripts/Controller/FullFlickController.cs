using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullFlickController : NoteController
{
    public bool isMoved = false;
    public float judgedTime = 0;
    public override void Init()
    {
        base.Init();
        isJudged = false;
    }
    public override void Judge(double currentTime, TouchPhase touchPhase)
    {
        if (isJudged)
        {
            //判定成功
            isMoved = true;
            judgedTime = (float)currentTime;
        }
        if (!isJudged)
        {
            isJudged = true;
        }
    }
    public override void PassHitTime(double currentTime)
    {
        base.PassHitTime(currentTime);
        //这里放CurrentX，X的数据是-1-1之间的数据，理论上应该根据时间，计算出当前X
        float currentX = transform.localPosition.x;
        if (isJudged && isMoved)
        //if (true)
        {
            float percent = ((float)currentTime - thisNote.hitTime) / thisNote.HoldTime;
            currentX = (1 - thisNote.positionX) * percent + thisNote.positionX;
        }
        transform.localPosition = new Vector2(currentX, -noteCanvas.localPosition.y);//维持位置到“x和-y（本地坐标轴）”
    }
    public override void ReturnPool()
    {
        if (isJudged && isMoved)
        {
            base.Judge(ProgressManager.Instance.CurrentTime, TouchPhase.Canceled);
        }
    }
    public override bool IsinRange(Vector2 currentPosition)
    {
        float inThisLine = transform.InverseTransformPoint(currentPosition).x;//将手指的世界坐标转换为局部坐标后的x拿到

        if (inThisLine <= ValueManager.Instance.fullFlick_noteRightJudgeRange &&//如果x介于ValueManager设定的数值之间
            inThisLine >= ValueManager.Instance.fullFlick_noteLeftJudgeRange)
        {
            //UIManager.Instance.DebugTextString = $"onlineJudge:{onlineJudge}||offlineJudge:{offlineJudge}||Result:true ||inThisLine:{inThisLine}";
            return true;//返回是
        }
        else
        {
            //UIManager.Instance.DebugTextString = $"onlineJudge:{onlineJudge}||offlineJudge:{offlineJudge}||Result:false||inThisLine:{inThisLine}";
            return false;//返回否
        }
    }

}
