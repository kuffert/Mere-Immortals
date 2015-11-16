using UnityEngine;
using System.Collections.Generic;

public class Player {

	public string characterName;
    public int favor;
	public List<Card> hand = new List<Card> ();
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


}
