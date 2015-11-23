﻿using UnityEngine;
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
    public TextMesh DebugText;
    public TextMesh commitButton;
	public TextMesh weatherText;

    // These are private and game-specific. They should not be visible outside of this class.
    private Season season;
	private List<Player> players = new List<Player>();

    // These are the card back images 

    // list of card gameobjects that updates during each players turn
    private List<GameObject> displayedCards;

	private List<GameObject> displayedPlayedCards;

    // These are game-specific, but the rendering scripts will need access to these by using getters.
    private Vector2 currentWeather;
    private Sprite currentWeatherSprite;
    private Player currentMoveOwner;

    // number of people who have played cards on a turn
    private int movesPlayed;
    // number of people who have had a turn during a season
    private int turnsPlayed;

    // These are private and only exist for code redability
    private Vector2 neutral = new Vector2(0, 0);

    public Vector2 getCurrentWeather() { return currentWeather; }
    public Sprite getCurrentWeatherSprite() { return currentWeatherSprite; }
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

    void Start() {
        season = new Summer();
		weatherText.text = season.seasonName;
        currentWeather = new Vector2(0, 0);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeather);

        for (int i = 0; i < numberOfPlayers; i++) {
            players.Add(new Player("Player " + (i + 1), CardAssets.cardAssets.allCardbacks[i], startingFavor));
        }

        DebugText.text = currentWeather.x + ", " + currentWeather.y;
        currentMoveOwner = players[0];
        displayedCards = players[0].showCards();
    }
	
	void Update () {

        if (checkLose())
        {
            Debug.Log("Sweet scrotum boi");
            Application.Quit();
        }

        if (checkWin())
        {
            Debug.Log("Suck me off");
            Application.Quit();
        }

        // Show the current weather vector
        DebugText.text = "(" + currentWeather.x + ", " + currentWeather.y + ") (" + currentMoveOwner.characterName + ") (" + currentMoveOwner.favor + " favor" + ") (" + townHealth + " Town Health)";

        // Update card positions
        currentMoveOwner.updateCardPositions(displayedCards);


        // Check for cards being clicked
        if (Input.GetMouseButtonDown(0))
        {
            checkForCardClicks();
            checkCommit();
        }
    }
    
    // Happens when a player commits his cards. Changes the current player to the
    // next player, commits their cards, increments the number of people that have played,
    // and changes the card objects being shown in the game world.
    // If the max number of players have played, The season will change,  
    void commitMove()
    { 
        currentMoveOwner.commitCards();

        // increments the number of people who have played during this TURN, NOT DURING THE ROUND
        movesPlayed++;

        // If all players have played, enact divine intervention
        if (movesPlayed == numberOfPlayers)
        {
            movesPlayed = 0;
            turnsPlayed++;
            calculateNewCurrentWeather();
            calculateDivineInterventionEffect();
            // Current turn owner should not change.
        }
        // Otherwise move the current player to the next index, looping around if needed.
        else
        {
            int indexOfMoveOwner = players.IndexOf(currentMoveOwner);
            if (indexOfMoveOwner + 1 < players.Count)
            {
                currentMoveOwner = players[indexOfMoveOwner + 1];
            }
            else currentMoveOwner = players[0];
        }

        // If all players have had a turn, change the season
        if (turnsPlayed == numberOfPlayers)
        {
            turnsPlayed = 0;
            changeSeason();
            wipeAllPlayedCards();
            allPlayersRedraw();
        }
        
        clearDisplayedCards();
        displayedCards = currentMoveOwner.showCards();
		displayedPlayedCards = currentMoveOwner.showPlayedCards();
		//currentMoveOwner.updateCardPositions(displayedPlayedCards);
        Debug.Log("Current move owner: " + currentMoveOwner + " | moves played: " + movesPlayed + " | turns played " + turnsPlayed);
    }

    // Checks to see if, upon a click, the click should move a card.
    void checkForCardClicks()
    {
        for (int i = 0; i < displayedCards.Count; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (displayedCards[i].GetComponent<BoxCollider>().Raycast(ray, out hit, 100)) {
                currentMoveOwner.hand[i].isSelected = !currentMoveOwner.hand[i].isSelected;
                Debug.Log("Kiss me lips");
            }
        }
    }

    // Checks if the player pressed commit
    void checkCommit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (commitButton.GetComponent<BoxCollider>().Raycast(ray, out hit, 100))
        {
            commitMove();
        }
    }

	public void changeSeason()
	{
		if (season.seasonName == "Summer")
		{
			season = new Fall();
		}
		else if (season.seasonName == "Fall")
		{
			season = new Winter();
		}
		else if (season.seasonName == "Winter")
		{
			season = new Spring();
		}
		else if (season.seasonName == "Spring")
		{
			season = new Summer();
		}
		weatherText.text = season.seasonName;
		Debug.Log("Season changed to "+ season.seasonName);
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
    private void calculateDivineInterventionEffect() {
        
        if (currentWeather.Equals(neutral))
        {
            return;
        }

        if (season.getAlwaysBadWeatherEffects().Contains(currentWeather))
        {
            townHealth += alwaysBadHealthEffect;
            currentMoveOwner.adjustFavor(alwaysBadFavorEffect);
        }
        
        else if (season.getSometimesGoodWeatherEffects().Contains(currentWeather))
        {
            adjustAllPlayersFavor(someTimesGoodFavorEffect);
        }

        else
        {
            townHealth += sometimesBadHealthEffect;
            currentMoveOwner.adjustFavor(sometimesBadFavorEffect);
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

    // Checks if the players have lost
    private bool checkLose()
    {
        return townHealth <= 0;
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

    private void clearDisplayedCards()
    {
        foreach(GameObject cardObject in displayedCards)
        {
            Destroy(cardObject);
        }
    }
}
