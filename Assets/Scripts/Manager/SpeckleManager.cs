using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.Camera;
public class SpeckleManager : MonoBehaviourSingleton<SpeckleManager>
{
    public Speckle[] speckles;//手指触摸列表

    public List<LineNoteController> allLineNoteControllers = new();//所有的判定线，一个框默认五个判定线
    public List<LineNoteController> isInRangeLine = new();
    public List<NoteController> waitNote = new();
    public int lineNoteControllerCount = -1;//初始化为-1
    public int LineNoteControllerCount
    {
        get
        {
            if (lineNoteControllerCount < 0)//如果小于0，说明没有执行过这里的代码
                lineNoteControllerCount = allLineNoteControllers.Count;//就获取一次
            return lineNoteControllerCount;//返回获取到的数据
        }
    }
    protected override void OnAwake()//唤醒
    {
        speckles = new Speckle[ValueManager.Instance.maxSpeckleCount];//初始化手指触摸列表
        for (int i = 0; i < speckles.Length; i++)//循环每个列表
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

        for (int i = 0; i < Input.touchCount; i++)//遍历所有手指
        {
            Touch currentTouch = Input.GetTouch(i);
            speckles[i].SetCurrentTouch(currentTouch, i);
            isInRangeLine.Clear();
            waitNote.Clear();
            //思路：先判定判定线的横轴，在那个判定线范围区间内
            //然后时间判定，找到时间值最低的Notes
            //然后根据Notes做竖轴空间判定
            for (int j = 0; j < LineNoteControllerCount; j++)//每根手指遍历每根线,在哪些线的横轴范围内
            {
                if (allLineNoteControllers[j].SpeckleInThisLine(speckles[i].CurrentPosition))
                    isInRangeLine.Add(allLineNoteControllers[j]);
            }
            for (int j = 0; j < isInRangeLine.Count; j++)//时间判定
            {
                FindNotes(isInRangeLine[j].ariseOnlineNotes, waitNote);
                FindNotes(isInRangeLine[j].ariseOfflineNotes, waitNote);
            }
            for (int j = 0; j < waitNote.Count; j++)//音符竖轴判定
            {
                switch (speckles[i].Phase)
                {
                    case TouchPhase.Began:
                        switch (waitNote[j].thisNote.noteType)
                        {
                            case NoteType.Flick:
                                break;
                            default:
                                if (!speckles[i].isUsed)
                                {
                                    if (waitNote[j].IsinRange(speckles[i].CurrentPosition))
                                    {
                                        speckles[i].isUsed = !speckles[i].isUsed;
                                        waitNote[j].Judge(ProgressManager.Instance.CurrentTime);
                                    }
                                }
                                break;
                        }
                        break;
                    case TouchPhase.Moved:
                        switch (waitNote[j].thisNote.noteType)
                        {
                            case NoteType.Flick:
                            case NoteType.FullFlickPink:
                            case NoteType.FullFlickBlue:
                            case NoteType.Drag:
                                if (waitNote[j].IsinRange(speckles[i].CurrentPosition))
                                {
                                    waitNote[j].Judge(ProgressManager.Instance.CurrentTime);
                                }
                                break;

                        }
                        break;
                    case TouchPhase.Stationary:
                        switch (waitNote[j].thisNote.noteType)
                        {
                            case NoteType.Hold:
                            case NoteType.Drag:
                                if (waitNote[j].IsinRange(speckles[i].CurrentPosition))
                                {
                                    waitNote[j].Judge(ProgressManager.Instance.CurrentTime);
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }
    private void FindNotes(List<NoteController> needFindNotes, List<NoteController> waitNotes)
    {
        double currentTime = ProgressManager.Instance.CurrentTime;
        int index_startJudge_needJudgeNote = Algorithm.BinarySearch(needFindNotes, m => m.thisNote.hitTime < currentTime - JudgeManager.bad, false);
        int index_endJudge_needJudgeNote = Algorithm.BinarySearch(needFindNotes, m => m.thisNote.hitTime < currentTime + JudgeManager.bad + .00001, false);
        for (int i = index_startJudge_needJudgeNote; i < index_endJudge_needJudgeNote; i++)//每根线遍历每个出现的音符
        {
            //waitNote.Add(needFindNotes[i]);
            int index = Algorithm.BinarySearch(waitNote, m => m.thisNote.hitTime < needFindNotes[i].thisNote.hitTime, false);//寻找这个音符按照hitTime排序的话，应该插在什么位置
            waitNotes.Insert(index, needFindNotes[i]);//插入音符
        }
    }
}
[Serializable]
public struct Speckle//翻译为斑点，亦为安卓系统的触摸小白点，多个小白点酷似斑点，故起Speckle
{//一个斑点类就是一个手指的触摸
    [SerializeField] private float beganTime;//当前触摸段的开始时间
    public int thisIndex;//这个触摸是数组中的第几个索引
    public Vector2[] movePath;//移动过的路径
    public int index_movePath;//移动过的路径的索引
    public bool isFull_movePath;//移动过的路径是否已被全部填充
    public int length_movePath;
    public bool isUsed;
    public Vector2 CurrentPosition => movePath[index_movePath];
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
                    isUsed = false;
                    break;
            }
        }
    }
    /// <summary>
    /// 这个类的构造函数
    /// </summary>
    /// <param name="movePath">移动路径</param>
    /// <param name="startPoint">开始点</param>
    /// <param name="phase">触摸阶段</param>
    /// <param name="beganTime">开始时间</param>
    /// <param name="thisIndex">这个触摸在数组中的索引</param>
    /// <param name="index_movePath">movePath的索引</param>
    /// <param name="isFull_movePath">如果movePath已经满了</param>
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
        length_movePath = movePath.Length;
        isUsed = false;
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
        thisIndex = currentIndex;//先赋值当前索引
        movePath[index_movePath++] = main.ScreenToWorldPoint(touch.position);//然后吧传进来的屏幕像素坐标转换为世界坐标放进移动路径中
        Phase = touch.phase switch//看看人家系统给我传进来什么触摸阶段
        {
            TouchPhase.Began => TouchPhase.Began,//如果是触摸开始阶段那就直接赋值
            TouchPhase.Canceled => TouchPhase.Ended,//如果是取消跟踪那就默认抬起了手指
            TouchPhase.Ended => TouchPhase.Ended,//如果是抬起了手指那就直接赋值
            _ => IsMovedOrStationary(ValueManager.Instance.flickRange)//如果是剩下的Move阶段或者静止不动的阶段那就我自己来决定，不用人家给我的
        };
        CheckIndex(ref index_movePath, movePath.Length);//检查index_movePath是不是越界了
    }
    /// <summary>
    /// 判定是移动状态还是静止不动的状态
    /// </summary>
    /// <param name="deltaRange"></param>
    /// <returns></returns>
    TouchPhase IsMovedOrStationary(float deltaRange)
    {
        Vector2 currentPosition = movePath[LoopBackIndex(index_movePath, -1, length_movePath)];//获取到当前的位置
        Vector2 lastPosition = movePath[LoopBackIndex(index_movePath, -2, length_movePath)];//获取到上一帧的位置
        float deltaLength = (currentPosition - lastPosition).sqrMagnitude;//计算这一帧和上一帧手指的拉开的距离
        UIManager.Instance.DebugTextString = $"FingerIndex：{thisIndex}\n" +
            $"FlickLength:{deltaLength}\n" +
            $"TouchPhase:{phase}\n" +
            $"CurrentTargetFPS:{AssetManager.Instance.currentTargetFPS}";
        return deltaLength < deltaRange ? TouchPhase.Stationary : TouchPhase.Moved;//如果小于设定的值就判定为Stationary ，否侧判定为Moved
    }
    /// <summary>
    /// 索引回环，防止索引越界用的
    /// </summary>
    /// <param name="currentIndex"></param>
    /// <param name="needCalculateIndex"></param>
    /// <param name="maxIndex"></param>
    /// <returns></returns>
    int LoopBackIndex(int currentIndex, int needCalculateIndex, int maxIndex)
    {
        int indexDelta = currentIndex + needCalculateIndex;//看看计算过后的索引结果事什么
        int result;//最后要返回的结果
        if (indexDelta < 0)//如果小于0
        {
            result = indexDelta + maxIndex;//那就indexDelta + maxIndex
        }
        else if (indexDelta >= maxIndex)//如果大于等于最大值
        {
            result = indexDelta - maxIndex;//那就indexDelta - maxIndex
        }
        else//如果不大于也不小于
        {
            result = indexDelta;//直接赋值
        }
        return result;//返回最终结果
    }
    void CheckIndex(ref int index, int maxValue)//检查index是否大于最大值；
    {
        if (index >= maxValue)//如果index大于最大值；
            ResetIndex(ref index);//重置索引
    }
    void ResetIndex(ref int index) => index = 0;//重置一个索引

}
