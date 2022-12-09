using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviourSingleton<ValueManager>
{
    [Tooltip("曲线加速度计算精度")]
    public int easeAcceleratedCalculatedFineness;

    [Tooltip("方框的精细程度")] public float boxFineness;//方框线的精细度
}
