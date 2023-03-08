using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class End_UIManager : MonoBehaviourSingleton<End_UIManager>
{
    public Image background;
    public Image art;
    public TextMeshProUGUI APFC;
    public TextMeshProUGUI score;
    public TextMeshProUGUI perfect;
    public TextMeshProUGUI good;
    public TextMeshProUGUI bad;
    public TextMeshProUGUI miss;
    public TextMeshProUGUI maxCombo;
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI musicName;
    public TextMeshProUGUI level;
    private void Start()
    {
        background.sprite = GlobalData.Instance.currentCP;
        art.sprite = GlobalData.Instance.currentCPH;
        Texture2D cphTexture = GlobalData.Instance.currentCPH.texture;
        art.sprite = Sprite.Create(cphTexture, new Rect((cphTexture.width - cphTexture.height) / 2, 0, cphTexture.height, cphTexture.height), new Vector2(0.5f, 0.5f));
        if (GlobalData.Instance.score.Bad == 0 && GlobalData.Instance.score.Miss == 0 && GlobalData.Instance.score.Good == 0)
        {
            APFC.text = "AllPerfect";
        }
        else if (GlobalData.Instance.score.Bad == 0 && GlobalData.Instance.score.Miss == 0)
        {
            APFC.text = "FullCombo";
        }
        else
        {
            APFC.text = "";
        }
        score.text = $"{(int)GlobalData.Instance.score.Score:D7}";
        perfect.text = $"{GlobalData.Instance.score.Perfect}";
        good.text = $"{GlobalData.Instance.score.Good}";
        bad.text = $"{GlobalData.Instance.score.Bad}";
        miss.text = $"{GlobalData.Instance.score.Miss}";
        maxCombo.text = $"Max Combo: {GlobalData.Instance.score.maxCombo}";
        accuracy.text = $"Accuracy: {GlobalData.Instance.score.Accuracy * 100f:F2}%";
        musicName.text = $"{GlobalData.Instance.chartData.metaData.musicName}";
        level.text = $"{GlobalData.Instance.chartData.metaData.chartLevel}";
    }
}
