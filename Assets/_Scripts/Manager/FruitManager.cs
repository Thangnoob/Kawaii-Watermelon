using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    public static FruitManager Instance;

    [Header(" Elements ")]
    [SerializeField] private SkinDataSO skinData;
    [SerializeField] private Transform fruitParent;
    [SerializeField] private LineRenderer spawnFruitLine;
    private Fruit currentFruit;

    [Header(" Settings ")]
    [SerializeField] private float spawnYPosition;
    [SerializeField] private float spawnDelay = 0.5f;
    private bool canControl;
    private bool isControlling;

    [Header(" Next Fruit Settings ")]
    private int nextFruitIndex;

    [Header( "Debug" )]
    [SerializeField] private bool enableGizmos;

    [Header(" Actions ")]
    public static Action onNextFruitIndexSet;
    public static Action<Fruit> onFruitSpawned;

    private void Awake()
    {
        if ( Instance == null ) 
            Instance = this;
        else 
            Destroy(gameObject);
        
        MergeManager.onMergeProcessed += MergeProgressCallback;
        ShopManager.onSkinSeleted += SkinSeletedCallback;
    }

    private void Start()
    {
        canControl = true;
        HideLine();

        SetNextFruitIndex();
    }


    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProgressCallback;    
        ShopManager.onSkinSeleted -= SkinSeletedCallback;
    }
    private void SkinSeletedCallback(SkinDataSO skinDataSeleted)
    {
        skinData = skinDataSeleted;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsInGameState())
            return;

        if ( canControl )
        {
            ManagePlayerInput();
        }
    }

    //=========================================
    //MOUSE INPUT 
    //==========================================
    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDownCallBack();
        
        else if (Input.GetMouseButton(0))
        {
            if (isControlling)
            {
                MouseDragCallBack();
            }
            else
                MouseDownCallBack();
        }
        
        else if (Input.GetMouseButtonUp(0) && isControlling)
            MouseUpCallBack();
    }
    private void MouseDownCallBack()
    {
        if (!isClickDetected())
            return;

        ShowLine();
        PlaceLineAtClickedPosition();

        SpawnFruit();
        isControlling = true;
    }

    private void MouseDragCallBack()
    {
        PlaceLineAtClickedPosition();
        currentFruit.MoveToPosition(GetSpawnPosition());
    }

    private void MouseUpCallBack()
    {
        HideLine();

        currentFruit.EnablePhysics();

        StartControlTimer();

        isControlling = false;
    }

    private bool isClickDetected()
    {
        Vector2 mousePos = Input.mousePosition;

        return mousePos.y > Screen.height / 4 && mousePos.y < Screen.height - (Screen.height / 4.5);
    }
    private void StartControlTimer()
    {
        canControl = false;
        Invoke("StopControlTimer", spawnDelay);  
    }

    private void StopControlTimer()
    {
        canControl = true;
    }

    //=========================================
    //SPAWN
    //=========================================
    private void SpawnFruit()
    {
        Vector2 spawnPosition = GetSpawnPosition();

        currentFruit = Instantiate(
            skinData.GetSpawnablePrefabs()[nextFruitIndex], 
            spawnPosition, 
            Quaternion.identity, 
            fruitParent);

        SetNextFruitIndex();
        onFruitSpawned?.Invoke(currentFruit);
    }
    //==========================================
    //SPAWN LINE 
    //==========================================
    private void HideLine()
    {
        spawnFruitLine.enabled = false;
    }

    private void ShowLine()
    {
        spawnFruitLine.enabled = true;
    }

    private void PlaceLineAtClickedPosition()
    {
        spawnFruitLine.SetPosition(0, GetSpawnPosition());
        spawnFruitLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15);
    }

    //==========================================
    // POSITION 
    //==========================================
    private Vector2 GetClickedPositionInput()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 worldClickedPosition = GetClickedPositionInput();
        worldClickedPosition.y = spawnYPosition;
        return worldClickedPosition;
    }

    //===========================================
    //MERGE CALLBACK
    //===========================================
    private void MergeProgressCallback(FruitType type, Vector2 position)
    {
        for (int i = 0; i < skinData.GetObjectPrefabs().Length; i++)
        {
            if (skinData.GetObjectPrefabs()[i].GetFruitType() == type)
            {
                SpawnMergedFruit(skinData.GetObjectPrefabs()[i], position);
                break;
            }
        }
    }

    private void SpawnMergedFruit(Fruit spawnFruit, Vector2 spawnPosition)
    {
        Fruit mergedFruit = Instantiate(spawnFruit, spawnPosition, Quaternion.identity, fruitParent);
        mergedFruit.EnablePhysics();
        onFruitSpawned?.Invoke(mergedFruit);
    }
    //===========================================
    //NEXT FRUIT GET/SET
    //===========================================
    private void SetNextFruitIndex()
    {
        nextFruitIndex = Random.Range(0, skinData.GetSpawnablePrefabs().Length);

        onNextFruitIndexSet?.Invoke();
    }

    public string GetNextFruitName()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].GetFruitType().ToString();
    }

    public Sprite GetNextFruitSprite()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].GetFruitSprite();
    }
    //==========================================
    //GET FRUIT FOR POWERUP
    //==========================================
    public Fruit[] GetSmallFruitForBlast()
    {
        List<Fruit> smallFruits = new List<Fruit>();

        for (int i = 0; i < fruitParent.childCount; i++)
        {
            Fruit fruit = fruitParent.GetChild(i).GetComponent<Fruit>();
            int fruitType = (int)fruit.GetFruitType();
            if (fruitType < 3)
            {
                smallFruits.Add(fruit);
            }
        }
        return smallFruits.ToArray();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-50, spawnYPosition, 0), new Vector3(50, spawnYPosition, 0));
    }
#endif
}
