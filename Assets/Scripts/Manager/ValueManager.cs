using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviourSingleton<ValueManager>//这里存放一些数值相关的东西
{
    [Tooltip("计算面积的精细度")] public float calculatedAreaRange;
    [Tooltip("保留多少位，和上边的有强关联，如0.1就保留1位，0.01就2位，以此类推")] public int reservedBits;

    [Tooltip("方框的精细程度")] public float boxFineness;//方框线的精细度

    [Tooltip("每帧划多少算移动中")] public float flickRange;

    [Tooltip("最多处理多少个手指")] public int maxSpeckleCount;

    [Tooltip("手指按下后我要保存多少秒的位置")] public float fingerSavePosition;

    [Tooltip("音符渲染小层，每一大层有多少小层")] public int noteRendererOrder;

    [Tooltip("因为编辑器检测屏幕刷新了始终是0，所以这里手动设置编辑器目标FPS")] public int editorTargetFPS;

    [Tooltip("Runtime目标FPS")] public int FPS;

    protected override void OnAwake() => Application.targetFrameRate = FPS;
}
