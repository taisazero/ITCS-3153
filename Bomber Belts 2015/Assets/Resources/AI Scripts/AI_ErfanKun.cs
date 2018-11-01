using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

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
             * 
             * Distance Formula: 
             * 
             *    *
             *   /D\
             *  /S|T\
             * ----------------------------------------------------------------------------
             */

public class AI_ErfanKun : MonoBehaviour
{

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public float[] buttonLocations;
    public float curLoc;
    public float enemyLoc;
    public int[] beltDirections = new int[8]; // -1 towards me , 1 towards enemy, 0 for idle
    public float[] bombDistances = new float[8];
    public const float BUFFER = 0.37f;

    public SortedList <int,float> beltList = new SortedList <int,float>();

    public bool updateList = true;

    public bool safeBot = true;
    public bool aggroBot = false;
    // Use this for initialization
    void Start()
    {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();
        // Log(curLoc);




    }

    public int countEnemyBelts() {
        int result = 0;
        for (int i = 0; i < beltDirections.Length; i++)
        {
            if (beltDirections[i] == -1) {
                result++;
            }
        }

        return result;
        }

    public int countAllyBelts()
    {
        int result = 0;
        for (int i = 0; i < beltDirections.Length; i++)
        {
            if (beltDirections[i] == 1)
            {
                result++;
            }
        }

        return result;
    }

    public float getBoomTime(int bombNum)
    {
        //check if invalid bomb
        if (bombNum < 0 || bombNum > 7)
        {
            //really big time
            return Mathf.Infinity;
        }
        else if (bombSpeeds[bombNum] != 0)
        {
            //speed = distance / time
            // THEN: 
            // time = distance/speed
            return bombDistances[bombNum] / bombSpeeds[bombNum];
        }
        else
        {
            if (beltList.Count != 0) {
                return (float)beltList.Keys[beltList.Count - 1];
            }
            else {
                return 10;
            }
        }

    }

    public bool savable(int bomb)
    {
        // time = distance/ speed
        // distance from button = abs(botLoc - targetButton)
        float timeToSave = Mathf.Abs(curLoc - buttonLocations[bomb]) / playerSpeed + BUFFER;
        float boomTime = getBoomTime(bomb);

        return timeToSave < boomTime && buttonCooldowns [bomb] < 0.0f;


    }

    public bool considerableBomb(int bomb)
    {

        return savable(bomb) && beltDirections[bomb] != 1;

    }

    public int getCurBelt()
    {

        float distance = Mathf.Infinity;
        int curri = 0;
        for (int i = 0; i < beltDirections.Length; i++)
        {
            if (Mathf.Abs(curLoc - buttonLocations[i]) < distance)
            {
                curri = i;
                distance = Mathf.Abs(curLoc - buttonLocations[i]);
            }

        }

        return curri;


    }

    public int getEnemyCurBelt()
    {

        float distance = Mathf.Infinity;
        int curri = 0;
        for (int i = 0; i < beltDirections.Length; i++)
        {
            if (Mathf.Abs(enemyLoc - buttonLocations[i]) < distance)
            {
                curri = i;
                distance = Mathf.Abs(enemyLoc - buttonLocations[i]);
            }

        }

        return curri;


    }

    public int getLowestScore(SortedList<int, float> l) {
        int resultIndex = 0;
        float temp = Mathf.Infinity;
        for (int i = 0; i < l.Count; i++) {
            if (l.Values[i] < temp) {
                temp = l.Values[i];
                resultIndex = i;
            }



        }

        return resultIndex;

    }



    // Update is called once per frame
    void Update()
    {

        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();
        bombDistances = mainScript.getBombDistances();

        //locations
        curLoc = mainScript.getCharacterLocation();
        enemyLoc = mainScript.getOpponentLocation();
        //bugatti shit like desiigner
        bombSpeeds = mainScript.getBombSpeeds();





        /* ----------------------------------------------------------------------------
         * AVAILABLE METHODS:
         * void moveUp()
         * void moveDown()
         * void push()
         *
         * float getCharacterLocation() used
         * float getOpponentLocation() used
         * float[] getButtonLocations() used
         * float[] getButtonCooldowns() used
         * int[] getBeltDirections() used
         * float[] getBombDistances() used
         * float getPlayerSpeed() used
         * float[] getBombSpeeds() used 
         * ----------------------------------------------------------------------------
         */

        //Your AI code goes here


  


        // int marker = 0; // TODO

        for (int i = 0; i < beltDirections.Length; i++)
        {

                
            bool isPushed = beltDirections[i] == 1;
            float distanceFromBot = Mathf.Abs(curLoc - buttonLocations[i]);
            float timeToBoom = getBoomTime(i);
            float score = 0;
            int aggroBelts = countEnemyBelts();
            int myBelts = countAllyBelts();
            if (myBelts >3) //aggroBelts <3 && myBelts>aggroBelts
            {
                safeBot = false;
                aggroBot = true;
                
            }
            else {
                aggroBot = false;
                safeBot = true;
                
            }

            if (safeBot)
            {
                score =  distanceFromBot + (timeToBoom) - bombSpeeds[i]*10;
                if ( beltDirections[i] == -1)
                {
                    score -= 10;
                }

                }
            else
            {
                score = 1/distanceFromBot +  timeToBoom - bombSpeeds[i] * 4 - Math.Abs(getEnemyCurBelt() - getCurBelt())*2; ;
                if (beltDirections[i] == -1)
                {
                    score -= 6;
                }


            }
            if (!considerableBomb(i)) {

            score += 1000;
            }
            if (beltList.Count < 8)
            {
                beltList.Add(i, score);
            }

            else {
                beltList[i] = score;
            }

            Log("For Location: " +i+ " Distance is: " + distanceFromBot + " and time is: " + timeToBoom+ " and score is: "+score);


        }
        updateList = false;
        if (aggroBot)
            Log("Aggro On");
        else
            Log("Safe On");
        if (beltList.Count != 0)
        {

            // Goes to the button with the lowest score
            int index, loc = 0;
            float score = 0.0f;
            
                index = getLowestScore(beltList);
                loc = (int)beltList.Keys[index];
                score = (float)beltList.Values[index];
               
            

           
            
            Log("Going to " + loc + " with the score " + score);

            if (buttonLocations[loc] < mainScript.getCharacterLocation())
            {
                mainScript.moveDown();
                // mainScript.push();
            }

            else if (buttonLocations[loc] > mainScript.getCharacterLocation())
            {
                mainScript.moveUp();
                //mainScript.push();
            }


            if (savable(loc) && loc == getCurBelt())
            {
                mainScript.push();
                Log("Pushed " + loc);
                 
                updateList = true;
                    
            }

            





        }



    }





    public void Log(string msg)
    {
        Debug.Log("ErfanKun: --->  " + msg);
    }
    public void Log(float msg)
    {
        Debug.Log("ErfanKun: --->  " + msg);
    }
   /* public void Log(SortedList<int,float> l)
    {
        string s = "ErfanKun: --->  ";
        foreach (DictionaryEntry kvp in l)
        {
            s += " Score: " + kvp.Key;
            s += " Location: " + kvp.Value;

        }
        Log(s);
    }*/
}


