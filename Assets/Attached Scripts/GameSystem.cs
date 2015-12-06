using UnityEngine;
using System.Collections.Generic;
using System.Collections;


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
	public TextMesh weatherDebugText;
	public TextMesh healthDebugText;
	public TextMesh notifyPlayerDebugText;
    public TextMesh commitButton;
	public GameObject seasonButtonSprite;
	public GameObject weatherTableSprite;
	public GameObject weatherMarker;
	public GameObject LeftPalm;
	public GameObject LeftThumb;
	public GameObject RightPalm;
	public GameObject RightThumb;
    public GameObject CurrentWeatherObject;

    // These are private and game-specific. They should not be visible outside of this class.
    private Season season;
    public List<Player> players;


    // These are the card back images 

    // list of card gameobjects that updates during each players turn
    private List<GameObject> displayedCards;
	private List<GameObject> displayedPlayedCards;

    private List<GameObject> cardsToWipe;

    private List<GameObject> playerTiles;
    private List<GameObject> playerTexts;

    // These are game-specific, but the rendering scripts will need access to these by using getters.
    private Vector2 currentWeatherVector;
    private Sprite currentWeatherSprite;
    private Player currentMoveOwner;
    private int indexOfCurrentDIPlayer;

    // number of people who have played cards on a turn
    private int movesPlayed;
    // number of people who have had a turn during a season
    private int turnsPlayed;

    // These are private and only exist for code redability
    private Vector2 neutral = new Vector2(0, 0);

    public Vector2 getCurrentWeather() { return currentWeatherVector; }
    public Sprite getCurrentWeatherSprite() { return currentWeatherSprite; }
    public Player getCurrentMoveOwner() { return currentMoveOwner; }
    
    void Awake()
    {
        if (gameSystem == null)
        {
            DontDestroyOnLoad(gameObject);
            displayedPlayedCards = new List<GameObject>();
            gameSystem = this;
        }
        else if (gameSystem != this)
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        season = new Summer();
        seasonButtonSprite.GetComponent<SpriteRenderer>().sprite = season.seasonButtonSprite;
		weatherTableSprite.GetComponent<SpriteRenderer> ().sprite = season.seasonWeatherTable;
        currentWeatherVector = new Vector2(0, 0);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeatherVector);

		commitButton.GetComponent<MeshRenderer> ().sortingOrder = 5;
        players = GameInfo.gameInfo.players;
        numberOfPlayers = GameInfo.gameInfo.numberOfPlayers;
        
		weatherDebugText.GetComponent<MeshRenderer> ().sortingOrder = 5;
		healthDebugText.GetComponent<MeshRenderer> ().sortingOrder = 5;
		notifyPlayerDebugText.GetComponent<MeshRenderer> ().sortingOrder = 5;
        
        currentMoveOwner = players[0];
		RightPalm.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.rightHand;
		RightThumb.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.rightThumb;
		LeftPalm.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.leftHand;
		LeftThumb.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.leftThumb;
		displayedCards = players[0].showCards();
        
        generatePlayerTilesAndText();
        indexOfCurrentDIPlayer = setDITile();
    }
	
	void Update () {    
        movePlayerTilesAndText(players.IndexOf(currentMoveOwner));
        CurrentWeatherObject.GetComponent<SpriteRenderer>().sprite = currentWeatherSprite;

        if (checkLose())
        {
            GameInfo.endGameNotification = "Game Over";
			Destroy(this);
			Application.LoadLevel (2);
        }
		Player p = checkWin ();
        if (p != null)
        {
            GameInfo.endGameNotification = "Winner! " + p.characterName;
			Destroy(this);
			Application.LoadLevel (2);
        }

        // Show the current weather vector
		weatherDebugText.text = Weather.weather.findStringByWeatherVector (currentWeatherVector);
		healthDebugText.text = "Town: " + townHealth;
        // Update card positions
        currentMoveOwner.updateCardPositions(displayedCards);


        // Check for cards being clicked
        if (Input.GetMouseButtonDown(0))
        {
            checkForCardClicks();
            checkCommit();
        }
    }
	//put the weather marker at the location of currentWeatherVector
    void resetWeatherMarker(Vector2 weatherPrediciton) {
		weatherMarker.transform.position = new Vector2 ((float)-.21 + (float)(weatherPrediciton.x * 1.71), (float).79 + (float)(weatherPrediciton.y * 1.28));
	}
    // Happens when a player commits his cards. Changes the current player to the
    // next player, commits their cards, increments the number of people that have played,
    // and changes the card objects being shown in the game world.
    // If the max number of players have played, The season will change
    void commitMove()
    {
		notifyPlayerDebugText.text = "";
        currentMoveOwner.commitCards();
        currentMoveOwner.showPlayedCards(displayedPlayedCards, players.IndexOf(currentMoveOwner));
        resetWeatherMarker(currentWeatherVector);

        // increments the number of people who have played during this TURN, NOT DURING THE ROUND
        movesPlayed++;

        //last player in the round, gets to save the day! 
		if (movesPlayed == numberOfPlayers - 1) {
            resetWeatherMarker(predictCurrentWeather());
            clearDisplayedCards(displayedPlayedCards);
            foreach (Player p in players)
            {
                p.showPlayedCardsFaceUp(displayedPlayedCards, players.IndexOf(p));
            }
			notifyPlayerDebugText.text = "Divine Intervention!!";
        }

		// If all players have played, enact divine intervention
        if (movesPlayed == numberOfPlayers)
        {   
            movesPlayed = 0;
            turnsPlayed++;
            calculateNewCurrentWeather();
			resetWeatherMarker(currentWeatherVector);
            calculateDivineInterventionEffect();
            clearDisplayedCards(displayedPlayedCards);
            wipeMeUp();
            indexOfCurrentDIPlayer = setDITile();
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
        
        clearDisplayedCards(displayedCards);
        displayedCards = currentMoveOwner.showCards();
		RightPalm.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.rightHand;
		RightThumb.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.rightThumb;
		LeftPalm.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.leftHand;
		LeftThumb.GetComponent<SpriteRenderer>().sprite = currentMoveOwner.leftThumb;

        if (displayedCards.Count > 0)
        {
            RightPalm.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(
                Camera.main.WorldToViewportPoint(displayedCards[displayedCards.Count - 1].transform.position).x + .075f,
                .1f, 10f));

            RightThumb.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(
                Camera.main.WorldToViewportPoint(displayedCards[displayedCards.Count - 1].transform.position).x + .075f,
                .16f, 10f));
        }
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
				//move the weather marker
				moveWeatherMarker(currentMoveOwner.hand[i], currentMoveOwner.hand[i].isSelected);
            }
        }
    }
	//moves the weather marker when player selects or deselects a card in hand
	void moveWeatherMarker (Card c ,bool selected){
		if (selected) {
			weatherMarker.transform.Translate ((float)(c.effect.x * 1.71), (float)(c.effect.y * 1.28), 0);
		} else {
			weatherMarker.transform.Translate ((float)(c.effect.x * -1.71), (float)(c.effect.y * -1.28), 0);
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

    // Switches to current season to be the one following the current.
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

		seasonButtonSprite.GetComponent<SpriteRenderer>().sprite = season.seasonButtonSprite;
		weatherTableSprite.GetComponent<SpriteRenderer> ().sprite = season.seasonWeatherTable;
		Debug.Log("Season changed to "+ season.seasonName);
	}

    // Returns a vector what the new current weather will be, should the DI player not play cards
    private Vector2 predictCurrentWeather()
    {
        Vector2 cummulativeTotalOfPlayedCards = calculateTotalCardEffect();
        return trimCummulativeVectorToWeatherGrid(cummulativeTotalOfPlayedCards + currentWeatherVector);
    }
    
    // Once all players have played their cards, this function will calculate the new current weather 
    // based off of the cards the player's played.
    private void calculateNewCurrentWeather() {
        // First, total all of the players cards (not the DI player) and trim them
        Vector2 totalEffect = trimCummulativeVectorToWeatherGrid(calculateTotalCardEffect());
        // Then add them to the current weather and trim
        totalEffect = trimCummulativeVectorToWeatherGrid(totalEffect + currentWeatherVector);
        // Then, add the last player's total effect
        totalEffect += players[indexOfCurrentDIPlayer].calculateEffectOfPlayedCards();
        // Then trim it again
        currentWeatherVector = trimCummulativeVectorToWeatherGrid(totalEffect);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeatherVector);
        Debug.Log(currentWeatherSprite);
    }

    // Calculates the total effect of all cards played by all players except the current DI player
    private Vector3 calculateTotalCardEffect()
    {
        Vector2 cummulativeTotalOfPlayedCards = new Vector2(0, 0);
        for (int i = 0; i < players.Count; i++)
        {
            if (i != indexOfCurrentDIPlayer)
            {
                cummulativeTotalOfPlayedCards += players[i].calculateEffectOfPlayedCards();
            }
        }
        return cummulativeTotalOfPlayedCards;
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
        
        if (currentWeatherVector.Equals(neutral))
        {
            return; // ???
        }

        if (season.getAlwaysBadWeatherEffects().Contains(currentWeatherVector))
        {
            townHealth += alwaysBadHealthEffect;
            currentMoveOwner.adjustFavor(alwaysBadFavorEffect);
        }
        
        else if (season.getSometimesGoodWeatherEffects().Contains(currentWeatherVector))
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
            if (player.playedCards.Count > 0)
            {
                player.adjustFavor(effectAmount);
            }
        }
    }
    
    // Check if any player has won
    private Player checkWin()
    {
        foreach (Player player in players)
        {
            if (player.favor >= favorToWin)
            {

                return player;
            }
        }
        return null;
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

    private void clearDisplayedCards(List<GameObject> cardList)
    {
        foreach(GameObject cardObject in cardList)
        {
            Destroy(cardObject);
        }
    }

    private void wipeMeUp()
    {
        foreach (Player player in players)
        {
            player.wipePlayedCards();
        }
    }

    // Creates the player's favor text and tile images
    private void generatePlayerTilesAndText()
    {
        playerTiles = new List<GameObject>();
        playerTexts = new List<GameObject>();

        float yLoc = .76f;
        float xLoc = .12f;

        foreach(Player player in players)
        {
            GameObject playerTile = new GameObject();
            playerTile.AddComponent<SpriteRenderer>().sprite = SpriteAssets.spriteAssets.blankTile;
            playerTile.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xLoc, yLoc, 10f));
            playerTile.transform.localScale = new Vector3(2f, 1f, 1f);
            playerTiles.Add(playerTile);


            GameObject playerText = new GameObject();
            playerText.AddComponent<TextMesh>().text = player.characterName + ": " + player.favor;
            playerText.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xLoc, yLoc, 10f));
            TextMesh playerTextMesh = playerText.GetComponent<TextMesh>();
            playerTextMesh.anchor = TextAnchor.MiddleCenter;
            playerTextMesh.alignment = TextAlignment.Left;
            playerTextMesh.fontSize = 200;
            playerTextMesh.characterSize = .025f;
            playerText.GetComponent<MeshRenderer>().sortingOrder = 5;
            playerTexts.Add(playerText);

            yLoc -= .09f;
        }
    }

    // Updates the tiles locations to show who's turn it is
    private void movePlayerTilesAndText(int indexOfCurrentPlayer)
    {
        float xLoc = .12f;

        for (int i = 0; i < players.Count; i++)
        {
            float yLoc = Camera.main.WorldToViewportPoint(playerTiles[i].transform.position).y;
            if (i == indexOfCurrentPlayer)
            {
                playerTiles[i].transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xLoc + .02f, yLoc, 10f));
                playerTexts[i].transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xLoc + .02f, yLoc, 10f));
            }
            else
            {
                playerTiles[i].transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xLoc, yLoc, 10f));
                playerTexts[i].transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xLoc, yLoc, 10f));
            }
            playerTexts[i].GetComponent<TextMesh>().text = players[i].characterName + ": " + players[i].favor;
        }
    }

    // Changes the DI player's tile to the DI tile
    private int setDITile()
    {
        int indexOfDIPlayer;
        if (players.IndexOf(currentMoveOwner) == 0)
        {
            indexOfDIPlayer = players.Count - 1;
        }
        else
        {
            indexOfDIPlayer = players.IndexOf(currentMoveOwner) - 1;
        }

        for (int i = 0; i < playerTiles.Count; i++)
        {
            if (i == indexOfDIPlayer)
            {
                playerTiles[i].GetComponent<SpriteRenderer>().sprite = SpriteAssets.spriteAssets.DITile;
            }
            else
            {
                playerTiles[i].GetComponent<SpriteRenderer>().sprite = SpriteAssets.spriteAssets.blankTile;
            }
        }
        return indexOfDIPlayer;
    }
}
