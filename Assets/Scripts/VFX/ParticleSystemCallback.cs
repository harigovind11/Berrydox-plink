using UnityEngine;

public class ParticleSystemCallback : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onParticleSystemStopped = new UnityEngine.Events.UnityEvent();
    private void OnParticleSystemStopped()
    {
        onParticleSystemStopped.Invoke();
    }
}