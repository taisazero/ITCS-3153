using UnityEngine;

[AddComponentMenu("Custom Scripts/Utility/Auto Destruct Particle System")]
public class AutoDestructParticleSystem : MonoBehaviour
{
    void LateUpdate()
    {
        if (!GetComponent<ParticleSystem>().IsAlive())
            Object.Destroy(this.gameObject);
    }
}	
