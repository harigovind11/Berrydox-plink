using System;
using UnityEngine;
using System.Collections.Generic;

    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance {get; private set;}
        
        [SerializeField] private VFX[] vfxPrefabs;
        private Dictionary<VFXType, Queue<ParticleSystem>> vfxPool;
        ParticleSystem particleSystem;
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

            if (vfxPool[vfxType].Count > 0)
            {
                particleSystem = vfxPool[vfxType].Dequeue();
                particleSystem.gameObject.SetActive(true);
            }
            else
            {
                GameObject prefab = GetPrefabForType(vfxType);
                if (prefab == null) return;
                GameObject vfxInstance = Instantiate(prefab);
                particleSystem=vfxInstance.GetComponent<ParticleSystem>();
                
                // The Particle System's Stop Action is set to Disable, which returns it to the pool
                var main = particleSystem.main;
                main.stopAction = ParticleSystemStopAction.Callback;
                var trigger = vfxInstance.AddComponent<ParticleSystemCallback>();
                trigger.onParticleSystemStopped.AddListener(() => ReturnToPool(vfxType, particleSystem));
            }
             particleSystem.transform.position = position;
             particleSystem.Play();
        }
        private void ReturnToPool(VFXType type, ParticleSystem particleSystem)
        {
            particleSystem.gameObject.SetActive(false);
            vfxPool[type].Enqueue(particleSystem);
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
