using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float delayUntilSceneTransition = 3.0f;
    [SerializeField] private string nextSceneName;

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator GoToNextSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayUntilSceneTransition);
        if (nextSceneName != null) GoToNextScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(GoToNextSceneAfterDelay());
    }
}
