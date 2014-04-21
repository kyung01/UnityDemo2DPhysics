using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeformableSpriteApplier : MonoBehaviour {
    public bool isUnique = false;

    bool isDead = false;
    int idTexture = 0;
    Dictionary<int, GameObject> dics = new Dictionary<int, GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isDead) GameObject.Destroy(gameObject);
	
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
        //Debug.Log("Enter "+this.gameObject.GetInstanceID() + " " +c.gameObject.GetInstanceID());
        //transform.position = new Vector3(0, -100, 0);
    }
    void OnTriggerStay2D(Collider2D c)
    {
        if (isDead) return;
        foreach(var v in dics){
            if (v.Value == null) continue;
            var deformable = v.Value.GetComponent<DeformableSprite>();
            if (deformable == null) continue;

            if(isUnique){
                var texture = GetComponent<SpriteRenderer>().sprite.texture;
                deformable.Apply(gameObject, texture.GetPixels(),new Vector2( texture.width,texture.height));
            }
            else deformable.Apply(gameObject,
                DictionaryTexturesDeform.textureColors[idTexture],
                DictionaryTexturesDeform.textureSize  [idTexture]);
        }
        isDead = true;
    }
    void OnTriggerExit2D(Collider2D c)
    {
        Debug.Log("EXIT");
        //transform.position = new Vector3(0, -100, 0);
    }
}
