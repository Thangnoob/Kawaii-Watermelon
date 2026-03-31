using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedWall : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;

    private void Start()
    {
        SetupWall();
    }

    private void SetupWall()
    {
        float aspect = (float)Screen.width / Screen.height;

        Camera camera = Camera.main;

        float sceneWidth = camera.orthographicSize * aspect;
        
        leftWall.transform.position = new Vector3(sceneWidth + 0.5f, 2f, 0);
        rightWall.transform.position = new Vector3(-(sceneWidth + 0.5f), 2f, 0);
    }
}