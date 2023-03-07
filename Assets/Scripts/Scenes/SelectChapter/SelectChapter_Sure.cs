using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectChapter_Sure : MonoBehaviour
{
    public Button sure_button;
    // Start is called before the first frame update
    void Start()
    {
        sure_button.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("SelectMusic", LoadSceneMode.Single).completed +=
            (AsyncOperation obj) => SelectMusic_ControlSpace.Instance.musics = GlobalData.Instance.chapters[GlobalData.Instance.currentChapterIndex].musicPath;
        });
    }
}
