using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {
	public GameObject PREFAB;
	public float tickInterval;
	float timeElapsed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		if (timeElapsed > tickInterval)
		{
			timeElapsed = 0;
			Instantiate(PREFAB, transform.position, Quaternion.identity);
		}
	
	}
}
