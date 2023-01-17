using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : NoteController
{

    public override void Judge(double currentTime, TouchPhase touchPhase)
    {
        isJudged = true;//设置状态
    }
    public override void PassHitTime(double currentTime)
    {
        base.PassHitTime(currentTime);//执行基类的方法
        if (isJudged)//如果判定成功
        {
            base.Judge(currentTime, TouchPhase.Canceled);//执行判定，因为基类的判定没有用到TouchPhase，所以这里就随便用一个了
        }
    }
}
