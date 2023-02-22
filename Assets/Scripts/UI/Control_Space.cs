using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GlobalData = Data_Global__TouchToStart;
public class Control_Space : MonoBehaviourSingleton<Control_Space>
{
    public Scrollbar verticalBar;
    public float progressbar;
    public int elementCount;
    float single => 1f / (elementCount - 1);
    float res = 0;
    int currentElement = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Debug.Log("UP");


            float[] allEmementDistanceWithFinger = new float[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                allEmementDistanceWithFinger[i] =
                Mathf.Abs(verticalBar.value - single * i);
            }
            float[] allEmementDistance = new float[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                allEmementDistance[i] = single * i;
            }
            int minvalue = 0;
            for (int i = 1; i < allEmementDistanceWithFinger.Length; i++)
            {
                minvalue = allEmementDistanceWithFinger[i] < allEmementDistanceWithFinger[i - 1] ? i : minvalue;
            }
            res = allEmementDistance[minvalue];
            GlobalData.Instance.SelectChapter_CurrentChapter = minvalue;
            StartCoroutine(LERP());
        }

    }
    IEnumerator LERP()
    {
        while (Mathf.Abs(verticalBar.value - res) > .0001f)
        {
            verticalBar.value = Mathf.Lerp(verticalBar.value, res, .1f);
            yield return new WaitForEndOfFrame();
        }
    }
}
