using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionsTexture2D
{
    static class ExtensionsTexture2D
    {
        static public Texture2D helperGetNeewTexture(int w, int h, float r = 0, float g = 0, float b = 0, float a = 1, bool isColored = false)
        {
            Texture2D t = new Texture2D(w, h);
            if (!isColored) return t;
            Color[] c = new Color[w * h];
            for (int i = 0; i < w * h; i++)
                c[i] = new Color(r, g, b, a);
            t.SetPixels(c);
            t.Apply();
            return t;
        }
        static public Texture2D kCopy(this Texture2D me)
        {
            var t = new Texture2D(me.width, me.height);
            t.wrapMode = TextureWrapMode.Clamp;
            t.SetPixels(0,0, t.width,t.height,me.GetPixels());
            t.Apply();
            return t;
        }
    }
}
