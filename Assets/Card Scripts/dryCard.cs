using UnityEngine;
using System.Collections.Generic;

public class dryCard : Card {
	
	public dryCard(Sprite bckimg) {
		this.effect = new Vector2 (1, 0);
		this.frontImage = dry;
		this.backImage = bckimg;
        this.isSelected = false;
    }
	
}

