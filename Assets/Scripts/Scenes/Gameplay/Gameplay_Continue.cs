using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_Continue : Public_Button
{
    // Start is called before the first frame update
    void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            ProgressManager.Instance.ContinuePlay();
            SpeckleManager.Instance.enabled = true;
            UIManager.Instance.pauseCanvas.gameObject.SetActive(false);
        });
    }
}
