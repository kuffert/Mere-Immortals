using UnityEngine;
using System.Collections.Generic;

public class humidCard : Card {
	
	public humidCard(Sprite bckimg) {
		this.effect = new Vector2 (0, -1);
		this.frontImage = humid;
		this.backImage = bckimg;
        this.isSelected = false;
	}
	
}

