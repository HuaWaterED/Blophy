using Blophy.Chart;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm
{
    /// <summary>
    /// 二分查找算法
    /// 这里用的是左开右开的算法
    /// </summary>
    /// <param name="list">给我一个列表</param>
    /// <param name="target">要查找的数据</param>
    /// <returns>返回下标</returns>
    public static int BinarySearch(Blophy.Chart.Event[] list, float target)
    {
        int l = -1;//左初始化为-1
        int r = list.Length;//右初始化为数量
        int m;//m无默认值
        while (l + 1 != r)//如果l和r的下标没有挨在一起
        {
            m = (l + r) / 2;//将数据除2
            if (list[m].startTime > target)//如果大于我要找的数据
            {
                r = m;//更新右边界
            }
            else//否则
            {
                l = m;//更新左边界
            }
        }
        return l;//返回最终结果
    }
    /// <summary>
    /// 二分查找算法
    /// 这里用的是左开右开的算法
    /// </summary>
    /// <param name="list">给我一个列表</param>
    /// <param name="target">要查找的数据</param>
    /// <returns>返回下标</returns>
    public static int BinarySearch<T>(T[] notes, Predicate<T> match, bool isLeft)
    {
        int l = -1;//左初始化为-1
        int r = notes.Length;//右初始化为数量
        int m;//m无默认值
        while (l + 1 != r)//如果l和r的下标没有挨在一起
        {
            m = (l + r) / 2;//将数据除2
            if (match(notes[m]))//如果大于我要找的数据
            {
                l = m;//更新右边界
            }
            else//否则
            {
                r = m;//更新左边界
            }
        }
        return isLeft switch
        {
            true => l,
            false => r
        };//返回最终结果
    }
    /// <summary>
    /// 二分查找算法
    /// 这里用的是左开右开的算法
    /// </summary>
    /// <param name="list">给我一个列表</param>
    /// <param name="target">要查找的数据</param>
    /// <returns>返回下标</returns>
    public static int BinarySearch<T>(List<T> notes, Predicate<T> match, bool isLeft)
    {
        int l = -1;//左初始化为-1
        int r = notes.Count;//右初始化为数量
        int m;//m无默认值
        while (l + 1 != r)//如果l和r的下标没有挨在一起
        {
            m = (l + r) / 2;//将数据除2
            if (match(notes[m]))//如果大于我要找的数据
            {
                l = m;//更新右边界
            }
            else//否则
            {
                r = m;//更新左边界
            }
        }
        return isLeft switch
        {
            true => l,
            false => r
        };//返回最终结果
    }
    public static int BinaryStrictSearch(Keyframe[] list, float targetTime)
    {
        int l = -1;//左初始化为-1
        int r = list.Length;//右初始化为数量
        int m;//m无默认值
        while (l + 1 != r)//如果l和r的下标没有挨在一起
        {
            m = (l + r) / 2;//将数据除2
            if (list[m].time > targetTime)//如果大于我要找的数据
            {
                r = m;//更新右边界
            }
            else//否则
            {
                l = m;//更新左边界
            }
        }
        if (list.Length == 0) return -1;
        return list[l].time == targetTime ? l : -1;//返回最终结果
    }
}
