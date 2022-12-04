using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviourSingleton<StateManager>
{
    private bool _isStart = false;
    private bool _isEnd = false;
    private bool _isPause = false;
    public bool IsStart
    {
        get => _isStart;
        set
        {
            _isStart = value;
            AssetManager.Instance.musicPlayer.PlayScheduled(AssetManager.Instance.chartData.globalData.offset);
            ProgressManager.Instance.StartPlay(default);
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
    public bool IsPlaying => IsStart && !IsPause && !IsEnd;

    /// <summary>
    /// 当程序获得或者失去焦点时候调用
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus)
    {
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
