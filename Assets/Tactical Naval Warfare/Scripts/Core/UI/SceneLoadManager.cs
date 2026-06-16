using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider loadbar;
    [SerializeField] private GameObject loadPanel;

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            loadbar.value = asyncOperation.progress;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
