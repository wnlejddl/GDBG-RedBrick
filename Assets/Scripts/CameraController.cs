using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

    private Transform player; // 플레이어의 Transform
    [SerializeField] private float offsetY = 0.5f; // 카메라의 중간 위치 오프셋


    public float scrollSpeed = 2.0f; // 카메라 이동 속도
    bool stopFlag;


    [SerializeField] private float smoothTime = 1.3f; // 부드럽게 따라가는 속도
    private Vector3 velocity = Vector3.zero; // SmoothDamp의 현재 속도 저장

    private void Start() {
        stopFlag =false;

        if (SceneManager.GetActiveScene().buildIndex == 0) stopFlag = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if(!stopFlag){
            // 카메라를 아래로 이동
            transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
        }

         // 카메라 하단 1/4 지점 계산
        float cameraQuarterY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.25f, Camera.main.nearClipPlane)).y;

        // 플레이어가 카메라 하단 1/4 지점보다 아래에 있을 경우 카메라 이동
        if (player.position.y < cameraQuarterY)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        
    }

    public void Stop(){
        stopFlag = true;
    }

    public void StartCamera(){
        stopFlag = false;
    }
}