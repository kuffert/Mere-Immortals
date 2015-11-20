using UnityEngine;
using System.Collections.Generic;

public class CardAssets : MonoBehaviour {
    public static CardAssets cardAssets;
    public Sprite hot;
    public Sprite cold;
    public Sprite humid;
    public Sprite dry;

    public Sprite cardAppollo;
    public Sprite cardAphorodite;
    public Sprite cardArtemis;
    public Sprite cardAthena;

    public List<Sprite> allCardbacks;

    void Awake()
    {
        if (cardAssets == null)
        {
            DontDestroyOnLoad(gameObject);
            cardAssets = this;
            allCardbacks = new List<Sprite>();
            allCardbacks.Add(cardAppollo);
            allCardbacks.Add(cardAphorodite);
            allCardbacks.Add(cardArtemis);
            allCardbacks.Add(cardAthena);
        }
        else if (cardAssets != this)
        {
            Destroy(gameObject);
        }
    }
}
