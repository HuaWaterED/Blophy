using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace mewmesh.Extension
{
    public static class AnimationCurveExtension
    {
        public static void ClearAll(this AnimationCurve curve)
        {
            for (int i = 0; i < curve.keys.Length; i++)
                curve.RemoveKey(i);
        }
        public static void SyncKeysCount(this AnimationCurve curve, int keysCount)
        {
            if (curve.keys.Length == keysCount) return;
            curve.ClearAll();
            curve.preWrapMode = WrapMode.ClampForever;
            curve.postWrapMode = WrapMode.ClampForever;
            for (int i = 0; i < keysCount; i++)
            {
                Keyframe key = new()
                {
                    time = i / 10f,
                    value = i / 10f,
                    outTangent = i / 10f,
                    outWeight = i / 10f,
                    inWeight = i / 10f,
                    inTangent = i / 10f,
                    weightedMode = WeightedMode.Both
                };
                curve.AddKey(key);
            }
        }
    }
    public static class IntArrayExtension
    {
        public static void DefaultIncrementArray(out int[] ints, int length)
        {
            ints = new int[length];
            for (int i = 0; i < length; i++)
            {
                ints[i] = i;
            }
        }
    }
}