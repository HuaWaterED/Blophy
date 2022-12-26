using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviourSingleton<FPSManager>
{
    public int FPSvalue = 521;
    protected override void OnAwake()
    {
        Application.targetFrameRate = FPSvalue;
    }
}
