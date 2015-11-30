using UnityEngine;
using System.Collections.Generic;

public abstract class Card {
	public Vector2 effect;
	public Sprite frontImage;
	public Sprite backImage;
    public bool isSelected;
	
	public Sprite hot = SpriteAssets.spriteAssets.hot;
	public Sprite dry = SpriteAssets.spriteAssets.dry;
    public Sprite cold = SpriteAssets.spriteAssets.cold;
    public Sprite humid = SpriteAssets.spriteAssets.humid;
}
