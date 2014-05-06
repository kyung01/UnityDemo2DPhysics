using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsTransform;

public class Grid : MonoBehaviour {
	public static GridManager MANAGER_GRID;
	public List<KParticle> kList;
	
	Vector3 cornerBottomLeft, cornerTopRight;
	void Awake()
	{
		kList = new List<KParticle>();
	}
	void Start () {
	
	}
	public void init()
	{
		//Debug.Log("me START now " + transform.worldScale());
		var p = transform.parent;
		transform.parent = null;
		cornerBottomLeft = transform.getPosBottomLeft();
		cornerTopRight = transform.getPosTopRight();

		transform.parent = p;

	}
	void EVENT_PARTICLE_OUT(KParticle p)
	{
		MANAGER_GRID.register(p);
	}
	void UpdateInteraction(KParticle a, KParticle b)
	{
		a.applyRepel(b,Time.fixedDeltaTime);
		b.applyRepel(a, Time.fixedDeltaTime);
	}
	bool helperIsOut(KParticle p)
	{
		var pos = p.transform.position;
		return pos.x < cornerBottomLeft.x || pos.x > cornerTopRight.x ||
				pos.y < cornerBottomLeft.y || pos.y > cornerTopRight.y;
	}
	void FixedUpdate () {
		/**
		for (int i = kList.Count - 1; i >= 0; i--)
			kList[i].calculatePressure(kList);
		for (int i = 0; i < kList.Count ; i++) for (int j = i + 1; j < kList.Count ; j++)
			{
				UpdateInteraction(kList[i], kList[j]);
			}
		 * 
		 * **/
		for (int i = kList.Count -1 ; i >=0 ; i--)
		{
			var e = kList[i];
			if (helperIsOut(e))
			{
				EVENT_PARTICLE_OUT(e);
				kList.Remove(e);
				renderer.material.color = new Color(kList.Count * .1f, 0, 0, 1);

			}
		}
	}

	public void register(KParticle p)
	{
		kList.Add(p);
		p.myGrid = this;
		renderer.material.color = new Color(kList.Count * .1f,0,0,1);
	
	}
	public void unRegister(KParticle p)
	{
		kList.Remove(p);
	}
}
