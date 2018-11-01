using UnityEngine;
using System.Collections;

public class RotateRandomly : MonoBehaviour {

    public Vector3 rotationAmount = new Vector3(0.05f, 0.1f, 0.11f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rotationVector = new Vector3(Mathf.Sin(Time.time * rotationAmount.x), Mathf.Cos(Time.time * rotationAmount.y), -Mathf.Sin(Time.time * rotationAmount.z)) * 0.1f;
        transform.Rotate(rotationVector);
	}
}
