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
	public GameObject weatherSprite;
	public GameObject weatherTableSprite;
	public GameObject weatherMarker;
	public GameObject RightPalm;
	public GameObject RightThumb;

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
        weatherSprite.GetComponent<SpriteRenderer>().sprite = season.seasonButtonSprite;
		weatherTableSprite.GetComponent<SpriteRenderer> ().sprite = season.seasonWeatherTable;
        currentWeatherVector = new Vector2(0, 0);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeatherVector);

		commitButton.GetComponent<MeshRenderer> ().sortingOrder = 5;
        players = GameInfo.gameInfo.players;
        Debug.Log(players[0].cardBack);
        numberOfPlayers = GameInfo.gameInfo.numberOfPlayers;
        
		weatherDebugText.GetComponent<MeshRenderer> ().sortingOrder = 5;
		healthDebugText.GetComponent<MeshRenderer> ().sortingOrder = 5;
		notifyPlayerDebugText.GetComponent<MeshRenderer> ().sortingOrder = 5;
        
        currentMoveOwner = players[0];
        displayedCards = players[0].showCards();

        generatePlayerTilesAndText();
    }
	
	void Update () {    
        movePlayerTilesAndText(players.IndexOf(currentMoveOwner));

        if (checkLose())
        {
            Debug.Log("Game Over");
            Application.Quit();
        }

        if (checkWin())
        {
            Debug.Log("Winner!");
            Application.Quit();
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
        //place weather marker back to current weather position
        resetWeatherMarker(currentWeatherVector);

        // increments the number of people who have played during this TURN, NOT DURING THE ROUND
        movesPlayed++;

        //last player in the round, gets to save the day!
		if (movesPlayed == numberOfPlayers - 1) {   
			resetWeatherMarker (predictCurrentWeather());
			notifyPlayerDebugText.text = "Divine Intervention!!";
        }

		// If all players have played, enact divine intervention
        if (movesPlayed == numberOfPlayers)
        {
            movesPlayed = 0;
            turnsPlayed++;
            calculateNewCurrentWeather();
			resetWeatherMarker (currentWeatherVector);
            calculateDivineInterventionEffect();
            clearDisplayedCards(displayedPlayedCards);
            wipeMeUp();

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
        
        clearDisplayedCards(displayedCards);
        displayedCards = currentMoveOwner.showCards();
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
                Debug.Log("Kiss me lips");
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

		weatherSprite.GetComponent<SpriteRenderer>().sprite = season.seasonButtonSprite;
		weatherTableSprite.GetComponent<SpriteRenderer> ().sprite = season.seasonWeatherTable;
		Debug.Log("Season changed to "+ season.seasonName);
	}

    // Returns a vector what the new current weather will be, should the DI player not play cards
    private Vector2 predictCurrentWeather()
    {
        Vector2 cummulativeTotalOfPlayedCards = new Vector2(0, 0);
        foreach (Player player in players)
        {
            cummulativeTotalOfPlayedCards += player.calculateEffectOfPlayedCards();
        }
        return trimCummulativeVectorToWeatherGrid(cummulativeTotalOfPlayedCards + currentWeatherVector);
    }
    
    // Once all players have played their cards, this function will calculate the new current weather 
    // based off of the cards the player's played.
    private void calculateNewCurrentWeather() {

        Vector2 cummulativeTotalOfPlayedCards = new Vector2(0, 0);
        foreach (Player player in players)
        {
            cummulativeTotalOfPlayedCards += player.calculateEffectOfPlayedCards();
        }

		currentWeatherVector =  trimCummulativeVectorToWeatherGrid(cummulativeTotalOfPlayedCards +currentWeatherVector);
        currentWeatherSprite = Weather.weather.findSpriteByWeatherVector(currentWeatherVector);
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
}
