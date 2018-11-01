using UnityEngine;
using System.Collections;

public class AI_BriannaChen : MonoBehaviour{
	
	public CharacterScript mainScript;
	
	public float[] bombSpeeds;
	public float[] buttonCooldowns;
	public float playerSpeed;
	public int[] beltDirections;
	public float[] buttonLocations;
	public float[] bombLocations;
	
	// Use this for initialization
	void Start () {
		mainScript = GetComponent<CharacterScript>();
		
		if (mainScript == null)
		{
			print("No CharacterScript found on " + gameObject.name);
			this.enabled = false;
		}
		
		buttonLocations = mainScript.getButtonLocations ();
		playerSpeed = mainScript.getPlayerSpeed();
	}
	
	int targetBeltIndex = 0;
	
	// Update is called once per frame
	void Update () {
		
		buttonCooldowns = mainScript.getButtonCooldowns();
		beltDirections = mainScript.getBeltDirections();
		bombSpeeds = mainScript.getBombSpeeds();
		bombLocations = mainScript.getBombDistances();
		
		float minDistance = 1000; 
		int minIndex = 0;
		float curDistance;
		
		
		for (int i = 0; i < beltDirections.Length; i++)
		{
			curDistance = Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation());
			if (buttonCooldowns[i] <= 0 && (beltDirections[i] == -1 || beltDirections[i] == 0))
			{
				if (curDistance < minDistance)
				{
					minIndex = i;
					minDistance = curDistance;
					if(minIndex != 0 && (bombLocations[minIndex] > bombLocations[minIndex-1]) && (bombSpeeds[minIndex] < bombSpeeds[minIndex-1])){
						if(bombLocations[minIndex-1] < bombLocations[minIndex+1]){
							minIndex = i - 1;
							minDistance = curDistance;}
					}
					if(minIndex != 7 && (bombLocations[minIndex] > bombLocations[minIndex+1]) && (bombSpeeds[minIndex] < bombSpeeds[minIndex+1])){
						minIndex = i + 1;
						minDistance = curDistance;
					}
					
				}
			}
		}
		
		
		
		
		targetBeltIndex = minIndex;
		
		if (buttonLocations[targetBeltIndex] < mainScript.getCharacterLocation())
		{
			mainScript.moveDown();
			mainScript.push();
			
		}
		else
		{
			mainScript.moveUp();
			mainScript.push();
		}
		
		
	}
}



