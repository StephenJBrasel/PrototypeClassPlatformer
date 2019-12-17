using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        audioManager.Play("Background");
    }
}
