using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullFlickController : NoteController
{
    public override void PassHitTime(double currentTime)
    {
        base.PassHitTime(currentTime);
        transform.localPosition = new Vector2(transform.localPosition.x, -noteCanvas.localPosition.y);//维持位置到“x和-y（本地坐标轴）”
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
