using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Blophy.Chart;
using Newtonsoft.Json;

public class Public_ControlSpace : MonoBehaviour
{
    public Scrollbar verticalBar;
    public float progressBar;
    public int elementCount;
    public float[] allElementDistance;//所有元素标准的距离（0-1之间的数据）
    public float single => 1f / (elementCount - 1);
    public int currentElementIndex = 0;
    public float currentElement = 0;
    // Update is called once per frame
    private void Start()
    {
        allElementDistance = new float[elementCount];//所有元素标准的距离（0-1之间的数据）
        for (int i = 0; i < elementCount; i++)
        {
            allElementDistance[i] = single * i;
        }
        Send();
    }
    void Update()
    {
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            float[] allElementDistanceWithFinger = new float[elementCount];//手指抬手的时候，所有元素距离当前值的距离（取绝对值）
            for (int i = 0; i < elementCount; i++)
            {
                allElementDistanceWithFinger[i] =
                Mathf.Abs(verticalBar.value - single * i);
            }

            int minValue = 0;
            for (int i = 1; i < allElementDistanceWithFinger.Length; i++)
            {
                minValue = allElementDistanceWithFinger[i] < allElementDistanceWithFinger[i - 1] ? i : minValue;//判断哪个元素距离当前值最小
            }
            currentElement = allElementDistance[minValue];//复制索引值
            //GlobalData.Instance.SelectChapter_CurrentChapter = minValue;
            currentElementIndex = elementCount - 1 - minValue;
            Send();
            StartCoroutine(Lerp());
        }

    }
    public virtual void Send() { }
    IEnumerator Lerp()
    {
        while (Mathf.Abs(verticalBar.value - currentElement) > .0001f)
        {
            verticalBar.value = Mathf.Lerp(verticalBar.value, currentElement, .1f);
            yield return new WaitForEndOfFrame();
        }
    }
}
