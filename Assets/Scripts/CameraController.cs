using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform
    [SerializeField] private float offsetY = 0.5f; // ī�޶��� �߰� ��ġ ������

    public float scrollSpeed = 2.0f; // ī�޶� �̵� �ӵ�
    private bool stopFlag;

    [SerializeField] private float smoothTime = 1.3f; // �ε巴�� ���󰡴� �ӵ�
    private Vector3 velocity = Vector3.zero; // SmoothDamp�� ���� �ӵ� ����

    private bool isFollowingPlayer = false; // �÷��̾ ���󰡴��� ����
    private float remainingFollowTime = 0f; // ���� �ð�

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
            // ī�޶� �Ʒ��� �̵�
            transform.position += new Vector3(0, -scrollSpeed * Time.deltaTime, 0);
        }

        if (remainingFollowTime > 0)
        {
            // ���� �ð��� ���� �� �÷��̾ ����
            isFollowingPlayer = true;
            remainingFollowTime -= Time.deltaTime;
            Debug.Log(remainingFollowTime);

            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            // ���� �ð��� ������ ������ ����
            isFollowingPlayer = false;
            stopFlag = false; 

            // ī�޶� �ϴ� 1/4 ���� ���
            float cameraQuarterY = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.25f, Camera.main.nearClipPlane)).y;

            // �÷��̾ ī�޶� �ϴ� 1/4 �������� �Ʒ��� ���� ��� ī�޶� �̵�
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
        stopFlag = true; // ī�޶� ���� �̵� ���߱�

        // ���� �ð��� 5�� �߰�
        remainingFollowTime += duration;
    }
}