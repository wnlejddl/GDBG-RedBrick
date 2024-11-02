using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 2.0f; // ī�޶� �̵� �ӵ�
    bool stopFlag;

    private void Start() {
        stopFlag =false;
    }

    void Update()
    {
        if(!stopFlag){
            // ī�޶� �Ʒ��� �̵�
            transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
        }
        
    }

    public void Stop(){
        stopFlag = true;
    }
}