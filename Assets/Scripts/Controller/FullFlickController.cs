using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullFlickController : NoteController
{
    public Transform textureBoss;//就是FullFlick音符的两个渲染贴图（Texture）的爸爸（
    public bool isMoved = false;//是否已经移动了
    int decisionEndPoint;
    public override void Init()
    {
        base.Init();
        isJudged = false;//重置isJudged
        switch (thisNote.isClockwise)
        {
            case true:
                decisionEndPoint = -1;
                textureBoss.localRotation = Quaternion.Euler(Vector3.forward * 180);
                break;
            case false:
                decisionEndPoint = 1;
                textureBoss.localRotation = Quaternion.identity;
                break;
        }
    }
    public override void Judge(double currentTime, TouchPhase touchPhase)
    {
        if (!isJudged && touchPhase == TouchPhase.Began)
        {
            isJudged = true;
        }
        else if (isJudged && touchPhase == TouchPhase.Moved)
        {
            isMoved = true;
        }
    }
    public override void PassHitTime(double currentTime)
    {
        base.PassHitTime(currentTime);//执行基类的方法
        //这里放CurrentX，X的数据是-1-1之间的数据，理论上应该根据时间，计算出当前X
        float currentX = transform.localPosition.x;//默认赋值当前的LocalPosition.X
        if (isJudged && isMoved)//如果判定成功
        //if (true)
        {
            float percent = ((float)currentTime - thisNote.hitTime) / thisNote.HoldTime;//计算当前时间距离开始和结束过去了百分之多少
            currentX = (decisionEndPoint - thisNote.positionX) * percent + thisNote.positionX;//赋值计算得到的值，1是方框最右边，因为方框最左边是-1，左右边是1，中间是0
        }
        transform.localPosition = new Vector2(currentX, -noteCanvas.localPosition.y);//维持位置到“x和-y（本地坐标轴）”
    }
    public override void ReturnPool()
    {
        if (isJudged && isMoved)//如果判定成功
        {
            PlayHitEffectWithJudgeLevel(NoteJudge.Perfect, ValueManager.Instance.perfectJudge);
            ScoreManager.Instance.AddScore(thisNote.noteType, NoteJudge.Perfect, true);
            return;
        }
        ScoreManager.Instance.AddScore(thisNote.noteType, NoteJudge.Miss, true);
    }
    public override bool IsinRange(Vector2 currentPosition)
    {
        float inThisLine = noteCanvas.InverseTransformPoint(currentPosition).x;//将手指的世界坐标转换为局部坐标后的x拿到

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
