using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Built with the singleton pattern. No access to the constuctor is allowed, but a single reference of the class 
// is available through the weather field.
public class Weather : MonoBehaviour {

    public static Weather weather;

    public Sprite acidRain;
    public Sprite heavyMist;
    public Sprite scorching;
    public Sprite drought;
    public Sprite sandstorm;

    public Sprite hurricane;
    public Sprite warmRain;
    public Sprite warm;
    public Sprite heatWave;
    public Sprite arid;

    public Sprite downpour;
    public Sprite rain;
    public Sprite neutral;
    public Sprite dry;
    public Sprite desert;

    public Sprite hail;
    public Sprite snow;
    public Sprite cooler;
    public Sprite frosting;
    public Sprite icing;

    public Sprite blizzard;
    public Sprite snowStorm;
    public Sprite freezing;
    public Sprite frozen;
    public Sprite iceStorm;

    public Sprite noWeatherFoundSprite;

    private Hashtable weatherVectorSpritePairs;
	private Hashtable weatherVectorStringPairs;

    void Awake() 
    {
        {
            if (weather == null)
            {
                DontDestroyOnLoad(gameObject);
                weather = this;
            }
            else if (weather != this)
            {
                Destroy(gameObject);
            }
        }

        weatherVectorSpritePairs = new Hashtable();
        
        weatherVectorSpritePairs.Add(new Vector2(-2, 2), acidRain);
        weatherVectorSpritePairs.Add(new Vector2(-1, 2), heavyMist);
        weatherVectorSpritePairs.Add(new Vector2(0, 2), scorching);
        weatherVectorSpritePairs.Add(new Vector2(1, 2), drought);
        weatherVectorSpritePairs.Add(new Vector2(2, 2), sandstorm);

        weatherVectorSpritePairs.Add(new Vector2(-2, 1), hurricane);
        weatherVectorSpritePairs.Add(new Vector2(-1, 1), warmRain);
        weatherVectorSpritePairs.Add(new Vector2(0, 1), warm);
        weatherVectorSpritePairs.Add(new Vector2(1, 1), heatWave);
        weatherVectorSpritePairs.Add(new Vector2(2, 1), arid);

        weatherVectorSpritePairs.Add(new Vector2(-2, 0), downpour);
        weatherVectorSpritePairs.Add(new Vector2(-1, 0), rain);
        weatherVectorSpritePairs.Add(new Vector2(0, 0), neutral);
        weatherVectorSpritePairs.Add(new Vector2(1, 0), dry);
        weatherVectorSpritePairs.Add(new Vector2(2, 0), desert);

        weatherVectorSpritePairs.Add(new Vector2(-2, -1), hail);
        weatherVectorSpritePairs.Add(new Vector2(-1, -1), snow);
        weatherVectorSpritePairs.Add(new Vector2(0, -1), cooler);
        weatherVectorSpritePairs.Add(new Vector2(1, -1), frosting);
        weatherVectorSpritePairs.Add(new Vector2(2, -1), icing);

        weatherVectorSpritePairs.Add(new Vector2(-2, -2), blizzard);
        weatherVectorSpritePairs.Add(new Vector2(-1, -2), snowStorm);
        weatherVectorSpritePairs.Add(new Vector2(-0, -2), freezing);
        weatherVectorSpritePairs.Add(new Vector2(1, -2), frozen);
        weatherVectorSpritePairs.Add(new Vector2(2, -2), iceStorm);

		weatherVectorStringPairs = new Hashtable();
		
		weatherVectorStringPairs.Add(new Vector2(-2, 2), "Acid Rain");
		weatherVectorStringPairs.Add(new Vector2(-1, 2), "Heavy Mist");
		weatherVectorStringPairs.Add(new Vector2(0, 2), "Scorching");
		weatherVectorStringPairs.Add(new Vector2(1, 2), "Drought");
		weatherVectorStringPairs.Add(new Vector2(2, 2), "Sandstorm");
		
		weatherVectorStringPairs.Add(new Vector2(-2, 1), "Hurricane");
		weatherVectorStringPairs.Add(new Vector2(-1, 1), "Warm Rain");
		weatherVectorStringPairs.Add(new Vector2(0, 1), "Warm");
		weatherVectorStringPairs.Add(new Vector2(1, 1), "Heat Wave");
		weatherVectorStringPairs.Add(new Vector2(2, 1), "Arid");
		
		weatherVectorStringPairs.Add(new Vector2(-2, 0), "Downpour");
		weatherVectorStringPairs.Add(new Vector2(-1, 0), "Rain");
		weatherVectorStringPairs.Add(new Vector2(0, 0), "Neutral");
		weatherVectorStringPairs.Add(new Vector2(1, 0), "Dry");
		weatherVectorStringPairs.Add(new Vector2(2, 0), "Desert");
		
		weatherVectorStringPairs.Add(new Vector2(-2, -1), "Hail");
		weatherVectorStringPairs.Add(new Vector2(-1, -1), "Snow");
		weatherVectorStringPairs.Add(new Vector2(0, -1), "Cooler");
		weatherVectorStringPairs.Add(new Vector2(1, -1), "Frosting");
		weatherVectorStringPairs.Add(new Vector2(2, -1), "Icing");
		
		weatherVectorStringPairs.Add(new Vector2(-2, -2), "Blizzard");
		weatherVectorStringPairs.Add(new Vector2(-1, -2), "Snow Storm");
		weatherVectorStringPairs.Add(new Vector2(-0, -2), "Freezing");
		weatherVectorStringPairs.Add(new Vector2(1, -2), "Frozen");
		weatherVectorStringPairs.Add(new Vector2(2, -2), "Ice Storm");
    }


    public Sprite findSpriteByWeatherVector(Vector2 weatherVector)
    {
        if (weatherVectorSpritePairs.ContainsKey(weatherVector))
        {
            return (Sprite)weatherVectorSpritePairs[weatherVector];
        }
        else
            Debug.Log("No Weather found for weather vector: " + weatherVector);
            return noWeatherFoundSprite;
    }

	public string findStringByWeatherVector(Vector2 weatherVector)
	{
		if (weatherVectorStringPairs.ContainsKey(weatherVector))
		{
			return (string)weatherVectorStringPairs[weatherVector];
		}
		else
			Debug.Log("No Weather found for weather vector: " + weatherVector);
		return "Sorry no pizza for you";
	}
}





