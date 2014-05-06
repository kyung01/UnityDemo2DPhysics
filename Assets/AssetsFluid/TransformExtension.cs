
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace TransformExtension
{
    static class TransformExtension
    {
        public static Vector3 worldScale(this Transform me)
        {
            var parent = me.parent;
            me.parent = null;
            var scale = me.localScale;
            me.parent = parent;
            return scale;
        }
    }
}