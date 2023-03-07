using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectMusic_UIManager : MonoBehaviourSingleton<SelectMusic_UIManager>
{
    public TextMeshProUGUI userName;
    public TextMeshProUGUI musicName;
    public TextMeshProUGUI musicWriter;
    public TextMeshProUGUI chartWriter;
    public TextMeshProUGUI artWriter;
    public TextMeshProUGUI bestScore;
    public void SelectMusic(string musicName, string musicWriter, string chartWriter, string artWriter)
    {
        this.musicName.text = musicName;
        this.musicWriter.text = musicWriter;
        this.chartWriter.text = chartWriter;
        this.artWriter.text = artWriter;
        //最高分，从存档系统获取
    }
}
