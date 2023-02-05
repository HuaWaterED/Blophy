using Blophy.Chart;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    public int tapPerfect;
    public int holdPerfect;
    public int dragPerfect;
    public int flickPerfect;
    public int fullFlickPerfect;
    public int pointPerfect;

    public int tapGood;
    public int holdGood;
    public int pointGood;
    public int tapEarly_good;
    public int holdEarly_good;
    public int pointEarly_good;
    public int tapLate_good;
    public int holdLate_good;
    public int pointLate_good;

    public int tapBad;
    public int tapEarly_bad;
    public int tapLate_bad;

    public int tapMiss;
    public int holdMiss;
    public int dragMiss;
    public int flickMiss;
    public int fullFlickMiss;
    public int pointMiss;


    public int Perfect => tapPerfect + holdPerfect + dragPerfect + flickPerfect + fullFlickPerfect + pointPerfect;

    public int Good => tapGood + holdGood + pointGood;
    public int Early_good => tapEarly_good + holdEarly_good + pointEarly_good;
    public int Late_good => tapLate_good + holdLate_good + pointLate_good;

    public int Bad => tapBad;
    public int Early_bad => tapEarly_bad;
    public int Late_bad => tapLate_bad;

    public int Miss => tapMiss + holdMiss + dragMiss + flickMiss + fullFlickMiss + pointMiss;
    public int combo;
    public int Combo
    {
        get => combo;
        set
        {
            combo = value;
            maxCombo = maxCombo >= combo ? maxCombo : combo;
        }
    }
    public int maxCombo;

    public int tapCount;
    public int holdCount;
    public int dragCount;
    public int flickCount;
    public int fullFlickCount;
    public int pointCount;
    public int NoteCount => tapCount + holdCount + dragCount + flickCount + fullFlickCount + pointCount;
    public float Accuracy => (Perfect + Good * ValueManager.Instance.goodJudgePercent) / NoteCount;
    public float score;
    public float Score => Accuracy * 500000f +
                maxCombo / NoteCount * 150000f +
                35000f / tapCount * tapPerfect + 22750f / tapCount * tapGood +
                15217.39130434783f / holdCount * holdPerfect + 9891.304347826087f / holdCount * holdGood +
                70000 / dragCount * dragPerfect +
                20588.23529411765f / flickCount * flickPerfect +
                11666.66666666667f / fullFlickCount * fullFlickPerfect +
                23333.33333333333f / pointCount * pointPerfect + 15166.66666666667f / pointCount * pointGood;


    public void AddScore(NoteType noteType, NoteJudge noteJudge, bool isEarly)
    {
        switch (noteJudge)
        {
            case NoteJudge.Perfect:
                AddScorePerfect(noteType);
                break;
            case NoteJudge.Good:
                AddScoreGood(noteType, isEarly);
                break;
            case NoteJudge.Bad:
                AddScoreBad(noteType, isEarly);
                break;
            case NoteJudge.Miss:
                AddScoreMiss(noteType);
                break;
        }
    }

    private void AddScoreMiss(NoteType noteType)
    {
        switch (noteType)
        {
            case NoteType.Tap:
                tapMiss++;
                Combo = 0;
                break;
            case NoteType.Hold:
                holdMiss++;
                Combo = 0;
                break;
            case NoteType.Drag:
                dragMiss++;
                Combo = 0;
                break;
            case NoteType.Flick:
                flickMiss++;
                Combo = 0;
                break;
            case NoteType.Point:
                pointMiss++;
                Combo = 0;
                break;
            case NoteType.FullFlickPink:
                fullFlickMiss++;
                Combo = 0;
                break;
            case NoteType.FullFlickBlue:
                fullFlickMiss++;
                Combo = 0;
                break;
            default:
                Debug.LogError($"如果你看到这条消息，请截图发给花水终，这有助于我们改进游戏！\n" +
                    $"分数系统出错！\n" +
                    $"错误点：加分方法出错！" +
                    $"错误类型：小姐判定，没找到音符类型");
                break;
        }
    }

    private void AddScoreBad(NoteType noteType, bool isEarly)
    {
        switch (noteType)
        {
            case NoteType.Tap:
                tapBad++;
                switch (isEarly)
                {
                    case true:
                        tapEarly_bad++;
                        break;
                    case false:
                        tapLate_bad++;
                        break;
                }
                Combo = 0;
                break;
            default:
                Debug.LogError($"如果你看到这条消息，请截图发给花水终，这有助于我们改进游戏！\n" +
                    $"分数系统出错！\n" +
                    $"错误点：加分方法出错！" +
                    $"错误类型：坏判定，没找到音符类型");
                break;
        }
    }

    private void AddScoreGood(NoteType noteType, bool isEarly)
    {
        switch (noteType)
        {
            case NoteType.Tap:
                tapGood++;
                switch (isEarly)
                {
                    case true:
                        tapEarly_good++;
                        break;
                    case false:
                        tapLate_good++;
                        break;
                }
                Combo++;
                break;
            case NoteType.Hold:
                holdGood++;
                switch (isEarly)
                {
                    case true:
                        holdEarly_good++;
                        break;
                    case false:
                        holdLate_good++;
                        break;
                }
                Combo++;
                break;
            case NoteType.Point:
                pointGood++;
                switch (isEarly)
                {
                    case true:
                        pointEarly_good++;
                        break;
                    case false:
                        pointLate_good++;
                        break;
                }
                Combo++;
                break;
            default:
                Debug.LogError($"如果你看到这条消息，请截图发给花水终，这有助于我们改进游戏！\n" +
                    $"分数系统出错！\n" +
                    $"错误点：加分方法出错！" +
                    $"错误类型：好判定，没找到音符类型");
                break;
        }
    }

    private void AddScorePerfect(NoteType noteType)
    {
        switch (noteType)
        {
            case NoteType.Tap:
                tapPerfect++;
                Combo++;
                break;
            case NoteType.Hold:
                holdPerfect++;
                Combo++;
                break;
            case NoteType.Drag:
                dragPerfect++;
                Combo++;
                break;
            case NoteType.Flick:
                flickPerfect++;
                Combo++;
                break;
            case NoteType.Point:
                pointPerfect++;
                Combo++;
                break;
            case NoteType.FullFlickPink:
                fullFlickPerfect++;
                Combo++;
                break;
            case NoteType.FullFlickBlue:
                fullFlickPerfect++;
                Combo++;
                break;
            default:
                Debug.LogError($"如果你看到这条消息，请截图发给花水终，这有助于我们改进游戏！\n" +
                    $"分数系统出错！\n" +
                    $"错误点：加分方法出错！" +
                    $"错误类型：完美判定，没找到音符类型");
                break;
        }
    }
}
