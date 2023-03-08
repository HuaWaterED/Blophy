using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public TextMeshProUGUI musicName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI debugText;//用于显示debugText的UI
    string debugTextString = "";//需要显示的字符
    public string DebugTextString//通过这个来增加DebugText文本
    {
        get => debugTextString;//获取的话直接返回
        set
        {
            debugTextString += $"{value}\n";//字符串拼接
            debugText.text = debugTextString;//赋值
        }
    }
    public TextMeshProUGUI combo;
    public TextMeshProUGUI score;
    public void ChangeComboAndScoreText(int rawCombo, float rawScore)
    {
        combo.text = $"{rawCombo}";
        score.text = $"{(int)rawScore:D7}";
    }
    private void Update()
    {
        debugTextString = "";//每帧清空字符串本身
    }
}
