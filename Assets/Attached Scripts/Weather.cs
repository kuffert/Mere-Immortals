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
}





