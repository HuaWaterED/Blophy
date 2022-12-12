using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using System.Security.Cryptography;
/*
* 声明：
* StartTime简称ST
* EndTime简称ET
* StartValue简称SV
* EndValue简称EV
* 
* 百分比解释：
* 因为Unity的AnimationCurve的Key的InWeight和OutWeight的表示
* InWeight就是这个点距离上一个点为单位，自身为0，到上一个点的距离为1的表示法
* OutWeight就是这个点距离下一个点为单位，自身为0，到下一个点的距离为1的表示法
* 这里说为百分比是觉得百分比也是0-1之间的标准数值
*/
public class DecideLineController : MonoBehaviour
{
    [Tooltip("如果是自由框，那就用这个作为音符的爸爸")] public Transform freeBox_NoteParsent;

    public Transform onlineNote;//判定线上边的音符
    public Transform offlineNote;//判定线下边的音符

    public AnimationCurve canvasLocalOffset;//这个用来表示的是某个时间，画布的Y轴应该是多少
    public AnimationCurve canvasSpeed;//这个用来表示这根线的所有速度总览

    public float lineDistance;//线的距离，根据每帧计算，生成这个范围内的所有Note

    public Line thisLine;//这跟线的谱面元数据

    /// <summary>
    /// Start方法
    /// </summary>
    private void Start()
    {
        List<Keyframe> keyframes = CalculatedSpeedCurve(thisLine.speed);//将获得到的Key列表全部赋值
        canvasSpeed = new() { keys = keyframes.ToArray(), preWrapMode = WrapMode.ClampForever, postWrapMode = WrapMode.ClampForever };//把上边获得到的点转换为速度图
        canvasLocalOffset = CalculatedOffsetCurve(canvasSpeed, keyframes);//吧速度图转换为位移图
    }
    /// <summary>
    /// 根据速度图计算位移图
    /// </summary>
    /// <param name="canvasSpeed">速度图</param>
    /// <param name="keyframes">速度图的点</param>
    /// <returns>返回一个位移图</returns>
    AnimationCurve CalculatedOffsetCurve(AnimationCurve canvasSpeed, List<Keyframe> keyframes)
    {
        /*速度图的value等于位移图的斜率*/
        List<Keyframe> resultKeyframes = new()//声明一个列表，第一个点默认从0，0开始
        {
            new() {weightedMode=WeightedMode.Both, time = 0, value = 0, outWeight = keyframes[0].outWeight, outTangent = keyframes[0].value }
        };

        //下面是第一层for循环，，因为上边自动添加了第一个点，所以这里直接跳过第一个点，避免了数组越界的bug
        for (int i = 1; i < keyframes.Count; i++)
        {
            float result = 0;//计算这个点和上一个点的面积
            for (float j = keyframes[i - 1].time;//j等于上一个点的Time
                j < keyframes[i].time;//让j小于这个点的Time，就是让j处于这个点和上一点之间
                j += ValueManager.Instance.calculatedAreaRange)//每次处理步长
            {
                float currentValue = canvasSpeed.Evaluate(i - 1 + j);//用i-1定位上一个点的索引，用j定位当前的处于那个区间
                float lastTimeValue = canvasSpeed.Evaluate(i - 1 + j - ValueManager.Instance.calculatedAreaRange);//用i-1定位上一个点的索引，用j定位当前处于那个区间，再减去面积计算步长，配合上一行代码得到梯形的上底加下底


                result += (currentValue + lastTimeValue) * ValueManager.Instance.calculatedAreaRange / 2;//面积累加，加的内容是：（上底加下底）乘高除2
                result = (float)Math.Round(result, 3);//为了较小误差，保留小数点后三位数，四舍五入
                j = (float)Math.Round(j, ValueManager.Instance.reservedBits);//为了较小误差，保留小数点后三位数，四舍五入
            }

            Keyframe keyframe = new()//声明一个key
            {
                weightedMode = WeightedMode.Both,//key的模式为Both
                value = result + resultKeyframes[^1].value,//Key的Value直接等于面积结果加上次计算点的value
                time = keyframes[i].time,//时间数据直接赋值
            };

            keyframe.inTangent = keyframes.Find(m => m.time == keyframe.time).value;//key的入点斜率等于在这一时刻下所有点中第一个点的速度（value）值
            keyframe.outTangent = keyframes.FindLast(m => m.time == keyframe.time).value;//key的出点斜率等于在这一时刻下所有点中最后一个点的速度（value）值
            keyframe.inWeight = keyframes.Find(m => m.time == keyframe.time).inWeight;//key的入点百分比等于在这一时刻下所有点中第一个点的百分比
            keyframe.outWeight = keyframes.FindLast(m => m.time == keyframe.time).outWeight;//key的出点百分比等于在这一时刻下所有点中最后一个点的百分比
            AddKey2KeyList(resultKeyframes, keyframe, true);//使用严格搜索，如果这个时间有key就踢掉之前的key重加一次
        }
        return new() { keys = resultKeyframes.ToArray(), preWrapMode = WrapMode.ClampForever, postWrapMode = WrapMode.ClampForever };//把处理好的Key放到AnimationCurve里返回出去
    }
    /// <summary>
    /// 计算速度曲线
    /// </summary>
    /// <param name="speeds">给我一个Speed事件列表</param>
    /// <returns>返回一个处理好的AnimationCurve</returns>
    List<Keyframe> CalculatedSpeedCurve(Blophy.Chart.Event[] speeds)
    {
        List<Keyframe> keys = new();//声明一个Keys列表
        Vector2 keySeed_Speed = Vector2.zero;//Key种子，用来记录上一次循环结束时的Time和Value信息
        for (int i = 0; i < speeds.Length; i++)//循环遍历所有事件
        {
            float tant = (speeds[i].endValue - speeds[i].startValue)
                / (speeds[i].endTime - speeds[i].startTime);//这个计算的是：因为ST和ET与SV和EV形成的图形并不是正方形导致的斜率和百分比导致的误差，所以用Y/X计算出变化后的斜率

            DisposeKey(speeds, keys, keySeed_Speed, i, tant);//处理Key


            keySeed_Speed.x = keys[^1].time;//将这次处理后的最后一个Time赋值
            keySeed_Speed.y = keys[^1].value;//将这次处理后的最后一个Value赋值
        }
        return keys;//将获得到的Key列表全部赋值,然后返回出去
    }
    /// <summary>
    /// 处理Key
    /// </summary>
    /// <param name="speeds">Speed事件列表</param>
    /// <param name="keys">存处理后的key的列表</param>
    /// <param name="keySeed">上次处理完后最后一个Key的Time和Value值</param>
    /// <param name="i">这次处于第几循环</param>
    /// <param name="tant">斜率</param>
    void DisposeKey(Blophy.Chart.Event[] speeds, List<Keyframe> keys, Vector2 keySeed, int i, float tant)
    {
        for (int j = 0; j < speeds[i].curve.length; j++)//循环遍历所有的Key
        {
            Keyframe keyframe = InstKeyframe(speeds, keySeed, i, tant, j);//生成一个Key
            if (i != 0 && j == 0)//如果不是第一个Speed事件并且是第一个AnimationCurve的Key
            {
                keyframe.inTangent = keys[^1].inTangent;//将上次处理后的最后一个key的入点斜率拿到
                keyframe.inWeight = keys[^1].inWeight;//将上次处理后的最后一个key的入点百分比拿到
            }
            if (keys.Count != 0 && keyframe.time == keys[^1].time && keyframe.value == keys[^1].value)
                AddKey2KeyList(keys, keyframe, true);//将处理好的Key，加入Key的列表中
            else
                AddKey2KeyList(keys, keyframe, false);//将处理好的Key，加入Key的列表中
        }
    }
    /// <summary>
    /// 召唤一个Key
    /// </summary>
    /// <param name="speeds">速度列表</param>
    /// <param name="keySeed">上次处理完Key的Time和Value值</param>
    /// <param name="i">这次是处于第几循环</param>
    /// <param name="tant">斜率</param>
    /// <param name="index">这个是处于这个AnimationCurve中的第几个Key</param>
    /// <returns>返回一个处理好的Key</returns>
    Keyframe InstKeyframe(Blophy.Chart.Event[] speeds, Vector2 keySeed, int i, float tant, int index)
    {
        Keyframe keyframe = speeds[i].curve.keys[index];//把Key拿出来
        keyframe.weightedMode = WeightedMode.Both;//设置一下模式
        keyframe.time = (speeds[i].endTime - speeds[i].startTime) * keyframe.time + keySeed.x;//（当前事件的结束时间-开始时间）*当前Key的时间+上次事件处理完后的最后一个Key的时间
        keyframe.value = (speeds[i].endValue - speeds[i].startValue) * keyframe.value + speeds[i].startValue;//（当前事件的结束值-开始值）*当前Key的值+上次事件处理完后的最后一个Key的值

        keyframe.outTangent *= tant;//出点的斜率适应一下变化
        keyframe.inTangent *= tant;//入店的斜率适应一下变化（就是消除因为非正方形导致的误差）
        return keyframe;//返回Key
    }
    /// <summary>
    /// 将Key加入到Key列表
    /// </summary>
    /// <param name="keys">需要添加的Key列表</param>
    /// <param name="keyframe">需要添加的Key</param>
    void AddKey2KeyList(List<Keyframe> keys, Keyframe keyframe, bool isMove)
    {
        if (keys.Count != 0 && isMove)
        {
            int index = Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe.time);//使用二分严格搜索找这个时间是否存在Key
            if (index >= 0) keys.RemoveAt(index);//如果存在就移除
        }
        keys.Add(keyframe);//移除后添加
    }


    private void Update()
    {

    }
}