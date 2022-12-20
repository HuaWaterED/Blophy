using Blophy.Chart;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class LineNoteController : MonoBehaviour
{
    [Tooltip("如果是自由框，那就用这个作为音符的爸爸")] public Transform freeBox_NoteParsent;

    public DecideLineController decideLineController;//判定线控制

    public List<NoteController> ariseOnlineNotes = new();//判定线上方已经出现的音符列表
    public List<NoteController> ariseOfflineNotes = new();//判定线下方已经出现的音符列表

    public float CurrentAriseTime => 2 / decideLineController.canvasSpeed.Evaluate((float)ProgressManager.Instance.CurrentTime);//根据当前速度计算出按照当前速度走完两个单位长度需要多长时间

    public int lastOnlineIndex = 0;//上次召唤到Note[]列表的什么位置了，从上次的位置继续
    public int lastOfflineIndex = 0;//上次召唤到Note[]列表的什么位置了，从上次的位置继续
    private void Update()
    {
        if (gameObject.name != "Top") return;
        FindAndGetNote(decideLineController.ThisLine.onlineNotes, ref lastOnlineIndex, ariseOnlineNotes, true);//寻找这一时刻，在判定线上方需要生成的音符
        FindAndGetNote(decideLineController.ThisLine.offlineNotes, ref lastOfflineIndex, ariseOfflineNotes, false);//寻找这一时刻，在判定线下方需要生成的音符
        FindAndReturnNote(ariseOnlineNotes, true);//寻找这一时刻，在判定线上方需要回收的Miss掉的音符
        FindAndReturnNote(ariseOfflineNotes, false);//寻找这一时刻，在判定线下方需要回收的Miss掉的音符
    }
    /// <summary>
    /// 寻找这一时刻，在判定线需要生成的音符
    /// </summary>
    /// <param name="notes">音符列表</param>
    /// <param name="lastIndex">上次在什么地方结束，这次就从什么地方继续</param>
    /// <param name="arisedNote">生成后的音符存放点</param>
    /// <param name="isOnlineNote">当前处理的是不是判定线上方的音符，true代表是判定线上方的音符，false代表不是判定线上方的音符</param>
    private void FindAndGetNote(Note[] notes, ref int lastIndex, List<NoteController> arisedNote, bool isOnlineNote)
    {
        int direction = isOnlineNote switch//确定方向，如果是判定线上方，就是正值，如果是判定线下方，就是负值
        {
            true => 1,
            false => -1
        };
        int index = Algorithm.BinarySearch(notes, m => (float)ProgressManager.Instance.CurrentTime > m.hitTime - CurrentAriseTime, false);//寻找这个时刻需要出现的音符，出现要提前两个单位长度的时间出现
        for (int i = lastIndex; i < index; i++)//i从上次的地方继续，结束索引是寻找到的索引位置
        {//遍历所有符合要求的音符
            Note note = notes[i];//拿出当前遍历到的音符
            NoteController noteController = decideLineController.GetNote(note.noteType, isOnlineNote);//从对象池拿出来
            //noteController.transform.localPosition = note.hitFloorPosition * direction;
            noteController.transform.localPosition = new Vector2(note.positionX, note.hitFloorPosition * direction);//复制localPosition
            noteController.thisNote = note;//将这个音符的源数据赋值过去
            arisedNote.Add(noteController);//添加到音符存放点
        }
        lastIndex = index;//更新暂停位置
    }
    /// <summary>
    /// 寻找这一时刻，在判定线需要回收的Miss掉的音符
    /// </summary>
    /// <param name="notes">已经出现的音符列表存放点</param>
    /// <param name="isOnlineNote">是判定线上方还是下方</param>
    private void FindAndReturnNote(List<NoteController> notes, bool isOnlineNote)
    {
        int index = Algorithm.BinarySearch(notes, m => ProgressManager.Instance.CurrentTime >= m.thisNote.hitTime + JudgeManager.bad, false);//寻找已经出现的音符中有没有Miss掉的音符

        for (int i = 0; i < index; i++)//循环遍历所有Miss掉的音符
        {
            NoteController note = notes[i];//吧音符单独拿出来
            switch (isOnlineNote)
            {
                case true://如果是判定线上方
                    decideLineController.ReturnNote(note, note.thisNote.noteType, true);//那就返回到判定线上方对应的对象池中去
                    break;
                case false://如果是判定线下方
                    decideLineController.ReturnNote(note, note.thisNote.noteType, false);//那就返回到判定线下方对应的对象池中去
                    break;
            }
            notes.RemoveAt(i);//回收掉Miss音符后，从已出现的音符列表中移除
        }
    }
}
