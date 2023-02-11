using UnityEngine;
using Blophy.Chart;
using static UnityEngine.Camera;
using Event = Blophy.Chart.Event;
using System.Collections;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Mathf;
using System;

public class BoxController : MonoBehaviour
{
    public Transform squarePosition;//方框的位置

    public DecideLineController[] decideLineControllers;//所有的判定线控制器
    public SpriteRenderer[] spriteRenderers;//所有的渲染组件
    public ObjectPoolQueue<RippleController> ripples;

    public Box box;//谱面，单独这个box的谱面

    public int sortSeed = 0;//层级顺序种子
    public SpriteMask spriteMask;//遮罩

    public float currentScaleX;
    public float currentScaleY;
    public float currentAlpha;        //默认值
    public float currentCenterX;    //默认值
    public float currentCenterY;    //默认值
    public float currentLineAlpha;    //默认值
    public float currentMoveX;        //默认值
    public float currentMoveY;        //默认值
    public float currentRotate;       //默认值
    public float boxFineness;

    public Vector2 raw_center;
    public Vector2 center;
    public Color alpha;
    public Color lineAlpha;
    public Vector2 move;
    public Vector2 scale;
    public Quaternion rotation;
    public Vector2 horizontalFineness;
    public Vector2 verticalFineness;
    /// <summary>
    /// 设置遮罩种子
    /// </summary>
    /// <param name="sortSeed">种子开始</param>
    /// <returns>返回自身</returns>
    public BoxController SetSortSeed(int sortSeed)
    {
        this.sortSeed = sortSeed;//设置我自己的遮罩到我自己
        spriteMask.frontSortingOrder = sortSeed + ValueManager.Instance.noteRendererOrder - 1;//遮罩种子+一共多少层-1（这个1是我自己占用了，所以减去）
        spriteMask.backSortingOrder = sortSeed - 1;//遮罩的优先级是前包容后不包容，所以后的遮罩层级向下探一个
        for (int i = 0; i < spriteRenderers.Length; i++)//赋值渲染层级到组成渲染的各个组件们
        {
            spriteRenderers[i].sortingOrder = sortSeed;//赋值
        }
        return this;//返回自己
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="thisBox">这个方框</param>
    /// <returns></returns>
    public BoxController Init(Box thisBox)
    {
        box = thisBox;//赋值thisBox到box
        int length_decideLineControllers = decideLineControllers.Length;//获得到当前判定线的数量
        for (int i = 0; i < length_decideLineControllers; i++)//遍历
        {
            decideLineControllers[i].ThisLine = box.lines[i];//将line的源数据赋值过去
        }
        boxFineness = ValueManager.Instance.boxFineness;
        ripples = new(AssetManager.Instance.ripple, 0, squarePosition);
        return this;//返回自身
    }
    public void PlayRipple() => StartCoroutine(Play());
    public IEnumerator Play()
    {
        RippleController ripple = ripples.PrepareObject().Init(currentScaleX, currentScaleY);
        yield return new WaitForSeconds(1.1f);//打击特效时长是0.5秒，0.6秒是为了兼容误差
        ripples.ReturnObject(ripple);
    }
    private void Update()
    {
        UpdateEvents();
    }
    void UpdateEvents()
    {
        float currentTime = (float)ProgressManager.Instance.CurrentTime;
        UpdateCenterAndRotation(ref currentCenterX, ref currentCenterY, ref currentRotate, ref currentTime);
        if (box.boxEvents.Length_alpha > 0)
            UpdateAlpha(ref currentAlpha, ref currentTime);
        if (box.boxEvents.Length_lineAlpha > 0)
            UpdateLineAlpha(ref currentLineAlpha, ref currentTime);
        UpdateMove(ref currentMoveX, ref currentMoveY, ref currentTime);
        UpdateScale(ref currentScaleX, ref currentScaleY, ref currentTime);
        UpdateFineness();
    }

    void UpdateCenterAndRotation(ref float currentCenterX, ref float currentCenterY, ref float currentRotate, ref float currentTime)
    {
        if (box.boxEvents.Length_centerX > 0)
        {
            currentCenterX = CalculateCurrentValue(box.boxEvents.centerX, ref currentTime, ref this.currentCenterX);
            raw_center.x = currentCenterX;
        }

        if (box.boxEvents.Length_centerY > 0)
        {
            currentCenterY = CalculateCurrentValue(box.boxEvents.centerY, ref currentTime, ref this.currentCenterY);
            raw_center.y = currentCenterY;
        }
        center = main.ViewportToWorldPoint(raw_center);

        if (box.boxEvents.Length_rotate > 0)
        {
            currentRotate = CalculateCurrentValue(box.boxEvents.rotate, ref currentTime, ref this.currentRotate);
            rotation = Quaternion.Euler(Vector3.forward * currentRotate);
        }
        transform.SetPositionAndRotation(center, rotation);
    }

    void UpdateAlpha(ref float currentAlpha, ref float currentTime)
    {
        currentAlpha = CalculateCurrentValue(box.boxEvents.alpha, ref currentTime, ref this.currentAlpha);
        alpha.a = currentAlpha;
        spriteRenderers[0].color =
        spriteRenderers[1].color =
        spriteRenderers[2].color =
        spriteRenderers[3].color = alpha;//1234根线赋值，这里的0，0，0就是黑色的线
    }

    void UpdateLineAlpha(ref float currentLineAlpha, ref float currentTime)
    {
        currentLineAlpha = CalculateCurrentValue(box.boxEvents.lineAlpha, ref currentTime, ref this.currentLineAlpha);
        lineAlpha.a = currentLineAlpha;
        spriteRenderers[4].color = lineAlpha;
    }

    void UpdateMove(ref float currentMoveX, ref float currentMoveY, ref float currentTime)
    {
        if (box.boxEvents.Length_moveX > 0)
        {
            currentMoveX = CalculateCurrentValue(box.boxEvents.moveX, ref currentTime, ref this.currentMoveX);
            move.x = currentMoveX;
        }
        if (box.boxEvents.Length_moveY > 0)
        {
            currentMoveY = CalculateCurrentValue(box.boxEvents.moveY, ref currentTime, ref this.currentMoveY);
            move.y = currentMoveY;
        }
        squarePosition.localPosition = move;
    }

    void UpdateScale(ref float currentScaleX, ref float currentScaleY, ref float currentTime)
    {
        if (box.boxEvents.Length_scaleX > 0)
        {
            currentScaleX = CalculateCurrentValue(box.boxEvents.scaleX, ref currentTime, ref this.currentScaleX);
            scale.x = currentScaleX;
        }
        if (box.boxEvents.Length_scaleY > 0)
        {
            currentScaleY = CalculateCurrentValue(box.boxEvents.scaleY, ref currentTime, ref this.currentScaleY);
            scale.y = currentScaleY;
        }
        squarePosition.localScale = scale;
    }

    void UpdateFineness()
    {
        horizontalFineness.x = 2 - (boxFineness / currentScaleX);
        horizontalFineness.y = boxFineness / currentScaleY;
        verticalFineness.x = 2 + (boxFineness / currentScaleY);
        verticalFineness.y = boxFineness / currentScaleX;
        //缩放图片，保持视觉上相等
        spriteRenderers[0].transform.localScale =//第125根线都是水平的
            spriteRenderers[1].transform.localScale =
            spriteRenderers[4].transform.localScale = horizontalFineness;

        spriteRenderers[2].transform.localScale =//第34都是垂直的
            spriteRenderers[3].transform.localScale = verticalFineness;
        //这里的2是初始大小*2得到的结果，初始大小就是Prefabs里的
    }
    /// <summary>
    /// 计算当前数值
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public float CalculateCurrentValue(Event[] events, ref float currentTime, ref float defaultValue)
    {
        if (currentTime < events[0].startTime) return defaultValue;
        int eventIndex = Algorithm.BinarySearch(events, IsCurrentEvent, true, ref currentTime);//找到当前时间下，应该是哪个事件
        if (currentTime > events[eventIndex].endTime && events[eventIndex].endValue != 0) return events[eventIndex].endValue;
        return GameUtility.GetValueWithEvent(events[eventIndex], currentTime);//拿到事件后根据时间Get到当前值
    }
    public bool IsCurrentEvent(Event m, ref float currentTime) => currentTime >= m.startTime;

}
