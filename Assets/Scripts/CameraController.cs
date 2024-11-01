using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 2.0f; // 카메라 이동 속도

    void Update()
    {
        // 카메라를 아래로 이동
        transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
    }
}