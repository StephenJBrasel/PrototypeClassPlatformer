using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

	[SerializeField] private float interval = 3.0f;
	[SerializeField] private GameObject prefab;
	[SerializeField] private float posx = 5.0f;
	[SerializeField] private float posy = 3.0f;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(SpawnObjectRepeat(interval));
    }

	public void SpawnObjectAfter(float seconds)
	{
		StartCoroutine(SpawnObject(seconds));
	}

	private IEnumerator SpawnObject(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		//spawn object here.
		GameObject rubble = Instantiate(
			(GameObject)Resources.Load($"Prefabs/{prefab.name}"), new Vector3(posx, posy, 0), Quaternion.identity);
	}

	private IEnumerator SpawnObjectRepeat(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		//spawn object here.
		GameObject rubble = Instantiate(
			(GameObject)Resources.Load($"Prefabs/{prefab.name}"), new Vector3(posx, posy, 0), Quaternion.identity);
		StartCoroutine(SpawnObjectRepeat(seconds));
	}
}
