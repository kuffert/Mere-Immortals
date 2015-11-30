using UnityEngine;
using System.Collections.Generic;

public class hotCard : Card {

	public hotCard(Sprite bckimg) {
		this.effect = new Vector2 (0, 1);
		this.frontImage = hot;
		this.backImage = bckimg;
        this.isSelected = false;

	}

}
