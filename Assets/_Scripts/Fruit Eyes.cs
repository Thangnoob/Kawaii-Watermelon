using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitEyes : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform rightPupil;
    [SerializeField] private Transform rightEye;
    [SerializeField] private Transform leftPupil;
    [SerializeField] private Transform leftEye;

    [Header(" Settings ")]
    [SerializeField] private float maxPupilDistance = 0.1f;
    private Transform target;

    private void Awake()
    {
        FruitManager.onFruitSpawned += FruitSpawnedCallback;
    }

    private void OnDestroy()
    {
        FruitManager.onFruitSpawned -= FruitSpawnedCallback;
    }

    private void FruitSpawnedCallback(Fruit fruit)
    {
        target = fruit.transform;
    }

    private void Update()
    {
        if (target == null)
            return;
        MoveEyes();
    }

    private void MoveEyes()
    {
        Vector3 targetPos = target.position;

        Vector3 rightPupilDirection = (targetPos - rightEye.position ).normalized;
        Vector3 rightPupilTargetLocalPos = rightPupilDirection * maxPupilDistance;

        rightPupil.localPosition = rightPupilTargetLocalPos;

        Vector3 leftPupilDirection = (targetPos - leftEye.position).normalized;
        Vector3 leftPupilTargetLocalPos = leftPupilDirection * maxPupilDistance;
        
        leftPupil.localPosition = leftPupilTargetLocalPos;

    }
}
