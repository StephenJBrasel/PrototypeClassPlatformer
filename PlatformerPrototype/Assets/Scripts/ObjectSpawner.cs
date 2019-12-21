using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	[Tooltip("The interval at which GameObject prefab will be spawned. Set to 3 seconds by default.")]
	[SerializeField] private float interval = 3.0f;

	[Tooltip("The Gameobject that will be spawned. It must be located in the Assets/Resources/Prefabs folder.")]
	[SerializeField] private GameObject prefab;

	[Tooltip("If false, will use recursive method to spawn objects. This will cause objects to spawn even if this component is disabled.")]
	[SerializeField] private bool useUpdate = true;

	[Header("Outdated")]
	[Tooltip("The X position where the object will spawn. Uses the gameObject's transform if left at 3.402823e+38.")]
	[SerializeField] private float posx = float.MaxValue;

	[Tooltip("The Y position where the object will spawn. Uses the gameObject's transform if left at 3.402823e+38.")]
	[SerializeField] private float posy = float.MaxValue;

	private float timeSinceLast = 0f;

	private void Awake()
	{
		if (posx == float.MaxValue) posx = transform.position.x;
		if (posy == float.MaxValue) posy = transform.position.y;
	}

	void Start()
    {
		if(!useUpdate)
			StartCoroutine(SpawnObjectRepeat(interval));
    }

	private void Update()
	{
		if(useUpdate && Time.time - timeSinceLast > interval)
		{
			SpawnObjectAfter(0f);
			timeSinceLast = Time.time;
		}
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
