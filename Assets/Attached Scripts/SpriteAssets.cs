using UnityEngine;
using System.Collections.Generic;

public class SpriteAssets : MonoBehaviour {
    public static SpriteAssets spriteAssets;
    public Sprite hot;
    public Sprite cold;
    public Sprite humid;
    public Sprite dry;

    public Sprite cardAppollo;
    public Sprite cardAphorodite;
    public Sprite cardArtemis;
    public Sprite cardAthena;

    public Sprite summer;
    public Sprite winter;
    public Sprite fall;
    public Sprite spring;

    public List<Sprite> allCardbacks;

    void Awake()
    {
        if (spriteAssets == null)
        {
            DontDestroyOnLoad(gameObject);
            spriteAssets = this;
            allCardbacks = new List<Sprite>();
            allCardbacks.Add(cardAppollo);
            allCardbacks.Add(cardAphorodite);
            allCardbacks.Add(cardArtemis);
            allCardbacks.Add(cardAthena);
        }
        else if (spriteAssets != this)
        {
            Destroy(gameObject);
        }
    }
}
