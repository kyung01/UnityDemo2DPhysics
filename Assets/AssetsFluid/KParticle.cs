using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionsUnityVectors;

public class KParticle : MonoBehaviour
{
	public static int ME_COUNT = 0;
	public static GridManager MANAGER_GRID;
	public Grid myGrid;

	public int ID { get { return id; } }
	
	int id;
	float mass = 1.0f;
	float	disIdeal = .5f,
			disIdeal_SQRT;
	float	forceRepel = 10.0f;
	float	pressure,
			pressureRatio,
			pressureNearRatio;
	float VISCOSITY = .3f;
	float DT = 1f / 60f;
	
	float[] arrayDis;
	Vector2 force;
	Vector3 velo;
	void Awake()
	{
		id = ME_COUNT++;
		disIdeal_SQRT = disIdeal * disIdeal;
		arrayDis = new float[999];
	}
	void calculatePressure(KParticle other, int index)
	{
		var dis = (other.transform.position - transform.position).XY();
		float disMagSq = dis.sqrMagnitude;
		if (disMagSq > disIdeal_SQRT) { arrayDis[index] = float.MaxValue; return; }
		arrayDis[index] = Mathf.Sqrt(disMagSq);
		float ratio = 1 - (arrayDis[index] / disIdeal);
		pressureRatio += ratio * ratio;
		pressureNearRatio += ratio * ratio*ratio *2.0f;
		
	}
	Vector2 helperRandomDir()
	{
		float a = Random.Range(0, 3.14f * 2);
		return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
	}
	void applyPressure(KParticle other, int index, float timeElapsed = .001f)
	{
		if (arrayDis[index] > disIdeal) return;
		var dis = (other.transform.position - transform.position).XY() + new Vector2(.001f, .001f);
		float disMag = arrayDis[index];
		Vector2 dir;
		if (disMag > .01f)
		{
			dir = new Vector2(dis.x / disMag, dis.y / disMag);
		}
		else
		{
			dir = helperRandomDir();
			disMag = .01f;
		}
		float ratio = 1.0f - disMag / disIdeal;
		float factor = ratio * (pressureRatio + pressureNearRatio * ratio) / (disMag); // / (2 * disMag);
		Vector2 d = dis * factor;
		Vector2 veloDiff = other.rigidbody2D.velocity - rigidbody2D.velocity;
		d -= veloDiff * factor * timeElapsed;

		//Debug.Log(pressureRatio + " " + pressureNearRatio + " " + ratio + "  =  " + factor);
		d = dir * factor;
		var vChange = (other.rigidbody2D.velocity - rigidbody2D.velocity) * .5f;
		//Debug.Log(d);
		applyForce(d * -1.0f + vChange);
		other.applyForce(d - vChange);
	}
	public void applyRepel(KParticle other, float timeElapsed)
	{
		var dis = (other.transform.position - transform.position).XY() + new Vector2(.001f, .001f);
		float disMag = dis.magnitude;
		if (disMag > disIdeal) return;

		Vector2 dir;
		if (disMag != 0) dir = new Vector2(dis.x / disMag, dis.y / disMag);
		else
		{
			float angleRan = Random.Range(0, 3.14f * 2);
			dir = new Vector2(Mathf.Cos(angleRan), Mathf.Sin(angleRan));
		}
		float ratio = 1 - disMag / disIdeal;
		float factor = ratio * (pressureRatio + pressureNearRatio * ratio) / (2 * disMag);
		Vector2 d = dis * factor;
		Vector2 veloDiff = other.rigidbody2D.velocity - rigidbody2D.velocity;
		factor = VISCOSITY * ratio * DT;
		d -= veloDiff * factor * timeElapsed;
		//Debug.Log(d);
		applyForce(d * -1.0f);
		other.applyForce(d);
	}

