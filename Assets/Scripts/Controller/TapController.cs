using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : NoteController
{
    /// <summary>
    /// 被成功判定后调用
    /// </summary>
    /// <param name="currentTime"></param>
    public override void Judge(double currentTime)
    {
        isJudged = true;//修改属性为成功判定
        HitEffectManager.Instance.PlayHitEffect(transform.position, transform.rotation, ValueManager.Instance.perfectJudge);//播放打击音
        switch (isOnlineNote)//看看自己试线上的音符还是线下的音符
        {
            case true://线上的音符的话就从两个线上排序中移除自己
                decideLineController.lineNoteController.ariseOnlineNotes.Remove(this);//hitTime排序中移除自己
                decideLineController.lineNoteController.endTime_ariseOnlineNotes.Remove(this);//endTime排序中移除自己
                break;
            case false://线下的音符的话就从两个线下排序中移除自己
                decideLineController.lineNoteController.ariseOfflineNotes.Remove(this);//hitTime排序中移除自己
                decideLineController.lineNoteController.endTime_ariseOfflineNotes.Remove(this);//endTime排序中移除自己
                break;
        }
        decideLineController.ReturnNote(this, thisNote.noteType, isOnlineNote);//把自己返回对象池
    }
}
