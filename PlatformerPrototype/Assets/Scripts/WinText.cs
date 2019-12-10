using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Win_Text").GetComponent<Text>().enabled = true;
    }


}
