using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    
    [SerializeField] private Sound[] sounds;
    private AudioSource audioSource;
    private Dictionary<SoundType,AudioClip> soundDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance =  this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        soundDict = new Dictionary<SoundType, AudioClip>();

        foreach (var sound in sounds)
        {
            soundDict[sound.type] = sound.clip;
        }
    }

    private void OnEnable()
    {
        // Subscribe to game events
        GameEvents.OnFruitDropped += HandleFruitDrop;
        GameEvents.OnFruitMerged += HandleFruitMerge;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameEvents.OnFruitDropped -= HandleFruitDrop;
        GameEvents.OnFruitMerged -= HandleFruitMerge;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleFruitDrop()
    {
        PlaySound(SoundType.FruitDrop);
    }

    private void HandleFruitMerge(FruitData fruitData, Vector3 position)
    {
        PlaySound(SoundType.FruitMerge);
    }

    private void HandleGameOver()
    {
        PlaySound(SoundType.GameOver);
    }
    public void PlaySound(SoundType type)
    {
        if (soundDict.TryGetValue(type, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound type {type} not found in dictionary!");
        }
    }
}
