using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 2.0f; // ī�޶� �̵� �ӵ�

    void Update()
    {
        // ī�޶� �Ʒ��� �̵�
        transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
    }
}