using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private LeaderboardMemberContainer leaderboardMemberContainerPrefab;
    [SerializeField] private Transform leaderboardMemberContainerParent;

    private void Start()
    {
        Leaderboard.onLeaderboardFetched += LeaderboadFetchedCallback;
    }

    private void OnDestroy()
    {
        Leaderboard.onLeaderboardFetched -= LeaderboadFetchedCallback;
    }

    private void LeaderboadFetchedCallback(LootLockerLeaderboardMember[] members)
    {
        Debug.Log("Leaderboard fetched with " + members.Length + " members.");
        for (int i = 0; i < members.Length; i++)
        {
           if (leaderboardMemberContainerParent.childCount <= i)
           {
                CreateMemberContainer(members[i]);
            } else 
                UpdateMemberContainer(i, members[i]);
        }

        while (leaderboardMemberContainerParent.childCount > members.Length)
        {
            Transform t = leaderboardMemberContainerParent.GetChild(leaderboardMemberContainerParent.childCount - 1);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
    }
    private void CreateMemberContainer(LootLockerLeaderboardMember member)
    {
        LeaderboardMemberContainer container = Instantiate(leaderboardMemberContainerPrefab, leaderboardMemberContainerParent);
        ConfigureContainer(container, member);
    }
    private void UpdateMemberContainer(int containerIndex, LootLockerLeaderboardMember member)
    {
        LeaderboardMemberContainer container = leaderboardMemberContainerParent.GetChild(containerIndex).GetComponent<LeaderboardMemberContainer>();
        ConfigureContainer(container, member);
    }

    private void ConfigureContainer(LeaderboardMemberContainer container, LootLockerLeaderboardMember member)
    {
        container.Configure(member.rank, GetPlayerName(member), member.score);
    }

    private string GetPlayerName(LootLockerLeaderboardMember member)
    {
        return member.player.name.Length > 0 ? member.player.name : "Guest" + member.member_id;
    }

}
