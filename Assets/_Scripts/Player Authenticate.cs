using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using System;

public class PlayerAuthenticate : MonoBehaviour
{
    public static PlayerAuthenticate Instance { get; private set; }

    public string PlayerId { get; private set; }

    [Header(" Actions ")]
    public static Action onPlayerAuthenticated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(LoginCoroutine());
    }
    private void Start()
    {
        
    }

    IEnumerator LoginCoroutine()
    {
        Debug.Log("Authenticating player...");
        bool done = false; 

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player Authenticated\n" +
                    "Player ID: " + response.player_id + "\n" +
                    "Session Token: " + response.session_token + "\n" +
                    "Player Name: " + response.player_name + "\n" +
                    "Player Public UID: " + response.public_uid);
                PlayerId = response.player_id.ToString();
                done = true; 
                onPlayerAuthenticated?.Invoke();
            }
            else
            {
                Debug.LogError("Failed to authenticate player: " + response.errors);
                done = true; 
            }
        });
        yield return new WaitWhile(() => done);
    }
}
