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
        // 1. create x new players, where x = "numberOfPlayers"
        // 2. add those players to the list: "players"
        // 3. set the first player on the list to be "playerWhosTurnItIs"
		for (int i = 0; i < this.numberOfPlayers; i++) {
			this.players.Add (new Player("Player " + (i + 1)));
			print (this.players[i].characterName);
			for (int j = 0; j < this.players[i].hand.Count; j++){
				print (this.players[i].hand[j]);
			}
		}
		this.playerWhosTurnItIs = this.players [0];

	
	}
	
	void Update () {
	
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
}
