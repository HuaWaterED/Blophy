using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : NoteController
{
    public override void NoteHoldArise()
    {
        transform.localPosition = Vector2.up * PointNoteCurrentOffset;
    }
}
