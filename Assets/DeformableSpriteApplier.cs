using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeformableSpriteApplier : MonoBehaviour {
    Dictionary<int, GameObject> dics = new Dictionary<int, GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}
    public bool isCollided(int id, GameObject obj)
    {
        GameObject result;
        
        if (dics.TryGetValue(id, out result)) { return true; }
        dics.Add(id, obj); 
        return false;
    }
    bool isHit = false;
    void OnTriggerEnter2D(Collider2D c)
    {
        isCollided(c.GetInstanceID(), c.gameObject);
        Debug.Log("Enter "+this.gameObject.GetInstanceID() + " " +c.gameObject.GetInstanceID());
        //transform.position = new Vector3(0, -100, 0);
    }
    void OnTriggerStay2D(Collider2D c)
    {
        Debug.Log("STAY");
        foreach(var v in dics){
            var deformable = v.Value.GetComponent<DeformableSprite>();
            if (deformable == null) continue;
            deformable.Apply(gameObject);
            deformable.UpdateMesh();
        }
        Destroy(this.gameObject);
    }
    void OnTriggerExit2D(Collider2D c)
    {
        Debug.Log("EXIT");
        //transform.position = new Vector3(0, -100, 0);
    }
}
