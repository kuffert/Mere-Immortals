using UnityEngine;
using System.Collections.Generic;

public class Weather {

    public static Weather weather;
    public Sprite noWeatherFoundSprite;
    List<WeatherVectorSpritePair> weatherEffects = new List<WeatherVectorSpritePair>();



    public Sprite findSpriteByWeatherVector(Vector2 weatherVector)
    {
        foreach (WeatherVectorSpritePair pair in weatherEffects) {

            if (pair.getVectorEffect().Equals(weatherVector)) {
                return pair.getWeatherSprite();
            }
         }

        // If it isn't found, something went wrong.
        Debug.Log("No weather effect found for vector: " + weatherVector);
        return noWeatherFoundSprite;
    }

    class WeatherVectorSpritePair {

        Vector2 vectorEffect;
        Sprite weatherSprite;

        WeatherVectorSpritePair(Vector2 ve, Sprite ws)
        {
            this.vectorEffect = ve;
            this.weatherSprite = ws;
        }

        public Vector2 getVectorEffect() { return vectorEffect; }
        public Sprite getWeatherSprite() { return weatherSprite; }
    }
}





