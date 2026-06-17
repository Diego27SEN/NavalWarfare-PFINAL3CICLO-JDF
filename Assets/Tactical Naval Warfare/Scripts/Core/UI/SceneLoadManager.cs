using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneLoadManager : MonoBehaviour
{
    public int counter = 0;
    [SerializeField] private Slider loadbar;
    [SerializeField] private GameObject loadPanel;

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadYourAsyncScene(sceneIndex));
    }

    IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        
        loadbar.value = 0;
        Debug.Log("Counter: ");

        while (true)
        {
          //  yield return new WaitForSeconds(0.5f);
            counter++;
            Debug.Log("Counter: " + counter);
            float progressTarget = Mathf.Clamp01(counter*0.5f /5f);
            loadbar.value = progressTarget;


            
            if (loadbar.value >= 1) break;
            
            /* SceneManager.LoadScene(sceneIndex);
             yield return null;*/
        }
        yield return new WaitUntil(() => loadbar.value >= 1);
        SceneManager.LoadScene(sceneIndex);
        
        Debug.Log("Counter fin ");
        yield return null;

        /* AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

         asyncLoad.allowSceneActivation = false;

         while (!asyncLoad.isDone)
         {
             float progressTarget = Mathf.Clamp01(asyncLoad.progress / 0.9f);
             loadbar.value = Mathf.MoveTowards(loadbar.value, progressTarget, Time.deltaTime);

             if (loadbar.value >= 1)
             {
                 yield return new WaitForSeconds(1f);

                 asyncLoad.allowSceneActivation = true;
             }

             yield return null;
         }*/

    }
    /*IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            loadbar.value = asyncOperation.progress;
            yield return null;
        }
    }*/

   /* IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progressTarget = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadbar.value = Mathf.MoveTowards(loadbar.value, progressTarget, Time.deltaTime);

            if (loadbar.value >= 1)
            {
                yield return new WaitForSeconds(1f);

                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

    */
    /*
     private int counter = 0;

    void Start()
    {
        // Start the coroutine when the object is initialized
        StartCoroutine(CountUpRoutine());
    }

    IEnumerator CountUpRoutine()
    {
        while (true)
        {
            // Wait for 1 second in real-time before continuing
            yield return new WaitForSeconds(1f);
            
            counter++;
            Debug.Log("Counter: " + counter);
        }
    }
     */
}
