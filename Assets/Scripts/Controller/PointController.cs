using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : NoteController
{
    private void Update()
    {
        transform.localPosition = Vector2.up * PointNoteCurrentOffset;
    }
}
