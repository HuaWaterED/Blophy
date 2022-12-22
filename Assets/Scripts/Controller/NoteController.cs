using Blophy.Chart;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public List<SpriteRenderers> spriteRenderers;
    public Note thisNote;//负责储存这个音符的一些数据
}
[Serializable]
public class SpriteRenderers
{
    public List<SpriteRenderer> spriteRenderers;
}