using UnityEngine;
using Blophy.Chart;
using static UnityEngine.Camera;

public class Box : MonoBehaviour
{
    public Transform centerPoint;

    public DecideLineController[] decideLineControllers;
    public SpriteRenderer[] spriteRenderers;

    public Blophy.Chart.Box box;

    private void Update()
    {
        UpdateCurrentEvents();
    }
    /// <summary>
    /// 这里更新所有事件
    /// </summary>
    void UpdateCurrentEvents()
    {

        CalculateAllEventCurrentValue(out float currentPositionX,
            out float currentPositionY, out float currentAngle,
            out float currentAlpha, out float currentLineAlpha,
            out float currentCenterX, out float currentCenterY,
            out float currentScaleX, out float currentScaleY);

        GiveEventData2Box(currentPositionX, currentPositionY,
            currentAngle, currentAlpha, currentLineAlpha,
            currentCenterX, currentCenterY,
            currentScaleX,
            currentScaleY);
    }
    void CalculateAllEventCurrentValue(out float currentPositionX,
         out float currentPositionY, out float currentAngle, out float currentAlpha,
         out float currentLineAlpha, out float currentCenterX, out float currentCenterY,
        out float currentScaleX, out float currentScaleY)
    {
        currentPositionX = CalculateCurrentValue(box.boxEvents.moveX);
        currentPositionY = CalculateCurrentValue(box.boxEvents.moveY);
        currentCenterX = CalculateCurrentValue(box.boxEvents.centerX);
        currentCenterY = CalculateCurrentValue(box.boxEvents.centerY);
        currentAngle = CalculateCurrentValue(box.boxEvents.rotate);
        currentAlpha = CalculateCurrentValue(box.boxEvents.alpha);
        currentLineAlpha = CalculateCurrentValue(box.boxEvents.lineAlpha);
        currentScaleX = CalculateCurrentValue(box.boxEvents.scaleX);
        currentScaleY = CalculateCurrentValue(box.boxEvents.scaleY);
    }
    void GiveEventData2Box(float currentPositionX, float currentPositionY,
           float currentAngle, float currentAlpha, float currentLineAlpha,
           float currentCenterX, float currentCenterY, float currentScaleX,
           float currentScaleY)
    {
        //设置位置和旋转
        transform.SetPositionAndRotation((Vector2)main.ViewportToWorldPoint(new Vector2(currentCenterX, currentCenterY)), Quaternion.Euler(Vector3.forward * currentAngle));

        //透明度
        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].color = i switch
            {
                4 => new Color(0, 0, 0, currentLineAlpha),
                _ => new Color(0, 0, 0, currentAlpha)
            };

        //中心点的位置
        centerPoint.localPosition = (Vector2)main.ViewportToWorldPoint(new Vector2(currentPositionX, currentPositionY));

        //设置scale

        centerPoint.localScale = new Vector2(currentScaleX, currentScaleY);

        //缩放图片，保持视觉上相等
        spriteRenderers[0].transform.localScale =
            spriteRenderers[1].transform.localScale =
            spriteRenderers[4].transform.localScale =
            new Vector2(2 - (.1f / currentScaleX), .1f / currentScaleY);

        spriteRenderers[2].transform.localScale =
            spriteRenderers[3].transform.localScale =
            new Vector2(2 + (.1f / currentScaleY), .1f / currentScaleX);

        //结束设置scale
    }

    public float CalculateCurrentValue(Blophy.Chart.Event[] events)
    {
        int eventIndex = Algorithm.BinarySearch(events, (float)ProgressManager.Instance.CurrentTime);
        return GameUtility.GetValueWithEvent(events[eventIndex], (float)ProgressManager.Instance.CurrentTime);
    }
}
