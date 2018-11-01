using UnityEngine;
using System.Collections;
using UnityEditor;

public class GameScript : MonoBehaviour {

    public CharacterScript redPlayer, bluePlayer;

    public HealthBarScript redHealthIndicator, blueHealthIndicator;

    public Texture redWinScreen, blueWinScreen;

    public int startingHealth = 8;
    int blueHealth, redHealth;
    public float playerSpeed = 5;
    public float playerPressCooldown = 1f;
    public float buttonCooldown = 1.0f;
    public float initialBombSpeed = 1;
    public float bombAcceleration = 1.003f;

    public BeltScript[] belts;

    public CameraScript camera;

	// Use this for initialization
	void Awake () 
    {
        camera = Camera.main.GetComponent<CameraScript>();

        redHealth = startingHealth;
        blueHealth = startingHealth;

        redPlayer.setup(false, this, PlayerPrefs.GetString("RED_AI"));
        bluePlayer.setup(true, this, PlayerPrefs.GetString("BLUE_AI"));

        redHealthIndicator.setup(startingHealth);
        blueHealthIndicator.setup(startingHealth);

        for (int i = 0; i < belts.Length; i++)
        {
            belts[i].setup(this);
            belts[i].redButton.setup(buttonCooldown);
            belts[i].blueButton.setup(buttonCooldown);
            belts[i].bomb.setup(initialBombSpeed, bombAcceleration);
        }
	}

    int winner = 0;

    // Update is called once per frame
    void Update() 
    {
        //Death actions
        if (redHealth <= 0)
        {
            winner = -1;
            stop();
        }
        else if (blueHealth <= 0)
        {
            winner = 1;
            stop();
        }

	}

    void stop()
    {
        foreach (BeltScript belt in belts)
        {
            belt.stop();
        }

        redPlayer.stop();
        bluePlayer.stop();
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(Screen.width / 2 + 10, Screen.height - 65, 100, 50), "End Match"))
        {
            Application.LoadLevel("Menu");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 110, Screen.height - 65, 100, 50), "Restart Match"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (winner == 1)
            GUI.DrawTexture(new Rect((Screen.width / 2) - (redWinScreen.width / 2), (Screen.height / 2) - (redWinScreen.height / 2), redWinScreen.width, redWinScreen.height), redWinScreen);
        else if (winner == -1)
            GUI.DrawTexture(new Rect((Screen.width / 2) - (blueWinScreen.width / 2), (Screen.height / 2) - (blueWinScreen.height / 2), blueWinScreen.width, blueWinScreen.height), blueWinScreen);
    }

    #region Belt Info

    public float[] getButtonLocations()
    {
        float[] result = new float[BeltCount];

        for (int i = 0; i < BeltCount; i++)
        {
            result[i] = belts[i].Position;
        }

        return result;
    }

    public int[] getBeltDirections(bool playerID)
    {
        int[] result = new int[BeltCount];

        for (int i = 0; i < BeltCount; i++)
        {
            result[i] = belts[i].getDirection(playerID);
        }

        return result;
    }

    public float[] getBombDistances(bool playerID)
    {
        float[] result = new float[BeltCount];

        for (int i = 0; i < BeltCount; i++)
        {
            result[i] = belts[i].getBombDistance(playerID);
        }

        return result;
    }

    public float[] getBombSpeeds()
    {
        float[] result = new float[BeltCount];

        for (int i = 0; i < BeltCount; i++)
        {
            result[i] = belts[i].BombSpeed;
        }

        return result;
    }

    public float[] getButtonCooldowns(bool playerID)
    {
        float[] result = new float[BeltCount];

        for (int i = 0; i < BeltCount; i++)
        {
            result[i] = belts[i].getCoolDown(playerID);
        }

        return result;
    }

    public int BeltCount
    {
        get { return belts.Length; }
    }

    #endregion

    #region Player Info

    public float getOpponentLocation(bool playerID)
    {
        if (playerID == true)
            return redPlayer.getCharacterLocation();
        else
            return bluePlayer.getCharacterLocation();
    }

    public bool push(bool playerID)
    {
        foreach (BeltScript belt in belts)
        {
            if (belt.attemptPush(playerID) == true)
            {
                belt.pushButton(playerID);
                return true;
            }
        }

        return false;
    }

    public void damage(bool playerID)
    {
        if (playerID == true)
        {
            blueHealth--;
            blueHealthIndicator.damage();
        }
        else
        {
            redHealth--;
            redHealthIndicator.damage();
        }

        camera.shakeOnce(0.5f, 0.25f, 0.025f);
    }

    #endregion
}
