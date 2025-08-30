using UnityEngine;

public class ParticleSystemCallback : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onParticleSystemStopped;
    private void OnParticleSystemStopped()
    {
        onParticleSystemStopped.Invoke();
    }
}