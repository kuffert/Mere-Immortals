using UnityEngine;
using System.Collections.Generic;

public class coldCard : Card {
	
	public coldCard(Sprite bckimg) {
		this.effect = new Vector2 (0, -1);
		this.frontImage = cold;
		this.backImage = bckimg;
	}
	
}
