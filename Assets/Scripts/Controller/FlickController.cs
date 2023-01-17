using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickController : NoteController
{
    public override void Judge(double currentTime, TouchPhase touchPhase)
    {
        isJudged = true;
    }
    public override void PassHitTime(double currentTime)
    {
        base.PassHitTime(currentTime);
        if (isJudged)
        {
            base.Judge(currentTime, TouchPhase.Canceled);
        }
    }
}
