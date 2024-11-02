using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int range = 3; // 아이템 점수 값

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 트리거에 닿았을 때만 실행
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().BreakAllBelow(range);

            Destroy(gameObject);
        }
    }
}
