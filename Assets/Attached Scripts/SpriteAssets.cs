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
    public Sprite cardPoseidon;
    public Sprite cardHades;

    public Sprite summerButton;
    public Sprite winterButton;
    public Sprite fallButton;
    public Sprite springButton;

	public Sprite summerWeatherTable;
	public Sprite winterWeatherTable;
	public Sprite fallWeatherTable;
	public Sprite springWeatherTable;

    public Sprite blankTile;
    public Sprite threePlayerButton;
    public Sprite fourPlayerButton;
    public Sprite fivePlayerButton;
    public Sprite sixPlayerButton;
    public Sprite DITile;

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
            allCardbacks.Add(cardPoseidon);
            allCardbacks.Add(cardHades);

        }
        else if (spriteAssets != this)
        {
            Destroy(gameObject);
        }
    }
}
