using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviourSingleton<ValueManager>//这里存放一些数值相关的东西
{

    [Tooltip("方框的精细程度")] public float boxFineness;//方框线的精细度
}
