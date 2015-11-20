using UnityEngine;
using System.Collections.Generic;

public abstract class Card {
	public Vector2 effect;
	public Sprite frontImage;
	public Sprite backImage;
    public bool isSelected;
	
	public Sprite hot = CardAssets.cardAssets.hot;
	public Sprite dry = CardAssets.cardAssets.dry;
    public Sprite cold = CardAssets.cardAssets.cold;
    public Sprite humid = CardAssets.cardAssets.humid;
}
