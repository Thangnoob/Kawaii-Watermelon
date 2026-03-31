using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Object/Level Data", order = 1)]
public class LevelDataSO : ScriptableObject
{
    [Header("Level Data")]
    [SerializeField] private GameObject levelMapPrefab;
    [SerializeField] private int hightScoreRequired;

    public GameObject LevelMapPrefab => levelMapPrefab;
    public int HightScoreRequired => hightScoreRequired;

}
