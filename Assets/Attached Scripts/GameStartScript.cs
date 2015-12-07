using UnityEngine;
using System.Collections.Generic;

public class GameStartScript : MonoBehaviour {
    List<Player> players;

    List<GameObject> numberOfPlayersGameObjects;
    List<GameObject> availableCardBacksGameObjects;
    List<GameObject> availableGodNames;

    public List<Sprite> chosenCardBacks;
    public List<Sprite> availableCardbacks;
	public List<Sprite> availableLeftHands;
	public List<Sprite> availableLeftThumbs;
	public List<Sprite> availableRightHands;
	public List<Sprite> availableRightThumbs;
    public List<string> godNames;


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
        availableGodNames = new List<GameObject>();
        godNames = new List<string>();
        players = new List<Player>();
        numberOfPlayers = 0;
        numberOfPlayersChosen = 0;
        availableCardbacks = SpriteAssets.spriteAssets.allCardbacks;
		availableLeftHands = SpriteAssets.spriteAssets.allPlayerLeftHands;
		availableLeftThumbs = SpriteAssets.spriteAssets.allPlayerLeftThumbs;
		availableRightHands = SpriteAssets.spriteAssets.allPlayerRightHands;
		availableRightThumbs = SpriteAssets.spriteAssets.allPlayerRightThumbs;
        godNames.Add("Appollo");
        godNames.Add("Aphorodite");
        godNames.Add("Artemis");
        godNames.Add("Athena");
        godNames.Add("Poseidon");
        godNames.Add("Hades");
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
                    destroyListOfGameObjects(numberOfPlayersGameObjects);
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

        foreach(Sprite cardBack in availableCardbacks)
        {
            GameObject cardObject = new GameObject();
            cardObject.AddComponent<SpriteRenderer>();
            cardObject.GetComponent<SpriteRenderer>().sprite = cardBack;
            cardObject.AddComponent<BoxCollider>();
            cardObject.transform.localScale = new Vector3(.5f, .5f, 1f);
            cardObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(objectXLoc + (currentCardIndex * .1f), .6f, 10f));

            GameObject godName = new GameObject();
            godName.AddComponent<TextMesh>().text = godNames[currentCardIndex];
            TextMesh godNameTextMesh = godName.GetComponent<TextMesh>();
            godNameTextMesh.characterSize = .025f;
            godNameTextMesh.fontSize = 150;
            godNameTextMesh.color = Color.black;
            godNameTextMesh.alignment = TextAlignment.Center;
            godNameTextMesh.anchor = TextAnchor.MiddleCenter;
            godName.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(objectXLoc + (currentCardIndex * .1f), .45f, 10f));
            
            currentCardIndex++;
            availableCardBacksGameObjects.Add(cardObject);
            availableGodNames.Add(godName);
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
                    Player newPlayer = new Player(godNames[i], availableCardbacks[i], 3,
					                              availableLeftHands[i], availableLeftThumbs[i], availableRightHands[i], availableRightThumbs[i]);
                    chosenCardBacks.Add(availableCardbacks[i]);
                    godNames.RemoveAt(i);
                    availableCardbacks.RemoveAt(i);
					availableLeftHands.RemoveAt (i); 
					availableLeftThumbs.RemoveAt (i);
					availableRightHands.RemoveAt (i);
					availableRightThumbs.RemoveAt(i);
                    players.Add(newPlayer);
                    destroyListOfGameObjects(availableCardBacksGameObjects);
                    availableCardBacksGameObjects.Clear();
                    destroyListOfGameObjects(availableGodNames);
                    availableGodNames.Clear();

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

    void destroyListOfGameObjects(List<GameObject> gameObjects)
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
				SpriteAssets.spriteAssets.resetAllCardbacks();
				Destroy(this);
                Application.LoadLevel("Game Scene");
            }
        }
    }
}