	void applyPressure_5_01(KParticle other, int index, float timeElapsed = .001f)
	{
		if (arrayDis[index] > disIdeal) return;
		var dis = (other.transform.position - transform.position).XY() + new Vector2(.001f, .001f);
		float disMag = arrayDis[index];

		Vector2 dir = (arrayDis[index] > .01f) ?
			new Vector2(dis.x / disMag, dis.y / disMag) :
			helperRandomDir();

		float ratio = 1.0f - disMag / disIdeal;
		float factor = ratio * (pressureRatio + pressureNearRatio * ratio) / (2 * disMag); // / (2 * disMag);
		Vector2 d = dis * factor;
		Vector2 veloDiff = other.rigidbody2D.velocity - rigidbody2D.velocity;
		d -= veloDiff * factor * timeElapsed;

		//Debug.Log(pressureRatio + " " + pressureNearRatio + " " + ratio + "  =  " + factor);
		d = dir * factor;
		var vChange = (other.rigidbody2D.velocity - rigidbody2D.velocity) * ratio;
		//Debug.Log(d);
		applyForce(d * -1.0f + vChange);
		other.applyForce(d - vChange);
	}

	void Start()
	{
		MANAGER_GRID.register(this);
	}
	void Update() { }
	void FixedUpdate()
	{
		var others = myGrid.kList;

		pressureRatio = 0;
		pressureNearRatio = 0;
		for (int i = 0; i < others.Count; i++) calculatePressure(others[i], i);
		for (int i = 0; i < others.Count; i++) applyPressure(others[i], i);
		float x = rigidbody2D.velocity.x, y = rigidbody2D.velocity.y;
		bool isChanged = false;
		if (x > 1f) { x %= 1f; isChanged = true; }
		if (y > 1f) {y %= 1f; isChanged = true;}
		if (isChanged) rigidbody2D.velocity = new Vector2(x, y);
		//rigidbody2D.velocity *= .3f;
	}
	public void applyForce(Vector2 f)
	{
		float x = f.x, y = f.y;
		if (float.IsNaN(x)) x=0;
		if(float.IsNaN(y))  y=0;
		rigidbody2D.AddForce(new Vector2(x,y));
	}
	void helperCalculatePressure(KParticle other)
	{
		var dis = (other.transform.position - transform.position).XY();
		float disMagSq = dis.sqrMagnitude;
		if (disMagSq > disIdeal_SQRT) return;
		float disMag = Mathf.Sqrt(disMagSq);
		float ratio = 1- (disMag / disIdeal);
		pressureRatio += ratio * ratio;
		pressureNearRatio += ratio * ratio * ratio;
	}
	public void calculatePressure(List<KParticle> l)
	{
		pressureRatio = 0;
		for(int i =  0 ; i < l.Count;i++)
			helperCalculatePressure(l[i]);

	}
}
/**
 * 
 * for (int i = kList.Count - 1; i >= 0; i--)
			kList[i].calculatePressure(kList);
		for (int i = 0; i < kList.Count ; i++) for (int j = i + 1; j < kList.Count ; j++)
			{
				UpdateInteraction(kList[i], kList[j]);
			}
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
 * 
 * public void applyRepel(KParticle other, float timeElapsed)
	{
		var dis = (other.transform.position - transform.position).XY();
		if (dis.x < -disIdeal || dis.x > disIdeal || dis.y < -disIdeal || dis.y > disIdeal) return;
		float disMag = dis.magnitude;
		Vector2 dir;
		if (disMag != 0) dir = new Vector2(dis.x/disMag, dis.y/disMag);
		else
		{
			float angleRan = Random.Range(0, 3.14f * 2);
			dir = new Vector2(Mathf.Cos(angleRan), Mathf.Sin(angleRan));
		}
		float ratio = (disIdeal-disMag) / disIdeal;
		float force = forceRepel * ratio * timeElapsed;
		other.applyForce(new Vector2(dir.x * force, dir.y * force));

	}ss
**/