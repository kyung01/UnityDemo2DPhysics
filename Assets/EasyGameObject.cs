using UnityEngine;
using System.Collections;
using Vector3ExtensionMethods;
public class EasyGameObject : MonoBehaviour {
    public static void HelperSetLayersRecursively(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        foreach (Transform child in t)
            HelperSetLayersRecursively(child, layer);
    }
    public static void HelperSetRendererRecursively(Transform t, bool b)
    {
        t.renderer.enabled = b;
        foreach (Transform child in t)
            HelperSetRendererRecursively(child, b);
    }
    public Vector3 RotationWorld
    {
        get
        {
            Transform parent = transform.parent;
            transform.parent = null;
            Vector3 rot = transform.rotation.eulerAngles;
            transform.parent = parent;
            return rot;
        }
    }
    public Vector3 ScaleWorld
    {
        get
        {
            Transform parent = transform.parent;
            transform.parent = null;
            Vector3 rot = transform.localScale;
            transform.parent = parent;
            return rot;
        }
    }

    protected void setLayersRecursively()
    {
        HelperSetLayersRecursively(transform, gameObject.layer);
    }
    
	public Vector3 getPosTopLeft(){
		return transform.position + Vector3.Scale(new Vector3(-1,1,0) ,transform.localScale * .5f);
	}
	public Vector3 getPosBottomLeft(){
		return transform.position - transform.localScale * .5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public GameObject getRoot(){
        return transform.root.gameObject;
	}
	public void setColor(float r, float g, float b,float a = 1.0f){
		renderer.material.color = new Color(r, g, b,a);
	}

}
