using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{
    private IEnumerator Start()
    {
        for (int i = 0; i < AssetManager.Instance.chartData.boxes.Count; i++)
        {
            switch (AssetManager.Instance.chartData.boxes[i].type)
            {
                case Blophy.Chart.BoxType.free:
                    Instantiate(AssetManager.Instance.freeBox, AssetManager.Instance.box).box = AssetManager.Instance.chartData.boxes[i];
                    break;
                case Blophy.Chart.BoxType.square:
                    Instantiate(AssetManager.Instance.squareBox, AssetManager.Instance.box).box = AssetManager.Instance.chartData.boxes[i];
                    break;
                default:
                    Debug.Log("读取到未知方框类型，生成此方框失败");
                    break;
            }
        }

        yield return new WaitForSeconds(1);
        StateManager.Instance.IsStart = true;
    }
}
