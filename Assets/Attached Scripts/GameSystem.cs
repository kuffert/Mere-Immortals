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
    private Vector2 currentWeather;
    private Sprite currentWeatherSprite;
	private List<Player> players = new List<Player>();
    private Player playerWhosTurnItIs;
    private int numberOfPlayersWhoHavePlayedThisSeason;
    private Player playerWhoIsPlayingCards;
    private int numberOfPlayersWhoHavePlayedThisDIPhase;
    private bool divineInterventionPhaseActive;

    // These are private and only exist for code redability
    private Vector2 neutral = new Vector2(0, 0);
    
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
        // 2. add those players to the list: "players"
        // 3. set the first player on the list to be "playerWhosTurnItIs"
		for (int i = 0; i < numberOfPlayers; i++) {
			players.Add (new Player("Player " + (i + 1)));
			print (players[i].characterName);
			for (int j = 0; j < players[i].hand.Count; j++){
				print (players[i].hand[j]);
			}
		}
		playerWhosTurnItIs = players[0];
        numberOfPlayersWhoHavePlayedThisSeason = 0;
        playerWhoIsPlayingCards = players[1];
        numberOfPlayersWhoHavePlayedThisDIPhase = 0;
        divineInterventionPhaseActive = false;
    }
	
	void Update () {

        // 1. Checks win condition
        if (checkWin())
        {
            // Do the winning thing here
        }

        // 2. Players redraw cards if all players have gone. (in the final version, if the season changes. For now though, just if all players have gone)
        if (numberOfPlayers == numberOfPlayersWhoHavePlayedThisSeason)
        {
            allPlayersRedraw();
            numberOfPlayersWhoHavePlayedThisSeason = 0;
        }

        // 3. Display the playerWhoIsPlayingCards cards.

        //[loop while doing this] 
        // 4. If a card is tapped, set it as "selected," add that card to their list of played cards when they press "done"

        // 5. Once they've pressed the done button, increment the numberOfPlayersWhoHavePlayedThisDIPhase
        // 6. If that number == number of players, increment numberOfPlayerwhoHavePlayedThisSeason, set DI phase == true
        // then, call setNextPlayerWhoIsPlayingCards
        numberOfPlayersWhoHavePlayedThisDIPhase++;
        if (numberOfPlayersWhoHavePlayedThisDIPhase == numberOfPlayers)
        {
            numberOfPlayersWhoHavePlayedThisDIPhase = 0;
            numberOfPlayersWhoHavePlayedThisSeason++;
            setNextPlayerWhosTurnItIs();
            divineInterventionPhaseActive = true;
        }
        setNextPlayerWhoIsPlayingCards();

        // 7. If DI phase is true
        if (divineInterventionPhaseActive)
        {
            divineInterventionPhaseActive = false;
            // Do the DI Stuff here
        }

	}

    // Once all players have played their cards, this function will calculate the new current weather 
    // based off of the cards the player's played.
    private void calculateNewCurrentWeather() {

        // 1. Loop through all players, call their function that calculates the total of their cards
        // 2. Add the currentWeather to the outcome
        // 3. Trim it down to the grid's size (2,2) maximum
        // 4. Set the current weather = the new value
        // 5. Set the current weather sprite = the new current weather's corresponding sprite.

    }

    // Once the new weather is calulated, this function will determine the effect on the town based on the
    // current weather and season.

    private void determineDivineInterventionEffect() {

        // Check if its (0,0) first. If it is, do nothing.

        // 1. Check if the current weather is on the current season's list of Always Bad weather effects
        // ----> If it is:
        //                  - decrement the town's health by the alwaysBadHealthEffect field
        //                  - decrement the current player's favor by the alwaysBadFavorEffect field

        // 2. If it isn't, check if the current weather is on the current season's  list of Sometimes Good effects
        // ----> If it is:
        //                  - leave the town's health alone
        //                  - add the someTimesGoodFavorEffect to all player's favor

        // ----> If it is not on either of these lists:
        //                  - decrement the town health by the sometimesBadHealthEffect field
        //                  - decrement the current player's favor by the sometimesBadFavorEffect

    }

    // Finds the "next" player. Makes sure to cycle through the array of players to find the next.
    private void setNextPlayerWhoIsPlayingCards()
    {
        //1. Get the index of the playerWhoIsPlayingCards

        // If the numberOfPlayersWhoHavePlayedThisDIPhase == numberOfPlayers
        // Set the next player to be

        //2. if the index is less than the length of the list minus one, set the playerWhoIsPlayingCards to be the i+1 player

        //3. If is not, set it to the 0th player.
    }

    // Finds the next player's turn.
    private void setNextPlayerWhosTurnItIs()
    {
        //1. get the index of the playerWhosTurnItIs

        //2. if the number of players played this season is equal to the number of players, set the player who's turn it is to i+2 (check if it loops around)

        // if not...

        //. If the index is less than the length of the list minus one, set the playerWhoIsPlayingCards to be the i+1 player

        //. if not, set it to the 0th player
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

    // All player's redraw cards
    private void allPlayersRedraw()
    {
        foreach(Player player in players)
        {
            player.drawCards();
        }
    }
}
