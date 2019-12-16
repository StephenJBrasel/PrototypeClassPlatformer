using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string nextScene;

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
