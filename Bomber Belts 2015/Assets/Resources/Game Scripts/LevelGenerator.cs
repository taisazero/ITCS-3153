using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour {
    public GameObject beltPrefab;

    public GameObject redIndicatorPrefab, blueIndicatorPrefab;

    public GameObject redPlayerPrefab, bluePlayerPrefab;

    public int beltCount = 8;

    public Vector3 gameBoardCenter = Vector3.zero;

    public float beltSpacing = 2.4f;

    public List<GameObject> belts = new List<GameObject>();

    //These are here so that they will be serialized
    public GameObject gameScriptObject;

    public GameObject red, blue;
    public GameObject redIndicator, blueIndicator;
    
}
