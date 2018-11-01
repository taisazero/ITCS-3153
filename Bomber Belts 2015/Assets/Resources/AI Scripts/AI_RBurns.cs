using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_RBurns: MonoBehaviour
{
	public CharacterScript mainScript;
	
	public float[] bombSpeed;
	public float playerSpeed;
	public float[] buttonCooldowns;
	public float[] buttonLocations;
	public float playerLoc, enemyLoc;
	public int[] beltDirections = new int[8];
	public float[] bombDistance = new float[8];
	
	// Start is called once when the game runs
	void Start () {
		mainScript = GetComponent<CharacterScript>();
		
		if (mainScript == null)
		{
			print("No CharacterScript found on " + gameObject.name);
			this.enabled = false;
		}
		
		buttonLocations = mainScript.getButtonLocations();
		
		playerSpeed = mainScript.getPlayerSpeed();
	}
	
	int targetBelt = 0;
	
	public float GetBombTime (int target)
	{
		if (target < 0 || target > beltDirections.Length - 1)
			return Mathf.Infinity;
		return bombDistance[targetBelt] / bombSpeed[targetBelt];
	}
	
	// Update is called once per frame
	void Update ()
	{
		buttonCooldowns = mainScript.getButtonCooldowns();
		beltDirections = mainScript.getBeltDirections();
		
		//how fast the bombs move
		bombSpeed = mainScript.getBombSpeeds();
		
		//get locations of players
		playerLoc = mainScript.getCharacterLocation ();
		enemyLoc = mainScript.getOpponentLocation ();
		
		bombDistance = mainScript.getBombDistances ();
		
		//print ("Start");
		int currentSlot = 0;
		float distanceFromCurrent = Mathf.Infinity;
		
		List<int> Spots = new List<int> ();
		
		for (int i = 0; i < beltDirections.Length; i++)
		{
			if (Mathf.Abs (playerLoc - buttonLocations [i]) < distanceFromCurrent)
			{
				currentSlot = i;
				distanceFromCurrent = Mathf.Abs (playerLoc - buttonLocations [i]);
			}
			
			if (beltDirections [i] == -1 || beltDirections [i] == 0) 
			{
				float bombTime = bombDistance [i] / bombSpeed [i];
				float playerTime = Mathf.Abs (playerLoc - buttonLocations [i]) / playerSpeed;
				
				//print ("Loc:" + i + "  Bomb: " + bombTime + "  Player:" + playerTime);
				
				if (playerTime < bombTime && bombTime > buttonCooldowns[i])
				{
					Spots.Add(i);
				}
			}
		}
		//print("Stop");
		print (buttonCooldowns [5]);
		
		float currentTime = Mathf.Infinity;
		foreach (int spot in Spots)
		{
			float bombTime = bombDistance [spot] / bombSpeed [spot];
			
			if (currentTime > bombTime)
			{
				currentTime = bombTime;
				targetBelt = spot;
			}
		}
		
		print (targetBelt);
		
		if (buttonLocations[targetBelt] < mainScript.getCharacterLocation())
		{
			mainScript.moveDown();
		}
		else if (buttonLocations[targetBelt] > mainScript.getCharacterLocation())
		{
			mainScript.moveUp();
		}
		
		bool canMakeIt = (Mathf.Abs (playerLoc - buttonLocations [targetBelt]) / playerSpeed) + 0.35f < bombDistance[targetBelt] / bombSpeed[targetBelt];
		bool onTarget = targetBelt == currentSlot;
		
		if (beltDirections[currentSlot] != 1)
		{
			if ( canMakeIt || onTarget)
			{
				mainScript.push();
			}
		}
	}
}