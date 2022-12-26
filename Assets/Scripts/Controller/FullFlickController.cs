using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullFlickController : NoteController
{
    private void Update()
    {
        if (ProgressManager.Instance.CurrentTime >= thisNote.hitTime)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, -noteCanvas.localPosition.y);
        }
    }
}
