using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
public class DecideLineController : MonoBehaviour
{
    [Tooltip("如果是自由框，那就用这个作为音符的爸爸")] public Transform freeBox_NoteParsent;

    public Transform onlineNote;
    public Transform offlineNote;

    public AnimationCurve canvasLocalOffset;

    public float lineDistance;

    public Line thisLine;

    public List<AnimationCurve> tempUses;
    private void Start()
    {
        List<Keyframe> keys =
                CalculatedCurve(thisLine.speed);

        canvasLocalOffset = new(keys.ToArray()) { postWrapMode = WrapMode.ClampForever, preWrapMode = WrapMode.ClampForever };
    }
    List<Keyframe> CalculatedCurve(Blophy.Chart.Event[] speeds)
    {
        List<Keyframe> keys = new();
        Vector2 keySeed = Vector2.zero;
        for (int i = 0; i < speeds.Length; i++)
        {
            float tant = (speeds[i].endValue - speeds[i].startValue)
                / (speeds[i].endTime - speeds[i].startTime);

            DisposeKey(speeds, keys, keySeed, i, tant);


            keySeed.x = keys[^1].time;
            keySeed.y = keys[^1].value;
        }
        return keys;
    }

    void DisposeKey(Blophy.Chart.Event[] speeds, List<Keyframe> keys, Vector2 keySeed, int i, float tant)
    {
        for (int j = 0; j < speeds[i].curve.length; j++)
        {
            Keyframe keyframe = InstKeyframe(speeds, keySeed, i, tant, j);
            if (i != 0 && j == 0)
            {
                keyframe.inTangent = keys[^1].inTangent;
                keyframe.inWeight = keys[^1].inWeight;
            }
            AddKey2KeyList(keys, keyframe);
        }
    }
    Keyframe InstKeyframe(Blophy.Chart.Event[] speeds, Vector2 keySeed, int i, float tant, int index)
    {
        Keyframe keyframe = speeds[i].curve.keys[index];
        keyframe.weightedMode = WeightedMode.Both;
        keyframe.time = (speeds[i].endTime - speeds[i].startTime) * keyframe.time + keySeed.x;
        keyframe.value = (speeds[i].endValue - speeds[i].startValue) * keyframe.value + keySeed.y;

        keyframe.outTangent *= tant;
        keyframe.inTangent *= tant;
        return keyframe;
    }

    void AddKey2KeyList(List<Keyframe> keys, Keyframe keyframe)
    {
        int index = Algorithm.BinaryStrictSearch(keys.ToArray(), keyframe.time);
        if (index >= 0) keys.RemoveAt(index);
        keys.Add(keyframe);
    }


    private void Update()
    {

    }
}