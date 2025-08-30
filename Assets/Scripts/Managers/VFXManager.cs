using UnityEngine;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [SerializeField] private VFX[] vfxPrefabs;
    private Dictionary<VFXType, Queue<ParticleSystem>> vfxPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        vfxPool = new Dictionary<VFXType, Queue<ParticleSystem>>();
    }

    private void OnEnable()
    {
        GameEvents.OnFruitMerged += HandleFruitMerged;
    }

    private void OnDisable()
    {
        GameEvents.OnFruitMerged -= HandleFruitMerged;
    }

    private void HandleFruitMerged(FruitData fruitData, Vector3 position)
    {
        PlayVfx(VFXType.MergeEffect, position);
    }

    public void PlayVfx(VFXType vfxType, Vector3 position)
    {
        if (!vfxPool.ContainsKey(vfxType))
        {
            vfxPool[vfxType] = new Queue<ParticleSystem>();
        }

        ParticleSystem particleSystem;
        Vector3 spawnPosition = new Vector3(position.x, position.y, 0); // Force Z-axis to 0 for safety

        if (vfxPool[vfxType].Count > 0)
        {
            particleSystem = vfxPool[vfxType].Dequeue();
        }
        else
        {
            GameObject prefab = GetPrefabForType(vfxType);
            if (prefab == null) return;
            
            // Instantiate the prefab at the correct position from the start
            GameObject vfxInstance = Instantiate(prefab, spawnPosition, Quaternion.identity);
            particleSystem = vfxInstance.GetComponent<ParticleSystem>();
            
            var main = particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
            var trigger = vfxInstance.AddComponent<ParticleSystemCallback>();
            trigger.onParticleSystemStopped.AddListener(() => ReturnToPool(vfxType, particleSystem));
        }
        
        // This is a robust way to ensure it is not a child of anything
        particleSystem.transform.SetParent(null); 
        particleSystem.transform.position = spawnPosition;
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();
    }
    
    private void ReturnToPool(VFXType type, ParticleSystem particleSystem)
    {
        particleSystem.gameObject.SetActive(false);
        vfxPool[type].Enqueue(particleSystem);
    }

    private GameObject GetPrefabForType(VFXType vfxType)
    {
        foreach (var vfx in vfxPrefabs)
        {
            if (vfx.type == vfxType) return vfx.prefab;
        }
        Debug.LogError($"VFX prefab for type {vfxType} not found!");
        return null;
    }
}