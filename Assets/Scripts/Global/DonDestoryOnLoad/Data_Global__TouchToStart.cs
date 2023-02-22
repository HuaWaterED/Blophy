using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Global__TouchToStart : MonoBehaviourSingleton<Data_Global__TouchToStart>
{
    public int SelectChapter_CurrentChapter = 0;
    protected override void OnAwake()
    {
        DontDestroyOnLoad(this);
    }
}
