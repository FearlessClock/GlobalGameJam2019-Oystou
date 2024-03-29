﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;

    public MemorableItem walkingStick;
    public MemorableItem clilmbingShoes;
    public MemorableItem waterWings;
    public MemorableItem gem1;
    public MemorableItem gem2;

    private AudioMixerSnapshot nothingSnapshot;
    private AudioMixerSnapshot theme1Snapshot;
    private AudioMixerSnapshot theme2Snapshot;
    private AudioMixerSnapshot theme3Snapshot;
    private AudioMixerSnapshot theme4Snapshot;
    private AudioMixerSnapshot theme5Snapshot;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    int musicPlayingRank = 0;

    private void Start()
    {

        DontDestroyOnLoad(this.gameObject);

        PlayerController.OnItemPlacedInFoyer += OnItemPlacedInFoyer;

        nothingSnapshot = mixer.FindSnapshot("Nothing");
        theme1Snapshot  = mixer.FindSnapshot("Theme1Playing");
        theme2Snapshot  = mixer.FindSnapshot("Theme2Playing");
        theme3Snapshot  = mixer.FindSnapshot("Theme3Playing");
        theme4Snapshot  = mixer.FindSnapshot("Theme4Playing");
        theme5Snapshot  = mixer.FindSnapshot("Theme5Playing");

        OnItemPlacedInFoyer(null);
    }

    private void OnItemPlacedInFoyer(GameObject item)
    {
        musicPlayingRank = 0;
        musicPlayingRank += PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + walkingStick.ID, 0);
        musicPlayingRank += PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + clilmbingShoes.ID, 0);
        musicPlayingRank += PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + waterWings.ID, 0);
        musicPlayingRank += PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + gem1.ID, 0);
        musicPlayingRank += PlayerPrefs.GetInt(PlayerPrefsStrings.itemId + gem2.ID, 0);
        SetSnapshotFromState();
    }

    public void SetSnapshotFromState()
    {
        switch (musicPlayingRank)
        {
            case 0:
                nothingSnapshot.TransitionTo(0.2f);
                break;
            case 1:
                theme1Snapshot.TransitionTo(0.2f);
                break;
            case 2:
                theme2Snapshot.TransitionTo(0.2f);
                break;
            case 3:
                theme3Snapshot.TransitionTo(0.2f);
                break;
            case 4:
                theme4Snapshot.TransitionTo(0.2f);
                break;
            case 5:
                theme5Snapshot.TransitionTo(0.2f);
                break;
            default:
                break;
        }
    }
}
