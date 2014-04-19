using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Vector3ExtensionMethods;
using ExtensionsSprite;
using ExtensionsSpriteRenderer;
using ExtensionsTexture2D;

class DeformableSprite : EasyGameObject
{
    const int LAYER_DESTROY = 8;
    bool    isNewColors = false,
            isTextureEmpty= false;

    float pixelsToWorld;
    Vector2 textureSize;
    Color[] colors;
    bool[] colorsValid;
    int countColor;
    SpriteRenderer mySpriteRnederer;
    Sprite mySprite;
    Texture2D mySpriteTexture;
    
    void Awake()
    {
        mySpriteRnederer = GetComponent<SpriteRenderer>();
        mySpriteRnederer.kNewSprite();
        mySprite = mySpriteRnederer.sprite;

        pixelsToWorld = helperGetSpriteScale(mySpriteRnederer.sprite);
        colors = mySpriteRnederer.sprite.texture.GetPixels();
        colorsValid =   colors.Select(s => (s.a != 0) ? true : false).ToArray();
        countColor =    colorsValid.Where(s => s == true).Count();

        textureSize = new Vector2(mySpriteRnederer.sprite.texture.width, mySpriteRnederer.sprite.texture.height);
    }
    float helperGetSpriteScale(Sprite s)
    {
        return (s.rect.width) / (s.bounds.extents.x*2.0f);
    }
    void helperSetColorAt(int count, ref Color c)
    {
        if (c.a == 0)
        {
            colorsValid[count] = false;
            countColor--;
            if (countColor <= 500) isTextureEmpty = true;
        }
        //Debug.Log(countColor);
        colors[count] = c;
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
    public void applyFilter01(Texture2D sample, Vector2 from, Vector2 indexFrom, Vector2 indexTo, Vector2 dis)
    {
        Vector2 sampleSize = new Vector2(sample.width, sample.height);
        Color[] sampelColors = sample.GetPixels();
        int countSample = sampelColors.Length,
            countColor = colors.Length;
        //Debug.Log(from + " " + indexFrom + " " + indexTo + " " + dis);
        for (int j = (int)indexFrom.y; j <= indexTo.y; j++) for (int i = (int)indexFrom.x; i <= indexTo.x; i++)
            {
                Vector2 atTexture = new Vector2(i, j).divide(dis).mult(sampleSize);
                int indexX = (int)Mathf.Min(from.x + i, textureSize.x - 1);
                int indexY = (int)Mathf.Min(from.y + j, textureSize.y - 1);
                int index = (int)(indexX + indexY * textureSize.x);
                if (index < 0 || index >= countColor) continue;

                int indexSample = (int)((int)atTexture.x + (int)atTexture.y * (int)sampleSize.x) % countSample;
                if (index < 0 || indexSample < 0 || colors[index].a == 0) continue;
                helperSetColorAt(index, ref sampelColors[indexSample]);
            }
    }
    public void applyFilter02(Texture2D sample, Vector2 from, Vector2 indexFrom, Vector2 indexTo)
    {
        Vector2 sampleSize = new Vector2(sample.width, sample.height);
        Color[] sampelColors = sample.GetPixels();
        int countSample = sampelColors.Length,
            countColor = colors.Length;
        Debug.Log("filter02 : " + from);
            //for (int j = (int)indexFrom.y; j <= indexTo.y; j++) for (int i = (int)indexFrom.x; i <= indexTo.x; i++)
            //    {
            //        int indexX = (int)Mathf.Min(from.x + i, textureSize.x - 1);
            //        int indexY = (int)Mathf.Min(from.y + j, textureSize.y - 1);
            //        if (indexX < 0 || indexY < 0) continue;
            //        int index = (int)(indexX + indexY * textureSize.x);
            //        //Debug.Log(index);
            //        colors[index] = new Color(0, 1, 0, 1);
            //    }

            for (int j = 0; j < textureSize.y; j++)
            {
                colors[ (int) Mathf.Max(0,(textureSize.x * j - 1))] = Color.green;
            }
    }
    public void helperApplyTextureAt(Texture2D texture, Vector2 from, Vector2 to, bool isRatio= true)
    {
        if (isRatio)
        {
            from = new Vector2((int)(from.x * textureSize.x), (int)(from.y * textureSize.y));
            to = new Vector2((int)(to.x * textureSize.x),(int)( to.y * textureSize.y));
        }
        var dis = to - from;

        int iInit   = (int)Mathf.Abs(Mathf.Min(0, from.x)),
            iMax    = (int)(Mathf.Min(textureSize.x, from.x + dis.x) - from.x),
            jInit   = (int)Mathf.Abs(Mathf.Min(0, from.y)),
            jMax    = (int)(Mathf.Min(textureSize.y, from.y + dis.y) - from.y);

        applyFilter01(texture, from, new Vector2(iInit, jInit), new Vector2(iMax, jMax), dis);
    }
    public void Apply(GameObject o)
    {
        var s = o.GetComponent<SpriteRenderer>().sprite;
        var world2texture = new Vector3(s.texture.width / ScaleWorld.x, s.texture.height / ScaleWorld.y, 1);
        
        Vector3 size = gameObject.getSpriteSize(),
            from = (o.getSpriteBottomLeft() - gameObject.getSpriteBottomLeft()),
            to = from + o.getSpriteSize();
        
        helperApplyTextureAt(s.texture,  from.divide(size).XY(), to.divide(size).XY());
        isNewColors = true;
    }
    public void UpdateMesh()
    {
        mySpriteTexture.SetPixels(0, 0, (int)textureSize.x, (int)textureSize.y, colors);
        mySpriteRnederer.sprite.texture.Apply();
        var c = GetComponent<PolygonCollider2D>();
        if (c != null) Destroy(c);
        gameObject.AddComponent<PolygonCollider2D>();
    }
    void Update()
    {
        var o = GetComponent<PolygonCollider2D>();
        //Debug.Log("BEFORE" + o.points.Count() + " " + o.pathCount + " ");
        if (isNewColors) {
            isNewColors = false;
            //colors[0] = new Color(1, 0, 0, 1);
            mySpriteTexture = mySprite.texture;
            //var t = new Thread(delegate() { });
                //var t = new Thread(UpdateMesh);
            //t.Start();
            
            Debug.Log("BEFORE " +o.points.Count() + " " + o.pathCount + " ");
            UpdateMesh();
            Debug.Log("AFTER " + o.points.Count() + " " + o.pathCount + " ");
            Debug.Log("CountClor " + gameObject.name + " " + countColor + " " + isTextureEmpty );
        }
        if(isTextureEmpty) GameObject.Destroy(gameObject);
    }
}