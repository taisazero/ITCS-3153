using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {

    //BLUE = True, RED = False 
    bool playerID;

    //Master Script
    GameScript mainScript;
    public ParticleSystem wheelSmoke;
    public Transform wheel;

    float pushLockTime;
    float pushLockTick = 0;
	
	// Update is called once per frame
	void Update () {

        if (pushLockTick < pushLockTime && run)
        {
            pushLockTick += Time.deltaTime;
            wheelSmoke.enableEmission = false;
        }
	}

    public void setup(bool ID, GameScript game, string AIName)
    {
        mainScript = game;
        playerID = ID;
        pushLockTime = mainScript.playerPressCooldown;
        pushLockTick = pushLockTime;

        UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(gameObject, "Assets/Resources/Game Scripts/CharacterScript.cs (34,9)", AIName);
    }

    bool run = true;
    public void stop()
    {
        run = false;
        pushLockTick = 0;
    }

    #region Actions

    /// <summary>
    /// Move the character upwards relative to the camera. 
    /// </summary>
    public void moveUp()
    {
        if (pushLockTick >= pushLockTime && transform.position.z < 8.5f)
        {
            GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0, 0, mainScript.playerSpeed * Time.deltaTime));
            GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0,0,0));

            wheelSmoke.enableEmission = true;

            wheel.Rotate(-getPlayerSpeed(), 0, 0, Space.Self);
        }
    }

    /// <summary>
    /// Move the character downwards relative to the camera.
    /// </summary>
    public void moveDown()
    {
		if (pushLockTick >= pushLockTime && transform.position.z > -8.5f)
		{
            GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0, 0, -mainScript.playerSpeed * Time.deltaTime));
            GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, 180, 0));
            wheelSmoke.enableEmission = true;

            wheel.Rotate(-getPlayerSpeed(), 0, 0, Space.Self);
        }
    }

    /// <summary>
    /// Attempt to push the nearest available button. Will do nothing if no button can be pressed.
    /// </summary>
    public void push()
    {
        //TODO: Go to gamescript to find nearest button to push
        if (mainScript.push(playerID))
        {
            pushLockTick = 0;
            wheelSmoke.enableEmission = false;
        }
    }

    #endregion

    #region Getting Game Info

    /// <summary>
    /// Get this character's location on the Z(Up & Down)-axis.
    /// </summary>
    /// <returns></returns>
    public float getCharacterLocation()
    {
        return transform.position.z;
    }

    /// <summary>
    /// Get the opponent's location on the Z(Up & Down)-axis 
    /// </summary>
    /// <returns></returns>
    public float getOpponentLocation()
    {
        return mainScript.getOpponentLocation(playerID);
    }

    /// <summary>
    /// Returns an array of each button's Z(Up & Down)-axis location.
    /// </summary>
    /// <returns></returns>
    public float[] getButtonLocations()
    {
        return mainScript.getButtonLocations();
    }

    /// <summary>
    /// Returns an array of values representing each belt's direction. 1 = Away from this character, -1 = Towards this character, 0 = stationary
    /// </summary>
    /// <returns></returns>
    public int[] getBeltDirections()
    {
        return mainScript.getBeltDirections(playerID);
    }

    /// <summary>
    /// Returns an array of values corresponding the distance of the bomb to this side.
    /// </summary>
    /// <returns></returns>
    public float[] getBombDistances()
    {
        return mainScript.getBombDistances(playerID);
    }

    /// <summary>
    /// Returns an array of values corresponding to the bomb's current speed.
    /// </summary>
    /// <returns></returns>
    public float[] getBombSpeeds()
    {
        return mainScript.getBombSpeeds();
    }

    /// <summary>
    /// Returns an array of values corresponding to how much longer until a button can be pressed. A value less than or equal to zero means the button is available.
    /// </summary>
    /// <returns></returns>
    public float[] getButtonCooldowns()
    {
        return mainScript.getButtonCooldowns(playerID);
    }

    public int BeltCount
    {
        get { return mainScript.BeltCount; }
    }

    public float getPlayerSpeed()
    {
        return mainScript.playerSpeed;
    }

    #endregion

    #region Other

    /// <summary>
    /// Is this character the Red player?
    /// </summary>
    public bool isRed
    {
        get { return !playerID; }
    }

    /// <summary>
    /// Is this character the Blue player?
    /// </summary>
    public bool isBlue
    {
        get { return playerID; }
    }

    #endregion
}
