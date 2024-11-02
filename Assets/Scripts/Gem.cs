using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10; // 아이템 점수 값

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 트리거에 닿았을 때만 실행
        if (other.CompareTag("Player"))
        {
            // 점수 증가
            GameManager.instance.AddScore(scoreValue);
            SoundController.instance.PlayMoneySound();
            // 아이템 오브젝트 삭제
            Destroy(gameObject);
        }
    }
}