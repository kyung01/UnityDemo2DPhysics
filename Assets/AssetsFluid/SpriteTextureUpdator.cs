using UnityEngine;
using System.Collections;

public class SpriteTextureUpdator : MonoBehaviour {
    public SpriteRenderer sprRender;
    public RenderTexture renTexture;
    Texture2D texture;
	// Use this for initialization
	void Start () {
        texture = new Texture2D(renTexture.width, renTexture.height);
	
	}
	
	// Update is called once per frame
	void Update () {
        RenderTexture.active = null;
	
	}
}
