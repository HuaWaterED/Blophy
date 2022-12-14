using Blophy.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Blophy.Extension;
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

    public LineNoteController lineNoteController;

    public AnimationCurve canvasLocalOffset;//这个用来表示的是某个时间，画布的Y轴应该是多少
    public AnimationCurve canvasSpeed;//这个用来表示这根线的所有速度总览

    private Line thisLine;

    public Line ThisLine
    {
        get => thisLine;
        set
        {
            thisLine = value;
            Init();
        }
    }//这跟线的谱面元数据

    /// <summary>
    /// 初始化方法
    /// </summary>
    void Init()
    {
        List<Keyframe> keyframes = GameUtility.CalculatedSpeedCurve(thisLine.speed);//将获得到的Key列表全部赋值
        canvasSpeed = new() { keys = keyframes.ToArray(), preWrapMode = WrapMode.ClampForever, postWrapMode = WrapMode.ClampForever };//把上边获得到的点转换为速度图
        canvasLocalOffset = GameUtility.CalculatedOffsetCurve(canvasSpeed, keyframes);//吧速度图转换为位移图
        lineNoteController.Init(thisLine);
    }
}