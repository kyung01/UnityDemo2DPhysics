using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class DictionaryTexturesDeform : MonoBehaviour
{
    public List<Texture2D> textures;
    public static List<Color[]> textureColors;
    public static List<Vector2> textureSize;
    public static DictionaryTexturesDeform me;
    void Awake()
    {
        DictionaryTexturesDeform.me = this;
        textureColors = new List<Color[]>();
        textureSize = new List<Vector2>();
        foreach (var v in textures)
        {
            textureColors.Add(v.GetPixels());
            textureSize.Add(new Vector2(v.width,v.height));
        }
    }
}
