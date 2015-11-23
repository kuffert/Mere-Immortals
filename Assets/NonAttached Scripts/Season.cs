using UnityEngine;
using System.Collections.Generic;

// Chris Kuffert 11/12/15
// Abstracted season class. Can't be instantiated, only meant to store the shared lists between each Season subclass.
public abstract class Season
{
    public static List<Vector2> alwaysBadWeatherEffects;
    public static List<Vector2> sometimesGoodWeatherEffects;

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