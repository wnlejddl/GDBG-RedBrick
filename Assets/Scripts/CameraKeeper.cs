using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeeper : MonoBehaviour
{
    [SerializeField] private float duration = 5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 트리거에 닿았을 때만 실행
        if (other.CompareTag("Player"))
        {
            Camera.main.GetComponent<CameraController>().SlowCamera(duration);

            Destroy(gameObject);
        }
    }
}
