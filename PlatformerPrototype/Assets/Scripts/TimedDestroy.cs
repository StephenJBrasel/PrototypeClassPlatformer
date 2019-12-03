using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
	[SerializeField] private float time = 3.0f;

    // Update is called once per frame
    void Start()
    {
		StartCoroutine(SelfDestruct());
    }

	private IEnumerator SelfDestruct()
	{
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}
}
