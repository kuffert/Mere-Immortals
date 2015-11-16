using UnityEngine;
using System.Collections.Generic;


// Chris Kuffert 11/12/15
public class GameSystem : MonoBehaviour {

    public static GameSystem gameSystem;

    // These parameters should stay visible in the inspector for easy playtesting modifications.
    public int numberOfPlayers;
    public int townHealth;
    public int alwaysBadHealthEffect;
    public int sometimesBadHealthEffect;
    public int alwaysBadFavorEffect;
    public int someTimesGoodFavorEffect;
    public int sometimesBadFavorEffect;
    public int startingFavor;
    public int favorToWin;
    public int numberOfCards;

    // These are private and game-specific. They should not be visible outside of this class.
    private Season season;
	private List<Player> players = new List<Player>();

    // These are game-specific, but the rendering scripts will need access to these by using getters.
    private Vector2 currentWeather;
    private Sprite currentWeatherSprite;
    private Player currentTurnOwner;
    private Player currentMoveOwner;

    // These are private and only exist for code redability
    private Vector2 neutral = new Vector2(0, 0);

    public Vector2 getCurrentWeather() { return currentWeather; }
    public Sprite getCurrentWeatherSprite() { return currentWeatherSprite; }
    public Player getCurrenturnOwner() { return currentTurnOwner; }
    public Player getCurrentMoveOwner() { return currentMoveOwner; }
    
    void Awake()
    {
        if (gameSystem == null)
        {
            DontDestroyOnLoad(gameObject);
            gameSystem = this;
        }
        else if (gameSystem != this)
        {
            Destroy(gameObject);
        }
    }

	void Start () {
        season = new Summer();
        currentWeather = new Vector2(0, 0);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeather);

        // 1. create x new players, where x = "numberOfPlayers"
        // (generate cards for those players too)
        // 2. add those players to the list: "players"
		for (int i = 0; i < numberOfPlayers; i++) {
			players.Add (new Player("Player " + (i + 1)));
			print (players[i].characterName);
			for (int j = 0; j < players[i].hand.Count; j++){
				print (players[i].hand[j]);
			}
		}
    }
	
	void Update () {
        
        if (checkWin())
        {
            // Do the winning thing here
        }

        // Main game loop
        foreach(Player turnOwner in players)
        {
            currentTurnOwner = turnOwner; // Allows access to the current TurnOwner outside of this loop.
            List<Player> DIPhaseList = generateListOfPlayersExcludingCurrentTurnOwner(players.IndexOf(turnOwner));

            foreach(Player moveOwner in DIPhaseList)
            {
                currentMoveOwner = moveOwner; // Allows access to the current MoveOwner outside of the loop.

                /* LEAVE THIS COMMENTED UNTIL THIS BOOLEAN CAN BE CHANGED.
                while (!moveOwner.hasCommittedCards)
                {
                    Debug.Log("Waiting for " + moveOwner.characterName + " to play cards.");
                }
                */
                moveOwner.hasCommittedCards = false;
            }
            /* LEAVE THIS COMMENTED UNTIL THE BOOLEAN CAN BE CHANGED.
            while (!turnOwner.hasCommittedCards)
            {
                Debug.Log("Waiting for " + turnOwner.characterName + " to play cards.");
            }
            */
            calculateNewCurrentWeather();
            determineDivineInterventionEffect();
            wipeAllPlayedCards();
        }
        allPlayersRedraw();
        adjustListOrderingForNextSeason();
        currentTurnOwner = null;
        currentMoveOwner = null;
    }
    
    // Once all players have played their cards, this function will calculate the new current weather 
    // based off of the cards the player's played.
    private void calculateNewCurrentWeather() {

        Vector2 cummulativeTotalOfPlayedCards = new Vector2(0, 0);
        foreach (Player player in players)
        {
            cummulativeTotalOfPlayedCards += player.calculateEffectOfPlayedCards();
        }

        currentWeather = trimCummulativeVectorToWeatherGrid(cummulativeTotalOfPlayedCards);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeather);
    }

    // Trims a given vector down to size so that it fits correctly in the weather grid.
    private Vector2 trimCummulativeVectorToWeatherGrid(Vector2 vectorToBeTrimmed)
    {
        int xMax = 2;
        int yMax = 2;
        int xMin = -2;
        int yMin = -2;

        if (vectorToBeTrimmed.x > xMax) { vectorToBeTrimmed.x = xMax; }
        if (vectorToBeTrimmed.x < xMin) { vectorToBeTrimmed.x = xMin; }
        if (vectorToBeTrimmed.y > yMax) { vectorToBeTrimmed.y = yMax; }
        if (vectorToBeTrimmed.y < yMin) { vectorToBeTrimmed.y = yMin; }

        return vectorToBeTrimmed;
    }

    // Once the new weather is calulated, this function will determine the effect on the town based on the
    // current weather and season.

    private void determineDivineInterventionEffect() {
        
        if (currentWeather.Equals(neutral))
        {
            return;
        }

        if (season.getAlwaysBadWeatherEffects().Contains(currentWeather))
        {
            townHealth -= alwaysBadHealthEffect;
            currentTurnOwner.adjustFavor(alwaysBadFavorEffect);
        }
        
        else if (season.getSometimesGoodWeatherEffects().Contains(currentWeather))
        {
            adjustAllPlayersFavor(someTimesGoodFavorEffect);
        }

        else
        {
            townHealth -= sometimesBadHealthEffect;
            currentTurnOwner.adjustFavor(sometimesBadFavorEffect);
        }
    }

    // Adjusts the entire list of players favor
    private void adjustAllPlayersFavor(int effectAmount)
    {
        foreach (Player player in players)
        {
            player.adjustFavor(effectAmount);
        }
    }
    
    // Check if any player has won
    private bool checkWin()
    {
        foreach (Player player in players)
        {
            if (player.favor >= favorToWin)
            {
                return true;
            }
        }
        return false;
    }

    // generates a a list of players that does not include the current player.
    private List<Player> generateListOfPlayersExcludingCurrentTurnOwner(int indexOfExcludedPlayer)
    {
        List<Player> listOfPlayersExcludingCurrentTurnOwner = new List<Player>();
        for (int i = indexOfExcludedPlayer + 1; i < players.Count; i++)
        {
            listOfPlayersExcludingCurrentTurnOwner.Add(players[i]);
        }

        for (int i = 0; i < indexOfExcludedPlayer; i++)
        {
            listOfPlayersExcludingCurrentTurnOwner.Add(players[i]);
        }
        return listOfPlayersExcludingCurrentTurnOwner;
    }

    // All player's redraw cards
    private void allPlayersRedraw()
    {
        foreach(Player player in players)
        {
            player.drawCards();
        }
    }

    // Wipe all player's list of played cards
    private void wipeAllPlayedCards()
    {
        foreach (Player player in players)
        {
            player.wipePlayedCards();
        }
    }

    // Shifts the list to start with the next person when the season changes.
    // The player who started the last season (index 0) should be moved to the back.
    private void adjustListOrderingForNextSeason()
    {
        Player tempCopyOfFirstPlayer = players[0];
        players.RemoveAt(0);
        players.Add(tempCopyOfFirstPlayer);
    }
}
