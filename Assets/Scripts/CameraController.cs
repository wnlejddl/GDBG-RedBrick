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
    private bool stopFlag;

    [SerializeField] private float smoothTime = 1.3f; // 부드럽게 따라가는 속도
    private Vector3 velocity = Vector3.zero; // SmoothDamp의 현재 속도 저장

    private bool isFollowingPlayer = false; // 플레이어를 따라가는지 여부
    private float remainingFollowTime = 0f; // 남은 시간

    private void Start()
    {
        stopFlag = false;

        if (SceneManager.GetActiveScene().buildIndex == 0) stopFlag = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (!stopFlag)
        {
            // 카메라를 아래로 이동
            transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
        }

        if (remainingFollowTime > 0)
        {
            // 남은 시간이 있을 때 플레이어를 따라감
            isFollowingPlayer = true;
            remainingFollowTime -= Time.deltaTime;
            Debug.Log(remainingFollowTime);

            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            // 남은 시간이 없으면 따라가지 않음
            isFollowingPlayer = false;
            stopFlag = false; 

            // 카메라 하단 1/4 지점 계산
            float cameraQuarterY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.25f, Camera.main.nearClipPlane)).y;

            // 플레이어가 카메라 하단 1/4 지점보다 아래에 있을 경우 카메라 이동
            if (player.position.y < cameraQuarterY)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }

    public void Stop()
    {
        stopFlag = true;
    }

    public void StartCamera()
    {
        stopFlag = false;
    }

    public void SlowCamera(float duration)
    {
        stopFlag = true; // 카메라 수직 이동 멈추기

        // 남은 시간을 5초 추가
        remainingFollowTime += duration;
    }
}