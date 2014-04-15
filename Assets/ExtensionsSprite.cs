using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;
using TransformExtension;
namespace ExtensionsSprite
{

    static class ExtensionsSprite
    {
        public static Vector3 getSpriteSize(this GameObject me)
        {
            var parent = me.transform.parent;
            var extents = me.GetComponent<SpriteRenderer>().sprite.bounds.extents;
            return new Vector3(extents.x * 2, extents.y * 2, 1).mult(me.transform.worldScale() );
            //Debug.Log(s.texture.width + " " + s.texture.height);
            //return new Vector3(s.texture.width * .01f, s.texture.height * .01f, 1)
            //                        .mult(me.transform.worldScale());
        }
        public static Vector3 getSpriteTopLeft(this GameObject me)
        {
            var size = getSpriteSize(me);
            return me.transform.position + new Vector3(size.x * -.5f, size.y * .5f, 0);
        }
        public static Vector3 getSpriteBottomLeft(this GameObject me)
        {
            var size = getSpriteSize(me);
            return me.transform.position + new Vector3(size.x * -.5f, -size.y * .5f, 0);
        }
    }
}