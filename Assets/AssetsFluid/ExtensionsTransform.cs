using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionsTransform
{
    static class ExtensionsTransform
    {

        public static Vector3 worldScale(this Transform t)
        {
            Transform parent = t.parent;
            t.parent = null;
            Vector3 rot = t.localScale;
            t.parent = parent;
            return rot;
        }
        static public Vector3 RotationWorld(this Transform t)
        {
            Transform parent = t.parent;
            t.parent = null;
            Vector3 rot = t.rotation.eulerAngles;
            t.parent = parent;
            return rot;
        }
		static public Vector3 getPosTopLeft(this Transform t)
		{
			return t.position + Vector3.Scale(new Vector3(-1, 1, 0), t.localScale * .5f);
		}
		static public Vector3 getPosTopRight(this Transform t)
		{
			return t.position + Vector3.Scale(new Vector3(1, 1, 0), t.localScale * .5f);
		}
		static public Vector3 getPosBottomLeft(this Transform t)
		{
			return t.position - new Vector3(t.localScale.x * .5f, t.localScale.y * .5f, 0);
		}
    }
}
