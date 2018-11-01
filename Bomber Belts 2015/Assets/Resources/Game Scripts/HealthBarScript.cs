using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

    float scaleIncrement;
    float currentHealth;
    int currentHealthInt;

    public GameObject barObj, scalingObj;
    public TextMesh healthText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Color healthColor = new Color((1 - currentHealth), currentHealth, currentHealth / 3f, 1f);
        barObj.GetComponent<Renderer>().material.color =  healthColor;



        if (currentHealth < 0.4f)
        {
            float pulseVal = (Mathf.Cos(Time.time / currentHealth) + 1) * 0.5f;
            healthText.color = new Color(1f, pulseVal, pulseVal); 
        }
	}

    public void setup(int startHealth)
    {
        scaleIncrement = 1f / startHealth;
        currentHealth = 1f;
        currentHealthInt = startHealth;
        scalingObj.transform.localScale = Vector3.one;

        healthText.text = currentHealthInt.ToString();
    }

    public void damage()
    {
        currentHealth -= scaleIncrement;
        currentHealthInt--;
        scalingObj.transform.localScale = new Vector3(currentHealth, 1, 1);
        healthText.text = currentHealthInt.ToString();
    }
}
