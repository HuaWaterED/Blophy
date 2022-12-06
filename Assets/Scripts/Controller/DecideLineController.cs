using Blophy.Chart;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideLineController : MonoBehaviour
{
    [Tooltip("如果是自由框，那就用这个作为音符的爸爸")] public Transform freeBox_NoteParsent;

    public Transform onlineNote;
    public Transform offlineNote;

    public AnimationCurve canvasGlobalOffset;
    public AnimationCurve canvasLocalOffset;

    public float lineDistance;

    public Line thisLine;

    private void Start()
    {

        List<Keyframe> keys = CalculatedCurve(AssetManager.Instance.chartData.globalData.speed);

        canvasGlobalOffset = new(keys.ToArray()) { postWrapMode = WrapMode.ClampForever, preWrapMode = WrapMode.ClampForever };
    }
    List<Keyframe> CalculatedCurve(List<Blophy.Chart.Event> speeds)
    {
        List<Keyframe> keys = new();
        Vector2 keySeed = Vector2.zero;
        for (int i = 0; i < speeds.Count; i++)
        {
            float tant = (speeds[i].endValue - speeds[i].startValue)
                / (speeds[i].endTime - speeds[i].startTime);

            //这里处理第一个
            Keyframe keyframe = speeds[i].curve.keys[0];
            keyframe.weightedMode = WeightedMode.Both;
            keyframe.time *= speeds[i].endTime - speeds[i].startTime;
            keyframe.time += keySeed.x;
            keyframe.value *= speeds[i].endValue - speeds[i].startValue;
            keyframe.value += keySeed.y;
            keyframe.outTangent *= tant;
            if (i != 0)
            {
                keyframe.inTangent = speeds[i - 1].curve.keys[^1].inTangent;
                keyframe.inWeight = speeds[i].curve.keys[^1].inWeight;
            }
            if (Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe.time) < 0)
            {
                keys.Add(keyframe);
            }
            else
            {
                int index = Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe.time);
                keys.RemoveAt(index);
                keys.Add(keyframe);
            }
            //
            for (int j = 1; j < speeds[i].curve.length - 1; j++)
            {
                Keyframe keyframe1 = speeds[i].curve.keys[j];
                keyframe1.weightedMode = WeightedMode.Both;
                keyframe1.time *= speeds[i].endTime - speeds[i].startTime;
                keyframe1.time += keySeed.x;
                keyframe1.value *= speeds[i].endValue - speeds[i].startValue;
                keyframe1.value += keySeed.y;
                keyframe1.outTangent *= tant;
                keyframe1.inTangent *= tant;
                if (Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe1.time) < 0)
                {
                    keys.Add(keyframe1);
                }
                else
                {
                    int index = Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe1.time);
                    keys.RemoveAt(index);
                    keys.Add(keyframe1);
                }
            }
            //这里处理最后一个
            Keyframe keyframe2 = speeds[i].curve.keys[^1];
            keyframe2.weightedMode = WeightedMode.Both;
            keyframe2.time *= speeds[i].endTime - speeds[i].startTime;
            keyframe2.time += keySeed.x;
            keyframe2.value *= speeds[i].endValue - speeds[i].startValue;
            keyframe2.value += keySeed.y;
            keyframe2.inTangent *= tant;
            if (i != speeds.Count - 1)
            {
                keyframe2.inTangent = speeds[i + 1].curve.keys[0].inTangent;
                keyframe2.inWeight = speeds[i].curve.keys[0].inWeight;
            }
            if (Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe2.time) < 0)
            {
                keys.Add(keyframe2);
            }
            else
            {
                int index = Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe2.time);
                keys.RemoveAt(index);
                keys.Add(keyframe2);
            }
            //
            keySeed.x = keyframe2.time;
            keySeed.y = keyframe2.value;
        }
        return keys;
    }
    private void Update()
    {

    }
}
