using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleText : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Puzzle_Text").GetComponent<Text>().text = transform.GetComponent<ButtonTrigger>().objectsToTrigger[0].name + " unlocked !";
        GameObject.Find("Puzzle_Text").GetComponent<Text>().enabled = true;
        StartCoroutine(unlockText());
    }

    IEnumerator unlockText()
    {
        yield return new WaitForSeconds(3);
        GameObject.Find("Puzzle_Text").GetComponent<Text>().enabled = false;
    }

}
