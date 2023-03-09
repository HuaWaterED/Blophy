using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay_Continue : Public_Button
{
    public Image[] allTexture;
    public TextMeshProUGUI pauseText;
    public bool isRunning = false;
    private void OnEnable()
    {
        for (int i = 0; i < allTexture.Length; i++)
        {
            allTexture[i].color = Color.white;
        }
        pauseText.alpha = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            if (isRunning) return;
            StartCoroutine(Continue());
        });
    }
    IEnumerator Continue()
    {
        isRunning = true;
        Color color = Color.white;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            for (int i = 0; i < allTexture.Length; i++)
            {
                allTexture[i].color = color;
            }
            pauseText.alpha = color.a;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        ProgressManager.Instance.ContinuePlay();
        SpeckleManager.Instance.enabled = true;
        isRunning = false;
        UIManager.Instance.pauseCanvas.gameObject.SetActive(false);
    }
}
