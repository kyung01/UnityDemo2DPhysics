using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MovementForward : MonoBehaviour
{
    public float speed;
    void Start()
    {
        float angle = (transform.rotation.eulerAngles.z + 90) * 0.0174f;
        var dir = new Vector2( Mathf.Cos(angle), Mathf.Sin(angle));
        rigidbody2D.velocity += dir * speed;
    }
}