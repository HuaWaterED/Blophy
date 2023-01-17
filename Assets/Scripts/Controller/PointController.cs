using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : NoteController
{
    public float ariseTime;
    public Vector2[] origins;//起点
    public Vector2[] destinations;//终点
    [SerializeField] int journeyLength = -1;
    public int JourneyLength
    {
        get
        {
            if (journeyLength < 0)
                journeyLength = origins.Length;
            return journeyLength;
        }
    }
    public Transform[] move_edgeCorner;
    public SpriteRenderer[] allHorizontal;
    public SpriteRenderer[] allVertical;

    public override void Judge(double currentTime, TouchPhase touchPhase)
    {
        isJudged = true;//修改属性为成功判定
        base.Judge(currentTime, TouchPhase.Canceled);
    }
    public override void NoteHoldArise()
    {
        transform.localPosition = Vector2.up * PointNoteCurrentOffset;
        SetTextureLocalScale();
        float percent = ((float)ProgressManager.Instance.CurrentTime - ariseTime) / (thisNote.hitTime - ariseTime);
        if (percent > 1)
        {
            return;
        }
        for (int i = 0; i < JourneyLength; i++)
        {
            //move_edgeCorner[i].localPosition = (destinations[i] - origins[i]) * (1 - percent) + origins[i];//这是从中间向周围扩散
            move_edgeCorner[i].localPosition = (destinations[i] - origins[i]) * percent + origins[i];
        }
    }

    void SetTextureLocalScale()
    {
        foreach (SpriteRenderer item in allHorizontal)
        {
            item.transform.localScale = new Vector2(.5f + (ValueManager.Instance.boxFineness / decideLineController.box.currentScaleX), decideLineController.box.spriteRenderers[0].transform.localScale.y);
        }
        foreach (SpriteRenderer item in allVertical)
        {
            item.transform.localScale = new Vector2(.45f - (ValueManager.Instance.boxFineness / decideLineController.box.currentScaleY), decideLineController.box.spriteRenderers[2].transform.localScale.y);
        }
    }

    public override void Init()
    {
        for (int i = 0; i < Length_renderOrder; i++)//循环渲染层级的每一层
        {
            for (int j = 0; j < renderOrder[i].Length_spriteRenderers; j++)//循环每一层的所有素材
            {
                renderOrder[i].spriteRenderers[j].color = Color.black;//改为白色
            }
        }
        isJudged = false;//是否判定过设为假
        ariseTime = (float)ProgressManager.Instance.CurrentTime;
    }
}
