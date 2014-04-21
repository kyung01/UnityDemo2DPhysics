using UnityEngine;
using System.Collections;

public class MissileLauncherBasic : MonoBehaviour {

    public GameObject missile;
    public void EVENT_FIRE()
    {
        Instantiate(missile, transform.position, transform.rotation);

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
