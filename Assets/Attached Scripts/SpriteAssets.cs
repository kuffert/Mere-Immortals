using UnityEngine;
using System.Collections.Generic;

public class SpriteAssets : MonoBehaviour {
    public static SpriteAssets spriteAssets;
    public Sprite hot;
    public Sprite cold;
    public Sprite humid;
    public Sprite dry;

    public Sprite cardAppollo;
    public Sprite cardAphorodite;
    public Sprite cardArtemis;
    public Sprite cardAthena;
    public Sprite cardPoseidon;
    public Sprite cardHades;


	public Sprite leftHandAppollo;
	public Sprite leftHandAphorodite;
	public Sprite leftHandArtemis;
	public Sprite leftHandAthena;
	public Sprite leftHandPoseidon;
	public Sprite leftHandHades;
	
	public Sprite leftThumbAppollo;
	public Sprite leftThumbAphorodite;
	public Sprite leftThumbArtemis;
	public Sprite leftThumbAthena;
	public Sprite leftThumbPoseidon;
	public Sprite leftThumbHades;

	public Sprite rightHandAppollo;
	public Sprite rightHandAphorodite;
	public Sprite rightHandArtemis;
	public Sprite rightHandAthena;
	public Sprite rightHandPoseidon;
	public Sprite rightHandHades;

	public Sprite rightThumbAppollo;
	public Sprite rightThumbAphorodite;
	public Sprite rightThumbArtemis;
	public Sprite rightThumbAthena;
	public Sprite rightThumbPoseidon;
	public Sprite rightThumbHades;

    public Sprite summerButton;
    public Sprite winterButton;
    public Sprite fallButton;
    public Sprite springButton;

	public Sprite summerWeatherTable;
	public Sprite winterWeatherTable;
	public Sprite fallWeatherTable;
	public Sprite springWeatherTable;

    public Sprite blankTile;
    public Sprite threePlayerButton;
    public Sprite fourPlayerButton;
    public Sprite fivePlayerButton;
    public Sprite sixPlayerButton;
    public Sprite DITile;

    public List<Sprite> allCardbacks;

	public List<Sprite> allPlayerLeftHands;
	public List<Sprite> allPlayerLeftThumbs;
	public List<Sprite> allPlayerRightHands;
	public List<Sprite> allPlayerRightThumbs;

    void Awake()
    {
        if (spriteAssets == null)
        {
            DontDestroyOnLoad(gameObject);
            spriteAssets = this;
            allCardbacks = new List<Sprite>();
            allCardbacks.Add(cardAppollo);
            allCardbacks.Add(cardAphorodite);
            allCardbacks.Add(cardArtemis);
            allCardbacks.Add(cardAthena);
            allCardbacks.Add(cardPoseidon);
            allCardbacks.Add(cardHades);

			allPlayerLeftHands = new List<Sprite>();
			allPlayerLeftHands.Add (leftHandAppollo);
			allPlayerLeftHands.Add (leftHandAphorodite);
			allPlayerLeftHands.Add (leftHandArtemis);
			allPlayerLeftHands.Add (leftHandAthena);
			allPlayerLeftHands.Add (leftHandPoseidon);
			allPlayerLeftHands.Add (leftHandHades);

			allPlayerLeftThumbs = new List<Sprite>();
			allPlayerLeftThumbs.Add (leftThumbAppollo);
			allPlayerLeftThumbs.Add (leftThumbAphorodite);
			allPlayerLeftThumbs.Add (leftThumbArtemis);
			allPlayerLeftThumbs.Add (leftThumbAthena);
			allPlayerLeftThumbs.Add (leftThumbPoseidon);
			allPlayerLeftThumbs.Add (leftThumbHades);

			allPlayerRightHands = new List<Sprite>();
			allPlayerRightHands.Add (rightHandAppollo);
			allPlayerRightHands.Add (rightHandAphorodite);
			allPlayerRightHands.Add (rightHandArtemis);
			allPlayerRightHands.Add (rightHandAthena);
			allPlayerRightHands.Add (rightHandPoseidon);
			allPlayerRightHands.Add (rightHandHades);
			
			allPlayerRightThumbs = new List<Sprite>();
			allPlayerRightThumbs.Add (rightThumbAppollo);
			allPlayerRightThumbs.Add (rightThumbAphorodite);
			allPlayerRightThumbs.Add (rightThumbArtemis);
			allPlayerRightThumbs.Add (rightThumbAthena);
			allPlayerRightThumbs.Add (rightThumbPoseidon);
			allPlayerRightThumbs.Add (rightThumbHades);

        }
        else if (spriteAssets != this)
        {
            Destroy(gameObject);
        }
    }

	public void resetAllCardbacks(){
		List<Sprite> cb = new List<Sprite> ();
		cb.Add(cardAppollo);
		cb.Add(cardAphorodite);
		cb.Add(cardArtemis);
		cb.Add(cardAthena);
		cb.Add(cardPoseidon);
		cb.Add(cardHades);
		allCardbacks = cb;

		cb.Clear();
		cb.Add(leftHandAppollo);
		cb.Add(leftHandAphorodite);
		cb.Add(leftHandArtemis);
		cb.Add(leftHandAthena);
		cb.Add(leftHandPoseidon);
		cb.Add(leftHandHades);
		allPlayerLeftHands = cb;

		cb.Clear ();
		cb.Add(leftThumbAppollo);
		cb.Add(leftThumbAphorodite);
		cb.Add(leftThumbArtemis);
		cb.Add(leftThumbAthena);
		cb.Add(leftThumbPoseidon);
		cb.Add(leftThumbHades);
		allPlayerLeftThumbs = cb;


		cb.Clear();
		cb.Add(rightHandAppollo);
		cb.Add(rightHandAphorodite);
		cb.Add(rightHandArtemis);
		cb.Add(rightHandAthena);
		cb.Add(rightHandPoseidon);
		cb.Add(rightHandHades);
		allPlayerRightHands = cb;
		
		cb.Clear ();
		cb.Add(rightThumbAppollo);
		cb.Add(rightThumbAphorodite);
		cb.Add(rightThumbArtemis);
		cb.Add(rightThumbAthena);
		cb.Add(rightThumbPoseidon);
		cb.Add(rightThumbHades);
		allPlayerRightThumbs = cb;




	}
}
