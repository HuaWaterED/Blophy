using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Camera;
public class SpeckleManager : MonoBehaviourSingleton<SpeckleManager>
{
    public Speckle[] speckles;
    protected override void OnAwake()
    {
        speckles = new Speckle[ValueManager.Instance.maxSpeckleCount];
        for (int i = 0; i < speckles.Length; i++)
        {
            speckles[i] = new Speckle(new Vector2[(int)(AssetManager.Instance.currentTargetFPS * ValueManager.Instance.fingerSavePosition)], Vector2.zero);
        }
    }
    private void Update()
    {
        UpdateTouch();
    }
    void UpdateTouch()
    {

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch currentTouch = Input.GetTouch(i);
            speckles[i].SetCurrentTouch(currentTouch, i);
        }
    }
}
[Serializable]
public struct Speckle//翻译为斑点，亦为安卓系统的触摸小白点，多个小白点酷似斑点，故起Speckle
{//一个斑点类就是一个手指的触摸
    [SerializeField] private float beganTime;//当前触摸段的开始时间
    public int thisIndex;
    public Vector2[] movePath;//移动过的路径
    public int index_movePath;//移动过的路径的索引
    public bool isFull_movePath;//移动过的路径是否已被全部填充
    public Vector2 startPoint;//开始位置
    [SerializeField] private TouchPhase phase;//当前的触摸阶段
    public TouchPhase Phase//当前触摸阶段
    {
        get => phase;//直接return~
        set//设置触摸阶段
        {
            phase = value;//先赋值
            switch (phase)
            {
                case TouchPhase.Began://如果触摸阶段是开始
                    beganTime = (float)ProgressManager.Instance.CurrentTime;//赋值当前时间进去
                    ResetIndex(ref index_movePath);//重置索引
                    startPoint = movePath[index_movePath++];//把第0个StartPoint赋值过去
                    isFull_movePath = false;//重置状态
                    break;
            }
        }
    }

    public Speckle(Vector2[] movePath, Vector2 startPoint,
        TouchPhase phase = TouchPhase.Canceled, float beganTime = 0, int thisIndex = 0, int index_movePath = 0, bool isFull_movePath = false)
    {
        this.beganTime = beganTime;
        this.thisIndex = thisIndex;
        this.index_movePath = index_movePath;
        this.isFull_movePath = isFull_movePath;
        this.phase = phase;
        this.movePath = movePath;
        this.startPoint = startPoint;
    }
    public float FlickDirection//滑动方向
    {
        get
        {
            //如果isFull MovePath是true，就直接用最后一个二维向量减去第一个，如果是False，就用当前的movepath列表返回出去
            float v = Vector2.SignedAngle(movePath[isFull_movePath ? ^1 : index_movePath] - movePath[0], Vector2.right);
            return v < 0 ? -v : 360 - v;//如果是负数就取赋值，如果是正数直接360-当前值返回出去
        }
    }
    public float FlickLength => (movePath[isFull_movePath ? ^1 : index_movePath] - movePath[0]).magnitude;//滑动长度
    public void SetCurrentTouch(Touch touch, int currentIndex)//当前触摸处理的初始化方法
    {
        thisIndex = currentIndex;
        movePath[index_movePath++] = main.ScreenToWorldPoint(touch.position);
        Phase = touch.phase switch
        {
            TouchPhase.Began => TouchPhase.Began,
            TouchPhase.Canceled => TouchPhase.Ended,
            TouchPhase.Ended => TouchPhase.Ended,
            _ => IsMovedOrStationary(ValueManager.Instance.flickRange)
        };
        CheckIndex(ref index_movePath, movePath.Length);
    }
    TouchPhase IsMovedOrStationary(float deltaRange)
    {
        Vector2 currentPosition = movePath[LoopBackIndex(index_movePath, movePath.Length, -1)];
        Vector2 lastPosition = movePath[LoopBackIndex(index_movePath, movePath.Length, -2)];
        float deltaLength = (currentPosition - lastPosition).magnitude;
        UIManager.Instance.DebugTextString = $"FingerIndex：{thisIndex}\n" +
            $"FlickLength:{deltaLength}\n" +
            $"TouchPhase:{phase}\n" +
            $"CurrentTargetFPS:{AssetManager.Instance.currentTargetFPS}";
        return deltaLength < deltaRange ? TouchPhase.Stationary : TouchPhase.Moved;
    }
    int LoopBackIndex(int currentIndex, int maxIndex, int needCalculateIndex)
    {
        int indexDelta = currentIndex + needCalculateIndex;
        int result;
        if (indexDelta < 0)
        {
            result = maxIndex + indexDelta;
        }
        else if (indexDelta >= maxIndex)
        {
            result = indexDelta - maxIndex;
        }
        else
        {
            result = indexDelta;
        }
        return result;
    }
    void CheckIndex(ref int index, int maxValue)
    {
        if (index >= maxValue)
            ResetIndex(ref index);
    }
    void ResetIndex(ref int index) => index = 0;

}
