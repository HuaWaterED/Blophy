using UnityEngine;
using Blophy.Chart;
using static UnityEngine.Camera;

public class BoxController : MonoBehaviour
{
    public Transform squarePosition;//方框的位置

    public DecideLineController[] decideLineControllers;//所有的判定线控制器
    public SpriteRenderer[] spriteRenderers;//所有的渲染组件

    public Box box;//谱面，单独这个box的谱面

    public int sortSeed = 0;//层级顺序种子
    public SpriteMask spriteMask;//遮罩

    public float currentScaleX;
    public float currentScaleY;
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
        return this;//返回自身
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
        //更新所有事件，拿到当前这一刻的数据
        CalculateAllEventCurrentValue(out float currentPositionX,
            out float currentPositionY, out float currentAngle,
            out float currentAlpha, out float currentLineAlpha,
            out float currentCenterX, out float currentCenterY,
            out currentScaleX, out currentScaleY,
            (float)ProgressManager.Instance.CurrentTime);
        //将当前这一刻的数据全部赋值给方框
        GiveEventData2Box(currentPositionX, currentPositionY,
            currentAngle, currentAlpha, currentLineAlpha,
            currentCenterX, currentCenterY,
            currentScaleX,
            currentScaleY);
    }
    /// <summary>
    /// 根据谱面数据更新当前所有事件
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
    /// <param name="currentTime">当前时间</param>
    void CalculateAllEventCurrentValue(out float currentPositionX,
         out float currentPositionY, out float currentAngle, out float currentAlpha,
         out float currentLineAlpha, out float currentCenterX, out float currentCenterY,
        out float currentScaleX, out float currentScaleY, float currentTime)
    {
        currentPositionX = CalculateCurrentValue(box.boxEvents.moveX, currentTime);
        currentPositionY = CalculateCurrentValue(box.boxEvents.moveY, currentTime);
        currentCenterX = CalculateCurrentValue(box.boxEvents.centerX, currentTime);
        currentCenterY = CalculateCurrentValue(box.boxEvents.centerY, currentTime);
        currentAngle = CalculateCurrentValue(box.boxEvents.rotate, currentTime);
        currentAlpha = CalculateCurrentValue(box.boxEvents.alpha, currentTime);
        currentLineAlpha = CalculateCurrentValue(box.boxEvents.lineAlpha, currentTime);
        currentScaleX = CalculateCurrentValue(box.boxEvents.scaleX, currentTime);
        currentScaleY = CalculateCurrentValue(box.boxEvents.scaleY, currentTime);
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
            spriteRenderers[i].color = new Color(0, 0, 0, currentAlpha);//1234根线赋值
        spriteRenderers[4].color = new Color(0, 0, 0, currentLineAlpha);//最后那条线单独赋值

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
    public float CalculateCurrentValue(Blophy.Chart.Event[] events, float currentTime)
    {
        int eventIndex = /*Algorithm.BinarySearch(events, currentTime);*/
            Algorithm.BinarySearch(events, m => currentTime >= m.startTime, true);//找到当前时间下，应该是哪个事件
        return GameUtility.GetValueWithEvent(events[eventIndex], currentTime);//拿到事件后根据时间Get到当前值
    }
}
