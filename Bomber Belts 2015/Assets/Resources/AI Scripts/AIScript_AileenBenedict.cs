using UnityEngine;
using System.Collections;

using System;
using System.Threading;

public class AIScript_AileenBenedict : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;

    // My variables ------------------------------------------------------
    private float[] bombDistances;
    private float playerLocation;
    private float enemyLocation;

    private int playerLives = 8, enemyLives = 8;

    private const float PADDING = 0.1f;
    private const int NUM_BUTTONS = 8;
      

    private const int MOVE = 1, PUSH = 2, THINK = 3; //teehee
    private int action = PUSH; // Start by pushing the one it's on

    //private Queue path = new Queue();
    private float[] buttonVals;
    private int nextStep = 0;
    private int lastStep = -1;

    private int spookyBombs = 0;
    private int oldSpookyBombs = 0;


    private float stressedBot = 1f; // :') Based on how many lives are left

    private Thread t;

    // -------------------------------------------------------------------


    // Use this for initialization
    void Start () {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();

        // My code ------------------------------------------------------

        buttonVals = new float[NUM_BUTTONS];

        playerLocation = mainScript.getCharacterLocation();
        enemyLocation = mainScript.getOpponentLocation();
        
        t = new Thread(new ThreadStart(findPath));
        t.Start();
        

        // --------------------------------------------------------------

    }


	// Update is called once per frame
	void Update () {

        // Lots of Notes in here~
        {
            /* ----------------------------------------------------------------------------
             * AVAILABLE METHODS:
             * void moveUp()
             * void moveDown()
             * void push()
             *
             * float getCharacterLocation()
             * float getOpponentLocation()
             * float[] getButtonLocations()
             * float[] getButtonCooldowns()
             * bool[] getBeltDirection()
             * float[] getBombDistances()
             * float getPlayerSpeed()
             * float getBombSpeed()
             * ----------------------------------------------------------------------------
             */
        }
        {
            /* Notes ----------------------------------------------------------------------
             * From AI Breakdown.JPG:
             *      1. Find bombs
             *          - Can reach in time
             *          - Moving in our direction
             *      2. Assign score to bombs (min = good)
             *          - Close to agent
             *          - Close to bottom (?)
             *      3. Move to bomb
             *      4. Send (Push) bomb
             *      5. Move to bomb closest to player
             *
             * Goal State: Enemy lives = 0
             * Terminal State: Agent OR Enemy lives = 0
             * Performance Measure: Ag. distance of bombs from each player
             * Available Actions:
             *      1. Move Up
             *      2. Move Down
             *      3. Send (Push) Bomb
             *      4. Idle
             *
             * Can move 10px each game step
             * Takes 1 game step to send a bomb
             * Bombs move @ 5px each step
             * 8 Bombs total
             * ----------------------------------------------------------------------------
             */
        }

        // ----------------------------------------------------------------------------
        // Updating Values
        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();

        bombSpeeds = mainScript.getBombSpeeds();
        playerSpeed = mainScript.getPlayerSpeed();
        bombDistances = mainScript.getBombDistances();
        playerLocation = mainScript.getCharacterLocation();
        enemyLocation = mainScript.getOpponentLocation();
        // ----------------------------------------------------------------------------

        // start new thread if old one ended (I think)
        Log("" + t.IsAlive);
        if (t.IsAlive == false)
        {
            //Log("done");

            t = new Thread(new ThreadStart(findPath));
            t.Start();
            Log("" + t.IsAlive);
           
        }

        // Keep track of player and enemy lives
        spookyBombs = bombsInOurDirection();
        if (spookyBombs != oldSpookyBombs)
        {
            foreach (float f in bombDistances)
            {
                if (f <= 0.1f) // I don't think this is super accurate :')..
                {
                    playerLives--;
                    stressedBot *= 2f;
                }

                if (f >= 18.05f)
                {
                    enemyLives--;
                    stressedBot *= 0.9f;
                }

            }

            if (spookyBombs >= playerLives)
            {
                stressedBot *= 10f; // spooky
            }
            else
            {
                stressedBot /= 10f;
            }

            oldSpookyBombs = spookyBombs;
        }
        

        /*
        // Printing stuff
        for(int i = 0; i < NUM_BUTTONS; i++)
        {
            Log("-------Bomb " + i + " distance: " + bombDistances[i]);
        }
        
        Log("Player Lives: " + playerLives);
        Log("Enemy Lives: " + enemyLives);
        Log("Stressed Bot: " + stressedBot); //teehee
        */

        switch (action){
            case MOVE:
                MoveToButton(buttonLocations[nextStep]);
                break;
                
            case PUSH:
                if (buttonCooldowns[nextStep] <= 0)
                {
                    mainScript.push();
                    lastStep = nextStep;
                    action = THINK;
                }

                break;
                
            case THINK:
                //findPath();
                nextStep = findHighestIndex(buttonVals);

                //Log("Next step is " + nextStep);
                if (nextStep != lastStep)
                {
                    action = MOVE;
                }

                break;
        }
        
           
       
    }

    // ----------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------

    void findPath()
    {

        // 8 Buttons. Find next best button and add to the path Queue
        // Log("We are in findPath");

        // Part 1: Start with distance from player... closer = higher
        float[] distanceVals = getDistanceValues();
        for (int i = 0; i < NUM_BUTTONS; i++)
        {
            buttonVals[i] = distanceVals[i];
        }

        // Part 2: Lower value if going away from you (no point pushing it again)
        for (int i = 0; i < NUM_BUTTONS; i++)
        {
            if (beltDirections[i] == 1 || canPush(i) == false)
            {
                buttonVals[i] -= 100f;
            }
        }

        // Part 3: Higher value if bomb is coming towards you and can be saved
        // The closer the bomb is to hitting, the higher the value
        for (int i = 0; i < NUM_BUTTONS; i++)
        {
            if (beltDirections[i] == -1 && canPush(i) == true)
            {
                //Log("Bomb distance " + i + " is " + bombDistances[i]);
                buttonVals[i] += (10f * (20f - bombDistances[i]) * stressedBot); // idk what to make these valuesss
            }

            //Log("Button " + i + " has value: " + buttonVals[i]);
        }

        // Part 4: Higher threat if bomb is faster
        for (int i = 0; i < NUM_BUTTONS; i++)
        {
            if (beltDirections[i] == -1 && canPush(i) == true)
            {
                buttonVals[i] += bombSpeeds[i] * 5f;
            }
        }

        // Hehe so many for loops

        
    }



    // ----------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------

    bool canPush(int i)
    {

        //Log("Bomb Distance: " + bombDistances[i] + "BombSpeed: " + bombSpeeds[i] +
        //    "ButtonDistance: " + buttonDistanceFromPlayer(i) + "Player Speed: " + playerSpeed);

        // Return true if the time taken from the player to reach the button is
        // less than the time it takes for the bomb to hit
        float BUFFER = 2.0f; // buffer time for player actually hitting the button before it hits

        //Log("Bomb time: " + (bombDistances[i] / bombSpeeds[i]));
        //Log("Player time: " + ((buttonDistanceFromPlayer(i) / playerSpeed)));
        if ((bombDistances[i] / bombSpeeds[i]) > (buttonDistanceFromPlayer(i) / playerSpeed) + BUFFER)
        {
            return true;
        }
        else
            return false;
        

    }
    
    float buttonDistanceFromPlayer(int i)
    {
        return Mathf.Abs(buttonLocations[i] - playerLocation);
    }

    // Part 1 ----------------------------------------------------------
    // For initial values based on button closest to player
    float[] getDistanceValues()
    {
        float[] a = new float[NUM_BUTTONS];

        float[] d = new float[NUM_BUTTONS];
        for(int i = 0; i < NUM_BUTTONS; i++)
        {
            d[i] = buttonDistanceFromPlayer(i);
        }

        float counter = 50f;
        for(int i = 0; i < NUM_BUTTONS; i++)
        {
            int index = findSmallest(d);
            a[index] = counter;
            counter -= 5f;
            d[index] = 100f;
        }


        return a;

    }

    int findSmallest(float[] array)
    {
        int minIndex = 0;
        float min = array[0];
        for(int i = 0; i < NUM_BUTTONS; i++)
        {
            if(array[i] < min)
            {
                min = array[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    // ----------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------

    void MoveToButton(float location) {

        if(playerLocation < (location + PADDING) && playerLocation > (location - PADDING))
        {
            action = PUSH;
        }
        else if(playerLocation < location  ){
          mainScript.moveUp();
        }
        else if(playerLocation > location){
          mainScript.moveDown();
        }
        
    }

    // -----------------------------------------------------------


    int bombsInOurDirection()
    {
        int counter = 0;
        foreach (int i in beltDirections)
        {
            if (i == -1) counter++;
        }

        return counter;
    }

    int findHighestIndex(float[] array)
    {
        float max = array[0];
        int maxIndex = 0;
        for(int i = 0; i < NUM_BUTTONS; i++)
        {
            if(max < array[i])
            {
                max = array[i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }
    
    // ----------------------------------------------------------------------------------------
    // Make output to console easier :')
    void Log(string message) {
        Debug.Log(message + "------------------");
    }

    void Log(float message)
    {
        Debug.Log("-----------------------------------------------------" +
            "-------------------------------------- " +
            message);
    }


}