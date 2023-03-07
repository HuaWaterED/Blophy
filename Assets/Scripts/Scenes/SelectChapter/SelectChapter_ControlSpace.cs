using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChapter_ControlSpace : Public_ControlSpace
{

    public override void Send()
    {
        GlobalData.Instance.currentChapterIndex = currentElementIndex;
        GlobalData.Instance.currentChapter = GlobalData.Instance.chapters[currentElementIndex].chapterName;
    }
}
