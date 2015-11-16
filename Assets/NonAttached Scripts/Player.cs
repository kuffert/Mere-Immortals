using UnityEngine;
using System.Collections.Generic;

public class Player {

	public string characterName;
    public bool hasCommittedCards = false;
    public int favor;
	public List<Card> hand = new List<Card> ();
    public List<Card> playedCards = new List<Card>();
	public Sprite cardBack;

	public Player(){
		this.characterName = "";
        this.favor = 0;
		drawCards ();
	}
	public Player(string aCharacterName){
		this.characterName = aCharacterName;
		drawCards();
	}

	//one more card in your hand than the number of players
	public void drawCards(){
		int cardType;
		while (this.hand.Count <= GameSystem.gameSystem.numberOfPlayers) {

			//0=hot 1=dry 2=cold 3=humid (??)
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
    }

    // @Matt this shit too
    public void wipePlayedCards()
    {
        // This needs to empty the list of played cards completely, leaving it a blank list.
    }

    // @Matt this shit too
    public void addSelectedCardsToPlayedCards()
    {
        // Needs to check their hand for cards with isSelected as true, remove them, and add them to their played cards list.
    }


    // @Matt this shit too
    public Vector2 calculateEffectOfPlayedCards()
    {
        Vector2 totalEffectOfPlayedCards = new Vector2(0, 0);

        // CODE GOES HERE BIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIITCH

        return totalEffectOfPlayedCards;
    }
}
