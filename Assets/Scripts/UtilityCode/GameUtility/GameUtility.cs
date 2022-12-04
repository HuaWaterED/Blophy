using Blophy.Chart;
public class GameUtility
{
    /// <summary>
    /// 这个就是在一个事件当中，根据时间获取值的一个方法
    /// </summary>
    /// <param name="event">传递一个事件给我</param>
    /// <param name="time">传递一个时间进去</param>
    /// <returns>返回的是这个时间（就是第二个参数）点上的值</returns>
    public static float GetValueWithEvent(Event @event, float time)
    {
        if (time < @event.startTime) return -.1f;//如果时间还没到开始时间，就直接返回-.1f
        float percentage = CalculatedPercentage(@event, time);
        float res = percentage * (@event.endValue - @event.startValue) + @event.startValue;//用百分比*总时间再加上开始时间就是当前这个时间所代表的值了
        return res;//返回这个值
    }

    private static float CalculatedPercentage(Event @event, float time)
    {
        float eventTimeDelta = @event.endTime - @event.startTime;//计算时间结束时间和开始时间的时间差
        if (eventTimeDelta <= 0) return 1;
        float currentTime = time - @event.startTime;//计算自从事件开始以来经历了多长时间
        float percentage = @event.curve.Evaluate(currentTime / eventTimeDelta);//用当前所经过的时间/时间差，子啊通过eva函数得到百分比
        return percentage;
    }
}
