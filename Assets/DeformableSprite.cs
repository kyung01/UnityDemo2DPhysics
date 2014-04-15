using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector3ExtensionMethods;
using ExtensionsSprite;
using ExtensionsSpriteRenderer;
using ExtensionsTexture2D;

class DeformableSprite : EasyGameObject
{
    public GameObject target;
    SpriteRenderer mySpriteRnederer;

    void Awake()
    {
        mySpriteRnederer = GetComponent<SpriteRenderer>();
        mySpriteRnederer.kNewSprite();
    }
    float helperGetSpriteScale(Sprite s)
    {
        return (s.rect.width) / (s.bounds.extents.x*2.0f);
    }
    public Texture2D applyFilter00(Texture2D texture, Texture2D sample, Vector2 from, Vector2 dis)
    {
        Vector2 sampleSize = new Vector2(sample.width, sample.height);
        for (int i = 0; i < dis.x; i++) for (int j = 0; j < dis.y; j++)
            {
                Vector2 ratio = new Vector2(i, j).divide(dis),
                        at = from + new Vector2(i, j),
                        atTexture = ratio.mult(sampleSize);
                texture.SetPixel((int)at.x, (int)at.y, sample.GetPixel((int)atTexture.x, (int)atTexture.y));
            }
        texture.Apply();
        return texture;
    }
    public Texture2D applyFilter01(Texture2D texture, Texture2D sample, Vector2 from, Vector2 dis)
    {
        Vector2 sampleSize = new Vector2(sample.width, sample.height);
        for (int i = 0; i < dis.x; i++) for (int j = 0; j < dis.y; j++)
            {
                Vector2 ratio = new Vector2(i, j).divide(dis),
                        at = from + new Vector2(i, j),
                        atTexture = ratio.mult(sampleSize);
                if (texture.GetPixel((int)at.x, (int)at.y).a == 0) continue;
                texture.SetPixel((int)at.x, (int)at.y, sample.GetPixel((int)atTexture.x, (int)atTexture.y));
            }
        texture.Apply();
        return texture;
    }
    public void helperApplyTextureAt(Texture2D texture, Vector2 from, Vector2 to, bool isRatio= true)
    {
        if (isRatio) {
            var size = new Vector2(mySpriteRnederer.sprite.texture.width, mySpriteRnederer.sprite.texture.height);
            from = from.mult(size); to = to.mult(size);
        }
        Texture2D textureCopy = applyFilter01(mySpriteRnederer.sprite.texture.kCopy(), texture, from, to - from);

        mySpriteRnederer.sprite = Sprite.Create(textureCopy, new Rect(0, 0, textureCopy.width, textureCopy.height),
            new Vector2(.5f, .5f),helperGetSpriteScale(mySpriteRnederer.sprite));
    }
    public void Apply(GameObject o)
    {
        var s = o.GetComponent<SpriteRenderer>().sprite;
        var world2texture = new Vector3(s.texture.width / ScaleWorld.x, s.texture.height / ScaleWorld.y, 1);
        
        Vector3 size = gameObject.getSpriteSize(),
            from = (o.getSpriteBottomLeft() - gameObject.getSpriteBottomLeft()),
            to = from + o.getSpriteSize();

        helperApplyTextureAt(s.texture,  from.divide(size).XY(), to.divide(size).XY());
        var c = GetComponent<PolygonCollider2D>();
        if (c != null) Destroy(c);
        gameObject.AddComponent<PolygonCollider2D>();
    }
    void OnMouseDown()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.transform.position = pos;
        Debug.Log("OnMouseDown");
        Apply(target);
        target.transform.position = new Vector3(0,-100,0);
    }
}