using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BehaviorReplaceIfHit : MonoBehaviour
{
    public GameObject OBJ_REPLACE;
    void OnCollisionEnter2D(Collision2D c)
    {
        Instantiate(OBJ_REPLACE, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }
}