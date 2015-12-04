using UnityEngine;
using System.Collections.Generic;

public class Player {

	public string characterName;
    public bool hasCommittedCards = false;
    public int favor;
	public List<Card> hand = new List<Card> ();
    public List<Card> playedCards = new List<Card>();
	public Sprite cardBack;

    private float gapBetweenCards;
    private float sideBuffer = .2f;
    private float rightSide = .8f;

    public Player(string characterName, Sprite cardBack, int startingFavor)
    {
        this.characterName = characterName;
        this.favor = startingFavor;
        this.cardBack = cardBack;
        drawCards();
        gapBetweenCards = .6f / hand.Count;
    }

	//one more card in your hand than the number of players
	public void drawCards(){
		int cardType;
		while (this.hand.Count <= GameInfo.gameInfo.numberOfPlayers) {

			//0=hot 1=dry 2=cold 3=humid
			cardType = Random.Range (0,4);
			if (cardType == 0) {
				this.hand.Add (new hotCard (cardBack));
			} else if (cardType == 1) {
				this.hand.Add (new dryCard(cardBack));
			} else if (cardType == 2) {
				this.hand.Add (new coldCard(cardBack));
			} else {
				this.hand.Add (new humidCard(cardBack));
			}
		}
	}

    // @Matt just fill thous out with what it needs to do
    public void adjustFavor(int favorEffect)
    {
        // Code that adds the favor effect to their favor
        this.favor += favorEffect;
    }

    public List<GameObject> showCards()
    {
        List<GameObject> cardObjects = new List<GameObject>();

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject cardObject = new GameObject();
            cardObject.AddComponent<SpriteRenderer>();
            cardObject.GetComponent<SpriteRenderer>().sprite = this.hand[i].frontImage;
            cardObjects.Add(cardObject);

            cardObject.AddComponent<BoxCollider>();

            cardObject.transform.localScale = new Vector3(.5f, .5f, 0f);

            cardObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(sideBuffer + (i * gapBetweenCards), .15f, 0f));
        }
        return cardObjects;
    }

    public void showPlayedCards(List<GameObject> cardImages, int index)
    {
        float math;
        math = index + 1;
        math /= 10;
        float yOffset = .9f - math;
        for (int i = 0; i < playedCards.Count; i++)
        {
            GameObject cardImage = new GameObject();
            cardImage.AddComponent<SpriteRenderer>();
            cardImage.GetComponent<SpriteRenderer>().sprite = this.cardBack;
            cardImage.transform.localScale = new Vector3(.25f, .25f, 0f);
            cardImage.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(rightSide + (i * (.1f / playedCards.Count)), yOffset, 10f));
            cardImages.Add(cardImage);
        }
    }

    // @Matt this shit too
    public void wipePlayedCards()
    {
        // This needs to empty the list of played cards completely, leaving it a blank list.
        for (int i = 0; i < this.playedCards.Count; i++)
        {
			this.playedCards.RemoveRange(0, this.playedCards.Count);
        }

    }

    // @Matt this shit too
    public void commitCards()
    {
        // Needs to check their hand for cards with isSelected as true, remove them, and add them to their played cards list.
        for (int i = 0; i < this.hand.Count; i++)
        {
            if (this.hand[i].isSelected)
            {
                this.playedCards.Add(this.hand[i]);
                this.hand.Remove(this.hand[i]);
                i--;
            }
        }
    }

    // @Matt this shit too
    public Vector2 calculateEffectOfPlayedCards()
    {
        Vector2 totalEffectOfPlayedCards = new Vector2(0, 0);
        
		for (int i = 0; i < this.playedCards.Count; i++) {
			totalEffectOfPlayedCards += this.playedCards [i].effect;
		}

        return totalEffectOfPlayedCards;
    }
    
    //a comment called fuckboy
    public void updateCardPositions(List<GameObject> cardObjects)
    {
        foreach (GameObject cardObject in cardObjects)
        {
            int i = cardObjects.IndexOf(cardObject);
            if (hand[i].isSelected)
            {
				cardObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(sideBuffer + (i * gapBetweenCards), .3f, 10f));
            }
            else
            {
				cardObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(sideBuffer + (i * gapBetweenCards), .15f, 10f));
            }
        }
    }
}
