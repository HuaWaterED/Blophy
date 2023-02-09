using UnityEngine;
using Blophy.Chart;
using static UnityEngine.Camera;
using Event = Blophy.Chart.Event;
using System.Collections;

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
        foreach (var item in spriteRenderers)//赋值渲染层级到组成渲染的各个组件们
        {
            item.sortingOrder = sortSeed;//赋值
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
        UpdateCurrentEvents();//更新所有事件
    }
    /// <summary>
    /// 这里更新所有事件
    /// </summary>
    void UpdateCurrentEvents()
    {
        float currentTime = (float)ProgressManager.Instance.CurrentTime;
        //更新所有事件，拿到当前这一刻的数据
        CalculateAllEventCurrentValue(
            out currentMoveX,
            out currentMoveY,
               out currentRotate,
               out currentAlpha,
            out currentLineAlpha,
             out currentCenterX,
             out currentCenterY,
            out currentScaleX,
            out currentScaleY,
            ref currentTime);
        //将当前这一刻的数据全部赋值给方框
        GiveEventData2Box(currentMoveX, currentMoveY,
            currentRotate, currentAlpha, currentLineAlpha,
            currentCenterX, currentCenterY,
            currentScaleX,
            currentScaleY);
    }
    /// <summary>
    /// 根据谱面数据更新当前所有事件
    /// </summary>
    /// <param name="currentMoveX">当前基于Center的LocalPositionX</param>
    /// <param name="currentMoveY">当前基于Center的LocalPositionY</param>
    /// <param name="currentRotate">当前角度</param>
    /// <param name="currentAlpha">当前方框的Alpha</param>
    /// <param name="currentLineAlpha">当前方框中间那根线的Alpha</param>
    /// <param name="currentCenterX">基于屏幕的X</param>
    /// <param name="currentCenterY">基于屏幕的Y</param>
    /// <param name="currentScaleX">当前的X缩放</param>
    /// <param name="currentScaleY">当前的Y缩放</param>
    /// <param name="currentTime">当前时间</param>
    void CalculateAllEventCurrentValue(out float currentMoveX,
         out float currentMoveY, out float currentRotate, out float currentAlpha,
         out float currentLineAlpha, out float currentCenterX, out float currentCenterY,
        out float currentScaleX, out float currentScaleY, ref float currentTime)
    {
        currentMoveX = CalculateCurrentValue(box.boxEvents.moveX, ref currentTime, ref this.currentMoveX);
        currentMoveY = CalculateCurrentValue(box.boxEvents.moveY, ref currentTime, ref this.currentMoveY);
        currentCenterX = CalculateCurrentValue(box.boxEvents.centerX, ref currentTime, ref this.currentCenterX);
        currentCenterY = CalculateCurrentValue(box.boxEvents.centerY, ref currentTime, ref this.currentCenterY);
        currentRotate = CalculateCurrentValue(box.boxEvents.rotate, ref currentTime, ref this.currentRotate);
        currentAlpha = CalculateCurrentValue(box.boxEvents.alpha, ref currentTime, ref this.currentAlpha);
        currentLineAlpha = CalculateCurrentValue(box.boxEvents.lineAlpha, ref currentTime, ref this.currentLineAlpha);
        currentScaleX = CalculateCurrentValue(box.boxEvents.scaleX, ref currentTime, ref this.currentScaleX);
        currentScaleY = CalculateCurrentValue(box.boxEvents.scaleY, ref currentTime, ref this.currentScaleY);
    }
    /// <summary>
    /// 把所有的方框数据都给Box
    /// </summary>
    /// <param name="currentPositionX">当前基于Center的LocalPositionX</param>
    /// <param name="currentPositionY">当前基于Center的LocalPositionY</param>
    /// <param name="currentAngle">当前角度</param>
    /// <param name="currentAlpha">当前方框的Alpha</param>
    /// <param name="currentLineAlpha">当前方框中间那根线的Alpha</param>
    /// <param name="currentCenterX">基于屏幕的X</param>
    /// <param name="currentCenterY">基于屏幕的Y</param>
    /// <param name="currentScaleX">当前的X缩放</param>
    /// <param name="currentScaleY">当前的Y缩放</param>
    void GiveEventData2Box(float currentPositionX, float currentPositionY,
           float currentAngle, float currentAlpha, float currentLineAlpha,
           float currentCenterX, float currentCenterY, float currentScaleX,
           float currentScaleY)
    {
        //设置位置和旋转
        transform.SetPositionAndRotation((Vector2)main.ViewportToWorldPoint(new(currentCenterX, currentCenterY)), Quaternion.Euler(Vector3.forward * currentAngle));

        //透明度
        for (int i = 0; i < spriteRenderers.Length - 1; i++)
            spriteRenderers[i].color = new Color(0, 0, 0, currentAlpha);//1234根线赋值，这里的0，0，0就是黑色的线
        spriteRenderers[4].color = new Color(0, 0, 0, currentLineAlpha);//最后那条线单独赋值，这里的0，0，0就是黑色的线

        //方框的位置
        squarePosition.localPosition = new Vector2(currentPositionX, currentPositionY);

        //设置scale

        squarePosition.localScale = new Vector2(currentScaleX, currentScaleY);

        //缩放图片，保持视觉上相等
        spriteRenderers[0].transform.localScale =//第125根线都是水平的
            spriteRenderers[1].transform.localScale =
            spriteRenderers[4].transform.localScale =
            new Vector2(2 - (ValueManager.Instance.boxFineness / currentScaleX), ValueManager.Instance.boxFineness / currentScaleY);

        spriteRenderers[2].transform.localScale =//第34都是垂直的
            spriteRenderers[3].transform.localScale =
            new Vector2(2 + (ValueManager.Instance.boxFineness / currentScaleY), ValueManager.Instance.boxFineness / currentScaleX);
        //这里的2是初始大小*2得到的结果，初始大小就是Prefabs里的
        //结束设置scale
    }
    /// <summary>
    /// 计算当前数值
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public float CalculateCurrentValue(Event[] events, ref float currentTime, ref float defaultValue)
    {
        if (events.Length <= 0 || currentTime < events[0].startTime) return defaultValue;
        int eventIndex = Algorithm.BinarySearch(events, IsCurrentEvent, true, ref currentTime);//找到当前时间下，应该是哪个事件
        if (currentTime > events[eventIndex].endTime && events[eventIndex].endValue != 0) return events[eventIndex].endValue;
        return GameUtility.GetValueWithEvent(events[eventIndex], currentTime);//拿到事件后根据时间Get到当前值
    }
    public bool IsCurrentEvent(Event m, ref float currentTime) => currentTime >= m.startTime;

}
