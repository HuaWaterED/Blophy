using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : NoteController
{
    public override void NoteHoldArise()
    {
        transform.localPosition = Vector2.up * PointNoteCurrentOffset;
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
    }
}
