using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
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
        List<Keyframe> keys =
                CalculatedCurve(thisLine.speed);//根据这根线的所有Speed事件，把他们的事件组合起来，成为一个Key的列表

        canvasLocalOffset = new(keys.ToArray()) { postWrapMode = WrapMode.ClampForever, preWrapMode = WrapMode.ClampForever };//将获得到的Key列表全部赋值
    }
    /// <summary>
    /// 计算曲线
    /// </summary>
    /// <param name="speeds">给我一个Speed事件列表</param>
    /// <returns>返回一个处理好的Key列表</returns>
    List<Keyframe> CalculatedCurve(Blophy.Chart.Event[] speeds)
    {
        List<Keyframe> keys = new();//声明一个Keys列表
        Vector2 keySeed = Vector2.zero;//Key种子，用来记录上一次循环结束时的Time和Value信息
        for (int i = 0; i < speeds.Length; i++)//循环遍历所有事件
        {
            float tant = (speeds[i].endValue - speeds[i].startValue)
                / (speeds[i].endTime - speeds[i].startTime);//这个计算的是：因为ST和ET与SV和EV形成的图形并不是正方形导致的斜率和百分比导致的误差，所以用Y/X计算出变化后的斜率

            DisposeKey(speeds, keys, keySeed, i, tant);//处理Key


            keySeed.x = keys[^1].time;//将这次处理后的最后一个Time赋值
            keySeed.y = keys[^1].value;//将这次处理后的最后一个Value赋值
        }
        return keys;//返回处理好的Key列表
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
            AddKey2KeyList(keys, keyframe);//将处理好的Key，加入Key的列表中
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
    void AddKey2KeyList(List<Keyframe> keys, Keyframe keyframe)
    {
        //int index = Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe.time);//使用二分严格搜索找这个时间是否存在Key
        //if (index >= 0) keys.RemoveAt(index);//如果存在就移除
        keys.Add(keyframe);//移除后添加
    }


    private void Update()
    {

    }
}