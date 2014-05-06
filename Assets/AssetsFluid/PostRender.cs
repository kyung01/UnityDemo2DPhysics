using UnityEngine;
using System.Collections;

public class PostRender : MonoBehaviour {
	bool isAtoB = true;
	RenderTexture sample;
	public Material mat;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnPostRender()
	{
		// Create a shader that renders white only to alpha channel
		 
		// Draw a quad over the whole screen with the above shader
		GL.PushMatrix();
		GL.LoadOrtho();
		for (var i = 0; i < mat.passCount; ++i)
		{
			this.mat.SetPass(i);
			GL.Begin(GL.QUADS);
			GL.Vertex3(0.0f, 0.0f,0.1f);
			GL.Vertex3(1.0f, 0.0f, 0.1f);
			GL.Vertex3(1.0f, 1.00f, 0.1f);
			GL.Vertex3(0.0f, 1.0f, 0.1f);
			GL.End();
		}
		GL.PopMatrix();
		//if (isAtoB) { camera.targetTexture = b; sample = a; }
		//else { camera.targetTexture = a; sample = b; }
		//isAtoB = !isAtoB;
	}
}
