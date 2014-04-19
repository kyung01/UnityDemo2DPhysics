using UnityEngine;
using System.Collections;
using ExtensionsTransform;

public class ManuelAlignObjects : MonoBehaviour {
    public GameObject PREFAB_OBJ;
    
	// Use this for initialization
	void Awake () {
        Transform p = transform.parent;
        Quaternion quat = transform.rotation;
        transform.parent = null;
        transform.rotation = Quaternion.identity;
        Vector2 posScale = new Vector2(PREFAB_OBJ.renderer.bounds.extents.x ,PREFAB_OBJ.renderer.bounds.extents.y )*2.0f;
        Vector3 corner = transform.getPosTopLeft();
        for (float i = 0; i < transform.localScale.x; i += posScale.x) for (float j = 0; j < transform.localScale.y; j += posScale.y)
        {
            var obj = Instantiate(PREFAB_OBJ, corner + new Vector3(posScale.x * .5f + i, posScale .y *- .5f - j, 0), Quaternion.identity);
            obj.name = "OBJ [ " + i +" , " + j+ " ]";
        }
        transform.parent = p;
        transform.rotation = quat;
        GameObject.Destroy(this.gameObject);
	}
}
