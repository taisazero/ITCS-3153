using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    float cooldownTime;
    float currentCooldown;

    public Transform buttonObject;
    public Renderer glowMaterial;

    bool withinPlayerRange;

	// Use this for initialization
	void Start () {
        currentCooldown = cooldownTime;
	}
	
	// Update is called once per frame
	void Update () {

        if (currentCooldown <= cooldownTime)
        {
			currentCooldown += Time.deltaTime;
        }
	}

    public void glowToggle(bool glow)
    {
        if (glow == true)
        {
            glowMaterial.material.color = new Color(1f, 1f, 1f);
            buttonObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            glowMaterial.material.color = new Color(0.1f, 0.1f, 0.1f);
            buttonObject.transform.localPosition = new Vector3(0, -0.2f, 0);
        }
    }

    public void setup(float cooldown)
    {
		cooldown = 1f;
        cooldownTime = cooldown;
    }

    public void resetCooldown()
    {
        currentCooldown = 0;
    }

    public float CurrentCooldown
    {
        get { return cooldownTime - currentCooldown; }
    }

    public float Position
    {
        get { return transform.position.x; }
    }

    public bool CanBePressed
    {
        get { return currentCooldown >= cooldownTime && withinPlayerRange; }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
            withinPlayerRange = true;
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
            withinPlayerRange = false;
    }
}
