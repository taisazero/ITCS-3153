using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    Object[] aiScripts;
    Object redAI, blueAI;

    Object[] levels;
    int selectLevel = 0;

    public Texture versusLogo;
    public Texture logo;

    public GUISkin menuGUISkin;

	// Use this for initialization
	void Start () {
        aiScripts = Resources.LoadAll("AI Scripts");
        levels = Resources.LoadAll("Levels");
	}

    // Update is called once per frame
    void Update()
    {

	}

    float buttonWidth = 185 * (Screen.width / 797f);
    float buttonHeight = 35 * (Screen.height / 598f);
    float startOffsetVertical = 45 * (Screen.height / 598f);
    float startOffsetHorizontal = 25 * (Screen.width / 797f);
    float boxBorderOffset = 5;

    Vector2 blueScrollView = Vector2.zero;
    Vector2 redScrollView = Vector2.zero;
    Vector2 levelScrollView = Vector2.zero;

    void OnGUI()
    {

        GUI.DrawTexture(new Rect(HalfScreenWidth - (versusLogo.width / 2), HalfScreenHeight + 75, versusLogo.width, versusLogo.height), versusLogo);

        GUI.DrawTexture(new Rect(HalfScreenWidth - (logo.width / 2), -75, logo.width, logo.height), logo);

        #region AI Selection

        Rect AIWindowInner = new Rect(0, 0, buttonWidth, (buttonHeight * aiScripts.Length) + boxBorderOffset);
        Rect blueAIScrollRect = new Rect(startOffsetHorizontal - boxBorderOffset, startOffsetVertical, buttonWidth + (2 * boxBorderOffset), (buttonHeight * 10) + boxBorderOffset);

        GUI.Label(new Rect(startOffsetHorizontal - boxBorderOffset, 10, buttonWidth, buttonHeight), "Choose Blue AI:", menuGUISkin.label);

        blueScrollView = GUI.BeginScrollView(blueAIScrollRect, blueScrollView, AIWindowInner);

        //Choosing Blue AI
        for (int i = 0; i < aiScripts.Length; i++)
        {
            if (GUI.Button(new Rect(0, i * buttonHeight, buttonWidth, buttonHeight), aiScripts[i].name, menuGUISkin.button))
            {
                blueAI = aiScripts[i];
            }
        }
        GUI.EndScrollView();

        GUI.Label(new Rect(Screen.width - (startOffsetHorizontal + buttonWidth - boxBorderOffset), 10, buttonWidth, buttonHeight), "Choose Red AI:", menuGUISkin.label);

        //Choosing Red AI
        Rect redAIScrollRect = new Rect(Screen.width - (buttonWidth + startOffsetHorizontal - boxBorderOffset), startOffsetVertical, buttonWidth + (2 * boxBorderOffset), (buttonHeight * 10) + boxBorderOffset); ;
        redScrollView = GUI.BeginScrollView(redAIScrollRect, redScrollView, AIWindowInner);

        for (int i = 0; i < aiScripts.Length; i++)
        {
            if (GUI.Button(new Rect(0, i * buttonHeight, buttonWidth, buttonHeight), aiScripts[i].name, menuGUISkin.button))
            {
                redAI = aiScripts[i];
            }
        }
        GUI.EndScrollView();

        if (redAI)
        {
            Rect labelRect = GUILayoutUtility.GetRect(new GUIContent("     " + redAI.name + "     "), menuGUISkin.label);
            GUI.Label(new Rect(HalfScreenWidth + (versusLogo.width / 2) + 15, HalfScreenHeight + 125, labelRect.width, menuGUISkin.label.fontSize + 5), " " + redAI.name, menuGUISkin.label);
        }

        if (blueAI)
        {
            Rect labelRect = GUILayoutUtility.GetRect(new GUIContent("     " + blueAI.name + "     "), menuGUISkin.label);
            GUI.Label(new Rect(HalfScreenWidth - labelRect.width - (versusLogo.width / 2), HalfScreenHeight + 125, labelRect.width, menuGUISkin.label.fontSize + 5), " " + blueAI.name, menuGUISkin.label);
        }

        #endregion

        #region Level Selection

        Rect levelWindowInner = new Rect(0, 0, buttonWidth, (buttonHeight * levels.Length) + boxBorderOffset);
        Rect levelScrollRect = new Rect(HalfScreenWidth - (buttonWidth / 2), 250, buttonWidth + (4 * boxBorderOffset), (buttonHeight * 3) + boxBorderOffset);

        GUI.Label(new Rect(HalfScreenWidth - (buttonWidth / 2), 215, buttonWidth, buttonHeight), "Choose a Level:", menuGUISkin.label);

        levelScrollView = GUI.BeginScrollView(levelScrollRect, levelScrollView, levelWindowInner);

        for(int i = 0; i < levels.Length; i++)
        {
            if (i == selectLevel)
                GUI.color = Color.cyan;
            else
                GUI.color = Color.white;

            if (GUI.Button(new Rect(0, i * buttonHeight, buttonWidth, buttonHeight), levels[i].name, menuGUISkin.button))
            {
                selectLevel = i;
            }
        }
        GUI.color = Color.white;
        GUI.EndScrollView();

        #endregion


        if (GUI.Button(new Rect(HalfScreenWidth - 100, HalfScreenHeight + 225, 200, 50), "Fight!", menuGUISkin.button) && blueAI && redAI)
        {
            PlayerPrefs.SetString("BLUE_AI", blueAI.name);
            PlayerPrefs.SetString("RED_AI", redAI.name);

            Application.LoadLevel(levels[selectLevel].name);
        }


    }

    public static float HalfScreenHeight
    {
        get { return Screen.height / 2; }
    }

    public static float HalfScreenWidth
    {
        get { return Screen.width / 2; }
    }
}
