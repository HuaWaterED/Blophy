using Blophy.Chart;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineNoteController : MonoBehaviour
{
    [Tooltip("如果是自由框，那就用这个作为音符的爸爸")] public Transform freeBox_NoteParsent;

    public DecideLineController decideLineController;

    public List<NoteController> ariseOnlineNotes = new();
    public List<NoteController> ariseOfflineNotes = new();

    int arisedOnlineNotesIndex = 0;
    int arisedOfflineNotesIndex = 0;

    private void Update()
    {
        if (gameObject.name != "Top") return;
        FindAndGetLineNotes(decideLineController.ThisLine.onlineNotes, ariseOnlineNotes, ref arisedOnlineNotesIndex);
        FindAndGetLineNotes(decideLineController.ThisLine.offlineNotes, ariseOfflineNotes, ref arisedOfflineNotesIndex);
    }

    private void FindAndGetLineNotes(Note[] notes, List<NoteController> arisedNotes, ref int arisedNoteIndex)
    {
        int currentOfflineNotesIndex = Algorithm.BinarySearch<Note>(notes, m =>
     (float)ProgressManager.Instance.CurrentTime >= m.ariseTime);
        for (int i = arisedNoteIndex; i < currentOfflineNotesIndex; i++)
        {
            Note note = notes[i];
            NoteController noteController = decideLineController.GetNote(note.noteType, true);
            noteController.Init(note);
            arisedNotes.Add(noteController);
        }
        arisedNoteIndex = currentOfflineNotesIndex;
    }
}
