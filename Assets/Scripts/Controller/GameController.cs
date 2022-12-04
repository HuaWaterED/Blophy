using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{
    private IEnumerator Start()
    {
        for (int i = 0; i < AssetManager.Instance.chartData.boxes.Count; i++)
        {
            Instantiate(AssetManager.Instance.boxController, AssetManager.Instance.box).box = AssetManager.Instance.chartData.boxes[i];
        }

        yield return new WaitForSeconds(1);
        StateManager.Instance.IsStart = true;
    }
}
