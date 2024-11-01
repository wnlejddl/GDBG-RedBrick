using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;


    [SerializeField] Sprite downDirectionSprite;
    private SpriteRenderer spriteRenderer;

    Vector3 targetPosition;
    Vector3 currentPosition;
    float moveSpeed = 5f; // 이동 속도

    private Coroutine moveCoroutine;

    private void Start()
    {
        // 초기 위치를 타일맵의 셀 위치로 변환하여 설정
        currentPosition = tilemap.CellToWorld(tilemap.WorldToCell(transform.position));
        targetPosition = currentPosition;


        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(-1, 0, 0);
            targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0, 0); // 오프셋 추가

            spriteRenderer.flipX = true;

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(1, 0, 0);
            targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0, 0); // 오프셋 추가

            spriteRenderer.flipX = false;

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(0, -1, 0);
            targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0, 0); // 오프셋 추가

            spriteRenderer.sprite = downDirectionSprite;
        }

        // 목표 위치와 현재 위치가 다를 때만 코루틴 실행
        if (currentPosition != targetPosition)
        {
            // 이전 코루틴이 실행 중이라면 중지
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            // 새로운 코루틴 시작
            moveCoroutine = StartCoroutine(MoveCharacter(targetPosition));
        }
    }

    IEnumerator MoveCharacter(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 목표 위치에 도달했을 때 정확하게 위치 맞추기
        transform.position = targetPos;
        currentPosition = targetPos; // 현재 위치 업데이트
        moveCoroutine = null; // 코루틴 초기화
    }


}
