using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 2.0f; // 카메라 이동 속도
    bool stopFlag;

    private void Start() {
        stopFlag =false;
    }

    void Update()
    {
        if(!stopFlag){
            // 카메라를 아래로 이동
            transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
        }
        
    }

    public void Stop(){
        stopFlag = true;
    }
}