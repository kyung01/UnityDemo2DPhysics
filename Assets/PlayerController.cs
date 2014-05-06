using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vector3ExtensionMethods;

public class PlayerController : MonoBehaviour
{
	public Camera cam;
    const float SPEED = 10.0f;
    const float SPEED_MAX = 20.0f;
    public List<MissileLauncherBasic> launcherMouseLeft;
    float tickFire = 0;
	// Use this for initialization
	void Start () {

	
	}
    Dictionary<KeyCode, Vector2> inputDir = new Dictionary<KeyCode, Vector2>()
    {
        {KeyCode.W,new Vector2(0,1)},
        {KeyCode.S,new Vector2(0,-1)},
        {KeyCode.A,new Vector2(-1,0)},
        {KeyCode.D,new Vector2(1,0)}
    };
    void inputKeyboardFire()
    {
        tickFire -= Time.deltaTime;
        if (Input.GetMouseButtonUp(0))
        {
            tickFire = 0;
        }
        if (tickFire <= 0 && Input.GetMouseButton(0))
        {
            tickFire = .1f;
            foreach (var v in launcherMouseLeft) v.EVENT_FIRE();
        }
        
    }
    void InputKeyboard()
    {

        Vector2 dir = Vector2.zero;
        foreach (var d in inputDir)
        {
            if (Input.GetKey(d.Key)) dir += d.Value;
        }
        dir = dir.normalized;
        var speed = dir * SPEED;
        rigidbody2D.velocity += speed * Time.deltaTime;
        
    }
    void InputMouse()
    {
		var mouseAt = Camera.main.ScreenToWorldPoint(Input.mousePosition).mult(1, 1, 0);
        var from =      mouseAt- transform.position.mult(1, 1, 0);
		//transform.position = mouseAt;
        transform.rotation =Quaternion.Euler( new Vector3(0,0, -90 + Mathf.Atan2(from.y, from.x) * 180/3.14f));
    }
	// Update is called once per frame
	void Update () {
        InputKeyboard();
        inputKeyboardFire();
        InputMouse();
        if (rigidbody2D.velocity.sqrMagnitude > SPEED_MAX * SPEED_MAX)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * SPEED_MAX;
        }
	
	}
}
