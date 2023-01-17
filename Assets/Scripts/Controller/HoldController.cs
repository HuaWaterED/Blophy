using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldController : NoteController
{
    public Transform holdBody;
    public Transform holdHead;

    public float remainTime;//停留时间

    public bool reJudge = false;
    public bool isMissed = false;
    public float reJudgeTime = 0;
    public float checkTime = -.1f;
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        AnimationCurve localOffset = decideLineController.canvasLocalOffset;//拿到位移图的索引
        holdBody.transform.localScale = //设置缩放
            new Vector2(1, localOffset.Evaluate(thisNote.hitTime + thisNote.HoldTime) - localOffset.Evaluate(thisNote.hitTime));//x轴默认为1，y轴为位移图上的距离
        isMissed = false;
        reJudge = false;
        checkTime = -.1f;
        reJudgeTime = 0;
        base.Init();
    }
    public override void Judge(double currentTime, TouchPhase touchPhase)
    {
        if (!isJudged && touchPhase == TouchPhase.Began)
        {
            isJudged = true;
            checkTime = Time.time;
            HitEffectManager.Instance.PlayHitEffect(transform.position, transform.rotation, ValueManager.Instance.perfectJudge);//播放打击特效
        }
        switch (touchPhase)
        {
            case TouchPhase.Began:
                break;
            default:
                checkTime = Time.time;
                reJudgeTime += Time.deltaTime;
                if (reJudgeTime >= ValueManager.Instance.holdLeaveScreenTime)
                {
                    reJudgeTime = 0;
                    reJudge = true;
                }
                break;
        }
    }
    public void HoldMiss()
    {
        ChangeColor(new Color(1, 1, 1, .3f));
    }
    public override void NoteHoldArise()
    {
        //这里放“现在是通过遮罩做的，我想的是，未来可不可以去掉遮罩，做成上下自动检测拥有停留时间”中的内容
        //***************************************************************************************

        if (checkTime > 0 && isJudged)
        {
            if (Time.time - checkTime > ValueManager.Instance.holdLeaveScreenTime && !isMissed)
            {
                isMissed = true;
                HoldMiss();
            }
            else if (Time.time - checkTime <= ValueManager.Instance.holdLeaveScreenTime && !isMissed && reJudge)
            {
                //checkTime = Time.time;
                //没有Miss
                //打击特效
                reJudge = false;
                HitEffectManager.Instance.PlayHitEffect(transform.position, transform.rotation, ValueManager.Instance.perfectJudge);//播放打击特效
            }
        }
        if (ProgressManager.Instance.CurrentTime >= thisNote.hitTime + JudgeManager.bad && !isJudged)
        {
            HoldMiss();
        }
    }

    public override void PassHitTime(double currentTime)
    {
        AnimationCurve localOffset = decideLineController.canvasLocalOffset;//拿到位移图的索引
        transform.localPosition = new Vector2(transform.localPosition.x, -noteCanvas.localPosition.y);//将位置保留到判定线的位置
        holdBody.transform.localScale = new Vector2(1, localOffset.Evaluate(thisNote.hitTime + thisNote.holdTime) - localOffset.Evaluate((float)currentTime));
    }
}
