using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        audioManager.Play("Win");
        GameObject.Find("Win_Text").GetComponent<Text>().enabled = true;
    }


}
