using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SpriteColorModifier : MonoBehaviour
{
    public SpriteRenderer kRender;
    Texture2D helperCopy(Texture2D txture)
    {
        var t = new Texture2D(txture.width, txture.height);
        t.SetPixels(txture.GetPixels());
        return t;
    }
    public void Start(){
        
        Sprite mySprite = Resources.Load("triangleRed", typeof(Sprite)) as Sprite;

        var myTxture = helperCopy(mySprite.texture);
        for(int i = 0 ; i < myTxture.width *.5f; i++)
            for (int j = 0; j < myTxture.height*.5f; j++)
            {
                //Debug.Log(""+i + " " + j);
                var p = myTxture.GetPixel(i,j);
                p.g += 1;
                p.r = 0;
                myTxture.SetPixel(i, j, p );
            }
        myTxture.Apply();
        var spriteNew = Sprite.Create(myTxture, new Rect(0, 0, myTxture.width, myTxture.height), new Vector2(.5f, .5f));

        kRender.sprite = spriteNew;
    }
}