using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {

    [SerializeField]
    LevelGenerator core;

    void Awake()
    {
        core = target as LevelGenerator;
        
    }

    GameScript mainScript;
    Vector3 initialBeltPosition = Vector3.zero;

    // Update is called once per fram

    public void update()
    {
        clear();

        for (int i = 0; i < core.beltCount; i++)
        {
            core.belts.Add(Instantiate(core.beltPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        }

        core.red = Instantiate(core.redPlayerPrefab, Vector3.zero, Quaternion.Euler(0, 180, 0)) as GameObject;
        core.blue = Instantiate(core.bluePlayerPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)) as GameObject;

        core.redIndicator = Instantiate(core.redIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        core.blueIndicator = Instantiate(core.blueIndicatorPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        core.gameScriptObject = new GameObject();
        mainScript = core.gameScriptObject.AddComponent<GameScript>();

        mainScript.name = "GameScript";
        mainScript.belts = new BeltScript[core.belts.Count];

        mainScript.redPlayer = core.red.GetComponent<CharacterScript>();
        mainScript.bluePlayer = core.blue.GetComponent<CharacterScript>();

        mainScript.redHealthIndicator = core.redIndicator.GetComponent<HealthBarScript>();
        mainScript.blueHealthIndicator = core.blueIndicator.GetComponent<HealthBarScript>();

        for (int i = 0; i < core.belts.Count; i++)
        {
            mainScript.belts[i] = core.belts[i].GetComponent<BeltScript>();
        }

    }

    public void clear()
    {
        for (int i = 0; i < core.belts.Count; i++)
        {
            DestroyImmediate(core.belts[i]);
        }

        core.belts.Clear();

        DestroyImmediate(core.red);
        DestroyImmediate(core.blue);
        DestroyImmediate(core.redIndicator);
        DestroyImmediate(core.blueIndicator);
        DestroyImmediate(core.gameScriptObject);
    }

    public override void OnInspectorGUI()
    {  
        DrawDefaultInspector();
        EditorGUIUtility.LookLikeControls();

        EditorGUILayout.Separator();

        if (GUILayout.Button("Update"))
        {
            update();
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button("Clear"))
        {
            clear();
        }

        EditorGUILayout.Separator();

        if (core.gameScriptObject)
        {
            Rect warningRect = EditorGUILayout.BeginHorizontal();
            warningRect.height = 35;
            EditorGUI.HelpBox(warningRect, "The GameScript gameobject may need some extra setup to work properly.", MessageType.Warning);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        }

    }

    void OnSceneGUI()
    {
        core = target as LevelGenerator;
        if (core && core.belts.Count > 0)
        {
            if (core.beltCount % 2 == 1)
            {
                initialBeltPosition = new Vector3(core.gameBoardCenter.x, core.gameBoardCenter.y, (core.beltCount - 1) / 2 * -core.beltSpacing);
            }
            else
            {
                initialBeltPosition = new Vector3(core.gameBoardCenter.x, core.gameBoardCenter.y, ((core.beltCount / 2) * -core.beltSpacing) + (core.beltSpacing / 2));
            }

            for (int i = 0; i < core.belts.Count; i++)
            {
                core.belts[i].transform.position = initialBeltPosition + new Vector3(0, 0, i * core.beltSpacing);
            }

            Vector3 redStart = new Vector3(core.belts[core.belts.Count - 1].GetComponent<BeltScript>().redButton.transform.position.x + 1.5f, core.gameBoardCenter.y, core.belts[core.belts.Count - 1].GetComponent<BeltScript>().Position);
            Vector3 blueStart = new Vector3(core.belts[0].GetComponent<BeltScript>().blueButton.transform.position.x - 1.5f, core.gameBoardCenter.y, core.belts[0].GetComponent<BeltScript>().Position);

            core.red.transform.position = redStart;
            core.blue.transform.position = blueStart;

            Vector3 redIndicatorPos = redStart + new Vector3(0, 0, core.beltSpacing * 3);
            Vector3 blueIndicatorPos = blueStart + new Vector3(0, 0, core.beltSpacing * (core.beltCount + 2));

            core.redIndicator.transform.position = redIndicatorPos;
            core.blueIndicator.transform.position = blueIndicatorPos;
        }


    }
}
