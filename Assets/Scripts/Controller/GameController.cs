using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{
    private IEnumerator Start()
    {
        for (int i = 0; i < AssetManager.Instance.chartData.boxes.Count; i++)
        {
            Instantiate(AssetManager.Instance.boxController, AssetManager.Instance.box)
                .SetSortSeed(i * 3)//这里的3是每一层分为三小层，第一层是方框渲染层，第二和三层是音符渲染层，有些音符占用两个渲染层，例如Hold，FullFlick
                .Init(AssetManager.Instance.chartData.boxes[i]);
        }

        yield return new WaitForSeconds(1);
        StateManager.Instance.IsStart = true;
    }
}
