using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldController : NoteController
{
    public Transform holdBody;
    public Transform holdHead;

    public float remainTime;//停留时间
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        AnimationCurve localOffset = decideLineController.canvasLocalOffset;//拿到位移图的索引
        holdBody.transform.localScale = //设置缩放
            new Vector2(1, localOffset.Evaluate(thisNote.hitTime + thisNote.HoldTime) - localOffset.Evaluate(thisNote.hitTime));//x轴默认为1，y轴为位移图上的距离
        base.Init();
    }
    public override void NoteHoldArise()
    {
        //这里放“现在是通过遮罩做的，我想的是，未来可不可以去掉遮罩，做成上下自动检测拥有停留时间”中的内容
    }
    public override void PassHitTime(double currentTime)
    {
        AnimationCurve localOffset = decideLineController.canvasLocalOffset;//拿到位移图的索引
        transform.localPosition = new Vector2(transform.localPosition.x, -noteCanvas.localPosition.y);//将位置保留到判定线的位置
        holdBody.transform.localScale = new Vector2(1, localOffset.Evaluate(thisNote.hitTime + thisNote.holdTime) - localOffset.Evaluate((float)currentTime));
    }
}
