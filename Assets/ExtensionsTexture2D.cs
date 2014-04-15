﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionsTexture2D
{
    static class ExtensionsTexture2D
    {
        public static Texture2D kCopy(this Texture2D me)
        {
            var t = new Texture2D(me.width, me.height);
            t.SetPixels(me.GetPixels());
            t.Apply();
            return t;
        }
    }
}