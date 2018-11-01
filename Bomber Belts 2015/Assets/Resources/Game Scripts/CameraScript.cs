using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    /// <summary>
    /// Amount to smooth the camera's movement by.
    /// </summary>                              
    public float smoothTime = 0.1f;

    Vector3 relativeOffset;                                     //The offset subtracted from the target's position = final target position
    Vector3 currentPosition, currentPositionV = Vector3.zero;   //Current position of the camera and its smoothing velocity
    Vector3 currentRotation, rotationV;                         //Current rotation and its smoothing velocity
    Vector3 initialPos;
	// Use this for initialization
	void Start () {
        initialPos = transform.position;
        currentPosition = initialPos;
	}


    void Update()
    {

        //Smooth the cam's current position and current rotation using smoothTime
        currentPosition = Vector3.SmoothDamp(currentPosition, initialPos, ref currentPositionV, smoothTime);

        //Handle shaking
        if (doShake)
        {
            float intensity = 1;

            if (!continuousShake)
                intensity = shakeTime / shakeDur;

            Vector3 shakePos = currentPosition + Random.insideUnitSphere * shakeMagn * intensity;

            currentPosition = Vector3.SmoothDamp(currentPosition, shakePos, ref shakeSmoothV, shakeSmooth);

            if (!continuousShake)
            {
                shakeTime -= Time.deltaTime;

                if (shakeTime < 0)
                    doShake = false;
            }
        }

        //Set the position and rotation
        transform.position = currentPosition;

    }


    float shakeMagn, shakeDur, shakeSmooth, shakeTime;
    Vector3 shakeSmoothV = Vector3.zero;
    bool doShake, continuousShake;

    /// <summary>
    /// Shake the camera for a specific amount of time, decaying over the course of the duration.
    /// </summary>
    /// <param name="magnitude">How much the camera should shake.</param>
    /// <param name="duration">How long the shake should last.</param>
    /// <param name="smoothing">How smooth the shake should be.</param>
    public void shakeOnce(float magnitude, float duration, float smoothing)
    {
        shakeMagn = magnitude;
        shakeDur = duration;
        shakeTime = shakeDur;
        shakeSmooth = smoothing;
        doShake = true;
    }

    /// <summary>
    /// Begin a constant shake that does not decay. Stop with stopShake()
    /// </summary>
    /// <param name="magnitude">How much the camera should shake.</param>
    /// <param name="smoothing">How smooth the shake should be.</param>
    public void startShake(float magnitude, float smoothing)
    {
        if (!doShake)
        {
            shakeMagn = magnitude;
            shakeSmooth = smoothing;
            doShake = true;
            continuousShake = true;
        }
    }

    /// <summary>
    /// Stop shaking, decaying over the course of fadeTime
    /// </summary>
    /// <param name="fadeTime">How long it should take to stop shaking</param>
    public void stopShake(float fadeTime)
    {
        if (doShake)
        {
            shakeDur = fadeTime;
            shakeTime = shakeDur;
            continuousShake = false;
        }
    }
}
