using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionsTexture2D;

namespace ExtensionsSpriteRenderer
{

    static class ExtensionsSpriteRenderer
    {
        static float helperGetSpriteScale(Sprite s)
        {
            return (s.rect.width) / (s.bounds.extents.x * 2.0f);
        }
        public static void kNewSprite(this SpriteRenderer s)
        {
            var texture = s.sprite.texture.kCopy();
            s.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(.5f, .5f), helperGetSpriteScale(s.sprite));
        }
    }
}
