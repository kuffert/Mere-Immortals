using UnityEngine;
using System.Collections.Generic;

public class GameStartScript : MonoBehaviour {

    List<Sprite> availableCardBacks;
    List<Player> players;

    List<GameObject> numberOfPlayersGameObjects;
    List<GameObject> availableCardBacksGameObjects;

    public GameObject infoText;
    public GameObject startText;

    bool selectNumberOfPlayersPhase;
    int numberOfPlayersChosen;
    bool selectCardBackingPhase;
    bool playerSelectedCardBack;
    bool startGamePhase;

    int numberOfPlayers;
    int playersWhoHaveSelectedCardBacks;

	// Use this for initialization
	void Start () {
        numberOfPlayersGameObjects = new List<GameObject>();
        availableCardBacksGameObjects = new List<GameObject>();
        players = new List<Player>();
        numberOfPlayers = 0;
        numberOfPlayersChosen = 0;
        availableCardBacks = SpriteAssets.spriteAssets.allCardbacks;
        startGamePhase = false;
        selectNumberOfPlayersPhase = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (startGamePhase)
        {
            infoText.GetComponent<TextMesh>().text = "Press Start to Begin!";
            startText.GetComponent<TextMesh>().text = "START";
            checkStartPressed();
        }

        if (numberOfPlayersChosen < numberOfPlayers && !startGamePhase)
        {
            checkCardBackSelected();
        }

        if (selectCardBackingPhase && !startGamePhase)
        {
            infoText.GetComponent<TextMesh>().text = "Player " + (numberOfPlayersChosen + 1) + " Choose your God!";
            showCardBackOptions();
            selectCardBackingPhase = false;
        }

        if (numberOfPlayers == 0 && !startGamePhase)
        {
            checkNumberOfPlayersSelected();
        }

        if (selectNumberOfPlayersPhase && !startGamePhase)
        {
            infoText.GetComponent<TextMesh>().text = "Choose the Number of Players";
            showNumberOfPlayerButtons();
            selectNumberOfPlayersPhase = false;
        }
    }


    void showNumberOfPlayerButtons()
    {
        GameObject fourPlayersButton = new GameObject();
        fourPlayersButton.AddComponent<SpriteRenderer>();
        fourPlayersButton.GetComponent<SpriteRenderer>().sprite = SpriteAssets.spriteAssets.fourPlayerButton;
        fourPlayersButton.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.25f, .6f, 10f));
        fourPlayersButton.AddComponent<BoxCollider>();

        GameObject fivePlayersButton = new GameObject();
        fivePlayersButton.AddComponent<SpriteRenderer>();
        fivePlayersButton.GetComponent<SpriteRenderer>().sprite = SpriteAssets.spriteAssets.fivePlayerButton;
        fivePlayersButton.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .6f, 10f));
        fivePlayersButton.AddComponent<BoxCollider>();

        GameObject sixPlayersButton = new GameObject();
        sixPlayersButton.AddComponent<SpriteRenderer>();
        sixPlayersButton.GetComponent<SpriteRenderer>().sprite = SpriteAssets.spriteAssets.sixPlayerButton;
        sixPlayersButton.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.75f, .6f, 10f));
        sixPlayersButton.AddComponent<BoxCollider>();

        numberOfPlayersGameObjects.Add(fourPlayersButton);
        numberOfPlayersGameObjects.Add(fivePlayersButton);
        numberOfPlayersGameObjects.Add(sixPlayersButton);
    }
    
    void checkNumberOfPlayersSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < numberOfPlayersGameObjects.Count; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (numberOfPlayersGameObjects[i].GetComponent<BoxCollider>().Raycast(ray, out hit, 100))
                {
                    numberOfPlayers = i + 4;
                    GameInfo.gameInfo.numberOfPlayers = numberOfPlayers;
                    infoText.GetComponent<TextMesh>().text = numberOfPlayers + " Players";
                    selectCardBackingPhase = true;
                    destroyAllButtons(numberOfPlayersGameObjects);
                    numberOfPlayersGameObjects.Clear();
                    break;
                }
            }
        }
    }

    void showCardBackOptions()
    {
        float objectXLoc = .25f;
        int currentCardIndex = 0;

        foreach(Sprite cardBack in availableCardBacks)
        {
            GameObject cardObject = new GameObject();
            cardObject.AddComponent<SpriteRenderer>();
            cardObject.GetComponent<SpriteRenderer>().sprite = cardBack;
            cardObject.AddComponent<BoxCollider>();
            cardObject.transform.localScale = new Vector3(.5f, .5f, 1f);
            cardObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(objectXLoc + (currentCardIndex * .1f), .6f, 10f));
            currentCardIndex++;
            availableCardBacksGameObjects.Add(cardObject);
        }

    }


    void checkCardBackSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < availableCardBacksGameObjects.Count; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (availableCardBacksGameObjects[i].GetComponent<BoxCollider>().Raycast(ray, out hit, 100))
                {
                    numberOfPlayersChosen += 1;
                    Player newPlayer = new Player("Player " + numberOfPlayersChosen, availableCardBacks[i], 3);
                    availableCardBacks.RemoveAt(i);
                    players.Add(newPlayer);
                    destroyAllButtons(availableCardBacksGameObjects);
                    availableCardBacksGameObjects.Clear();

                    if (numberOfPlayersChosen >= numberOfPlayers)
                    {
                        startGamePhase = true;
                    }
                    else
                    {
                        selectCardBackingPhase = true;
                    }

                    break;
                }
            }
        }
    }

    void destroyAllButtons(List<GameObject> gameObjects)
    {
        foreach(GameObject button in gameObjects)
        {
            Destroy(button);
        }
    }

    void checkStartPressed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (startText.GetComponent<BoxCollider>().Raycast(ray, out hit, 100))
            {
                GameInfo.gameInfo.players = players;
                Application.LoadLevel("Game Scene");
            }
        }
    }
}
