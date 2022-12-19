using Blophy.Chart;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public Note note;
    public NoteController Init(Note note)
    {
        this.note = note;
        return this;
    }
}
