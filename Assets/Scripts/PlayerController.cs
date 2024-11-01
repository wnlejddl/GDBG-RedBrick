using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    PlayerDirection direction;
    private SpriteRenderer spriteRenderer;

    Vector3 targetPosition;
    float moveSpeed = 5f; // 이동 속도

    private Coroutine moveCoroutine;


    // 애니메이션
    private Animator animator; // Animator 컴포넌트 참조

    bool isMoving;


    private void Start()
    {
        targetPosition = transform.position;

        isMoving = false;
        direction = PlayerDirection.Right;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isMoving){
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                direction = PlayerDirection.Left;
                Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(-1, 0, 0);

                if(!tilemap.HasTile(cellPos)){
                    targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.4f, 0); // 오프셋 추가
                }
                else{
                    Debug.Log("갈 수 없어요!");
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                direction = PlayerDirection.Right;
                Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(1, 0, 0);

                if(!tilemap.HasTile(cellPos)){
                    targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.4f, 0); // 오프셋 추가
                }            
                else{
                    Debug.Log("갈 수 없어요!");
                }
}

            // 스페이스바를 누를 때 점프 애니메이션 실행
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump"); // "Jump" Trigger를 활성화
                // Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(0, -1, 0);
                // targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.4f, 0); // 오프셋 추가
            }
            else if(Input.GetMouseButtonDown(0)){
                animator.SetTrigger("Attack");
            }
        }
        

        // 목표 위치와 현재 위치가 다를 때만 코루틴 실행
        if (transform.position != targetPosition)
        {
            // 이전 코루틴이 실행 중이라면 중지
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            isMoving = true;
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

        if (!HasTileBelow())
        {
            yield return StartCoroutine(MoveCharacterDownUntilTile());
        }
        moveCoroutine = null;
        isMoving = false;
    }


    IEnumerator MoveCharacterDownUntilTile()
    {

        Vector3Int belowCell = tilemap.WorldToCell(transform.position) + new Vector3Int(0, -1, 0);

        while (!tilemap.HasTile(belowCell))
        {
            belowCell = tilemap.WorldToCell(belowCell) + new Vector3Int(0, -1, 0);
        }

        targetPosition = tilemap.CellToWorld(belowCell)+ new Vector3Int(0, 1, 0) + new Vector3(0.5f, 0.4f, 0);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 한 칸 아래로 이동한 후 현재 위치 업데이트
            transform.position = targetPosition;
    }

    bool HasTileBelow()
    {
        Vector3Int currentCell = tilemap.WorldToCell(transform.position);
        Vector3Int belowCell = currentCell + new Vector3Int(0, -1, 0);
        return tilemap.HasTile(belowCell);
    }

    public void RemoveTile()
    {
        if(direction == PlayerDirection.Left){
            Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(-1, 0, 0);
           if(tilemap.HasTile(cellPos)) {
                tilemap.SetTile(cellPos, null);
           } 
        }
        else if(direction == PlayerDirection.Right){
            Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(1, 0, 0);
            if(tilemap.HasTile(cellPos)) {
                    tilemap.SetTile(cellPos, null);
            } 
        }

    }

    public void RemoveTileBelow()
    {
        Vector3Int cellPos = tilemap.WorldToCell(transform.position) + new Vector3Int(0, -1, 0);
        if(tilemap.HasTile(cellPos)) {
            tilemap.SetTile(cellPos, null);
        } 

        targetPosition = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.4f, 0); // 오프셋 추가

    }



}
