using Blophy.Chart;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineNoteController : MonoBehaviour
{
    [Tooltip("如果是自由框，那就用这个作为音符的爸爸")] public Transform freeBox_NoteParsent;

    public DecideLineController decideLineController;

    public Transform onlineNote;//判定线上边的音符
    public Transform offlineNote;//判定线下边的音符


    public Line thisLine;

    public float lineDistance;//线的距离，根据每帧计算，生成这个范围内的所有Note
    public void Init(Line thisLine)
    {
        this.thisLine = thisLine;
        Init();
    }
    void Init()
    {
        CalculatedNoteFloorPosition(thisLine.onlineNotes);
        CalculatedNoteFloorPosition(thisLine.offlineNotes);
    }
    void CalculatedNoteFloorPosition(Note[] notes)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].floorPosition = decideLineController.canvasLocalOffset.Evaluate(thisLine.onlineNotes[i].hitTime);

            List<Keyframe> speedKeyframes = GameUtility.CalculatedSpeedCurve(notes[i].speed);
            notes[i].localVelocity = new() { keys = speedKeyframes.ToArray(), preWrapMode = WrapMode.ClampForever, postWrapMode = WrapMode.ClampForever };

            notes[i].localDisplacement = GameUtility.CalculatedOffsetCurve(notes[i].localVelocity, speedKeyframes);
        }
    }
    private void Update()
    {
        float currentValue = decideLineController.canvasLocalOffset.Evaluate((float)ProgressManager.Instance.CurrentTime);
        onlineNote.localPosition = Vector3.down * currentValue;
        offlineNote.localPosition = Vector3.up * currentValue;
    }
}
