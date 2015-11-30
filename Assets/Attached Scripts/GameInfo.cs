using UnityEngine;
using System.Collections.Generic;

public class GameInfo : MonoBehaviour {

    public static GameInfo gameInfo;

    public int numberOfPlayers;
    public List<Player> players;

    void Awake()
    {
        if (gameInfo == null)
        {
            DontDestroyOnLoad(gameObject);
            gameInfo = this;
        }
        else if (gameInfo != this)
        {
            Destroy(gameObject);
        }
    }
}
