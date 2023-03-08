using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviourSingleton<StateManager>
{
    private bool _isStart = false;//已经开始
    private bool _isEnd = false;//已经结束
    private bool _isPause = false;//已经暂停
    public bool IsStart
    {
        get => _isStart;
        set
        {
            if (_isStart) return;//如果已经开始了就直接返回
            _isStart = value;//设置状态为开始
            AssetManager.Instance.musicPlayer.PlayScheduled(AssetManager.Instance.chartData.globalData.offset + GlobalData.Instance.offset);//播放音乐，带上延迟
            ProgressManager.Instance.StartPlay(default);//谱面开始播放
            AssetManager.Instance.box.gameObject.SetActive(true);//激活所有方框

        }
    }
    public bool IsEnd
    {
        get => _isEnd;
        set => _isEnd = value;
    }
    public bool IsPause
    {
        get => _isPause;
        set
        {
            _isPause = value;
            switch (value)
            {
                case true:
                    ProgressManager.Instance.PausePlay();
                    break;
                case false:
                    ProgressManager.Instance.ContinuePlay();
                    break;
            }
        }
    }
    public bool IsPlaying => IsStart && !IsPause && !IsEnd;//正在播放中，判定方法为：已经开始并且没有暂停没有结束

    /// <summary>
    /// 当程序获得或者失去焦点时候调用
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) return;
        switch (focus)
        {
            case true:
                if (IsStart && !IsPlaying) IsPause = false;
                break;
            case false:
                if (IsStart && IsPlaying) IsPause = true;
                break;
        }
    }
}
