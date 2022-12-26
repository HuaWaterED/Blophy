using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldController : NoteController
{
    public Transform holdBody;
    public Transform holdHead;

    public float remainTime;
    public override void Init()
    {
        AnimationCurve localOffset = decideLineController.canvasLocalOffset;
        holdBody.transform.localScale = new Vector2(1, localOffset.Evaluate(thisNote.hitTime + thisNote.holdTime) - localOffset.Evaluate(thisNote.hitTime));
    }
    private void Update()
    {
        AnimationCurve localOffset = decideLineController.canvasLocalOffset;
        if (ProgressManager.Instance.CurrentTime >= thisNote.hitTime)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, -noteCanvas.localPosition.y);
            holdBody.transform.localScale = new Vector2(1, localOffset.Evaluate(thisNote.hitTime + thisNote.holdTime) - localOffset.Evaluate((float)ProgressManager.Instance.CurrentTime));
        }
        if (ProgressManager.Instance.CurrentTime >= thisNote.hitTime + thisNote.holdTime)
        {
            decideLineController.ReturnNote(this, Blophy.Chart.NoteType.Hold, isOnlineNote);
        }
    }
}
