using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitEyesForCherry : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform rightPupil1;
    [SerializeField] private Transform rightEye1;
    [SerializeField] private Transform leftPupil1;
    [SerializeField] private Transform leftEye1; 
    [SerializeField] private Transform rightPupil2;
    [SerializeField] private Transform rightEye2;
    [SerializeField] private Transform leftPupil2;
    [SerializeField] private Transform leftEye2;

    [Header(" Settings ")]
    [SerializeField] private float maxPupilDistance = 0.1f;
    private Transform target;
    private void Awake()
    {
        FruitManager.onFruitSpawned += FruitSpawnedCallback;
    }
    private void Update()
    {
        if (target == null)
            return;
        MoveEyes();
    }

    private void OnDestroy()
    {
        FruitManager.onFruitSpawned -= FruitSpawnedCallback;
    }

    private void FruitSpawnedCallback(Fruit fruit)
    {
        target = fruit.transform;
    }


    private void MoveEyes()
    {
        Vector3 targetPos = target.position;

        Vector3 rightPupilDirection1 = (targetPos - rightEye1.position ).normalized;
        Vector3 rightPupilTargetLocalPos1 = rightPupilDirection1 * maxPupilDistance;

        rightPupil1.localPosition = rightPupilTargetLocalPos1;

        Vector3 leftPupilDirection1 = (targetPos - leftEye1.position).normalized;
        Vector3 leftPupilTargetLocalPos1 = leftPupilDirection1 * maxPupilDistance;
        
        leftPupil1.localPosition = leftPupilTargetLocalPos1;

        Vector3 rightPupilDirection2 = (targetPos - rightEye2.position).normalized;
        Vector3 rightPupilTargetLocalPos2 = rightPupilDirection2 * maxPupilDistance;

        rightPupil2.localPosition = rightPupilTargetLocalPos2;

        Vector3 leftPupilDirection2 = (targetPos - leftEye2.position).normalized;
        Vector3 leftPupilTargetLocalPos2 = leftPupilDirection2 * maxPupilDistance;

        leftPupil2.localPosition = leftPupilTargetLocalPos2;

    }
}
