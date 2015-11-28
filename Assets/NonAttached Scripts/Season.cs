using UnityEngine;
using System.Collections.Generic;

// Chris Kuffert 11/12/15
// Abstracted season class. Can't be instantiated, only meant to store the shared lists between each Season subclass.
public abstract class Season
{
    public static List<Vector2> alwaysBadWeatherEffects;
    public static List<Vector2> sometimesGoodWeatherEffects;
    public Sprite seasonButtonSprite;
	public Sprite seasonWeatherTable;
	public string seasonName;

    public List<Vector2> getAlwaysBadWeatherEffects()
    {
        return alwaysBadWeatherEffects;
    }

    public List<Vector2> getSometimesGoodWeatherEffects()
    {
        return sometimesGoodWeatherEffects;
    }

    protected Season()
    {
        alwaysBadWeatherEffects = new List<Vector2>();
        alwaysBadWeatherEffects.Add(new Vector2(-2, 2));
        alwaysBadWeatherEffects.Add(new Vector2(-2, -2));
        alwaysBadWeatherEffects.Add(new Vector2(2, 2));
        alwaysBadWeatherEffects.Add(new Vector2(2, -2));
    }

}

public class Summer : Season
{
    public Summer()
    {
        seasonButtonSprite = SpriteAssets.spriteAssets.summerButton;
		seasonWeatherTable = SpriteAssets.spriteAssets.summerWeatherTable;
		seasonName = "Summer";
        sometimesGoodWeatherEffects = new List<Vector2>();
        sometimesGoodWeatherEffects.Add(new Vector2(1, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(1, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(1, 2));
        sometimesGoodWeatherEffects.Add(new Vector2(0, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(0, 2));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, 2));
    }

}

public class Fall : Season
{
    public Fall()
    {
        seasonButtonSprite = SpriteAssets.spriteAssets.fallButton;
		seasonWeatherTable = SpriteAssets.spriteAssets.fallWeatherTable;
        seasonName = "Fall";
        sometimesGoodWeatherEffects = new List<Vector2>();
        sometimesGoodWeatherEffects.Add(new Vector2(0, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(1, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(1, 2));
        sometimesGoodWeatherEffects.Add(new Vector2(1, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(2, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(0, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(1, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(2, -1));
    }

}

public class Winter : Season
{
    public Winter()
    {
        seasonButtonSprite = SpriteAssets.spriteAssets.winterButton;
		seasonWeatherTable = SpriteAssets.spriteAssets.winterWeatherTable;
        seasonName = "Winter";
        sometimesGoodWeatherEffects = new List<Vector2>();
        sometimesGoodWeatherEffects.Add(new Vector2(-1, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, -2));
        sometimesGoodWeatherEffects.Add(new Vector2(0, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(0, -2));
        sometimesGoodWeatherEffects.Add(new Vector2(1, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(1, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(1, -2));
    }

}

public class Spring : Season
{
    public Spring()
    {
        seasonButtonSprite = SpriteAssets.spriteAssets.springButton;
		seasonWeatherTable = SpriteAssets.spriteAssets.springWeatherTable;
        seasonName = "Spring";
        sometimesGoodWeatherEffects = new List<Vector2>();
        sometimesGoodWeatherEffects.Add(new Vector2(0, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(-2, 1));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(-2, 0));
        sometimesGoodWeatherEffects.Add(new Vector2(0, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(-1, -1));
        sometimesGoodWeatherEffects.Add(new Vector2(-2, -1));
    }

}