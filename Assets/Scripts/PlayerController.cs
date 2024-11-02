using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Tilemap foreTile;
    [SerializeField] Tilemap middleTile;
    [SerializeField] Tilemap lastTile;

    List<Tilemap> tilemaps;


    private int blockWidth = 4; // 블록의 가로 크기 (타일 개수)
     private int blockHeight = 3;

    PlayerDirection direction;
    private SpriteRenderer spriteRenderer;

    Vector3 targetPosition;
    float moveSpeed = 10f;

    private Coroutine moveCoroutine;


    // 애니메이션
    private Animator animator; 

    bool isMoving= false;
    bool isAttacking = false;


    private void Start()
    {
        targetPosition = transform.position;
        direction = PlayerDirection.Right;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        tilemaps = new List<Tilemap>(){foreTile,middleTile, lastTile};
    }

    void Update()
    {
        if(!isMoving && !isAttacking){
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                direction = PlayerDirection.Left;
                Vector3Int cellPos = foreTile.WorldToCell(transform.position) + new Vector3Int(-blockWidth, 0, 0);

                if(GetRemainedBlock(cellPos)==-1){
                    targetPosition = foreTile.CellToWorld(cellPos) + new Vector3(0, 0.4f, 0);
                }
                else{
                    // 막힌 사운드 재생
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                direction = PlayerDirection.Right;
                Vector3Int cellPos = foreTile.WorldToCell(transform.position) + new Vector3Int(blockWidth, 0, 0);

                if(GetRemainedBlock(cellPos)==-1){
                    targetPosition = foreTile.CellToWorld(cellPos)+ new Vector3(0, 0.4f, 0);
                }            
                else{
                    // 막힌 사운드 재생
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump"); 
            }
            else if(Input.GetMouseButtonDown(0)){
                animator.SetTrigger("Attack");
                isAttacking = true;
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
        Vector3Int belowCell = foreTile.WorldToCell(transform.position) + new Vector3Int(0, -1, 0);

        while (!foreTile.HasTile(belowCell))
        {
            belowCell = foreTile.WorldToCell(belowCell) + new Vector3Int(0, -1, 0);
        }

        targetPosition = foreTile.CellToWorld(belowCell)+ new Vector3Int(0, 1, 0) + new Vector3(0.5f, 0.4f, 0);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 한 칸 아래로 이동한 후 현재 위치 업데이트
            transform.position = targetPosition;
    }

    int GetRemainedBlock(Vector3Int targetCell){

        for(int i=0;i<3;i++){
            if(tilemaps[i].HasTile(targetCell)) return i;
        }

        return -1;
    }

    bool HasTileBelow()
    {
        Vector3Int currentCell = foreTile.WorldToCell(transform.position);
        Vector3Int belowCell = currentCell + new Vector3Int(0, -1, 0);
        return foreTile.HasTile(belowCell);
    }

    public void RemoveTile()
    {
        Debug.Log("타일 삭제");
        Vector3Int currentPos = foreTile.WorldToCell(transform.position);
        if(direction == PlayerDirection.Left){
            Vector3Int targetCell = foreTile.WorldToCell(transform.position) + new Vector3Int(-blockWidth,0,0);
            int mapIndex = GetRemainedBlock(targetCell);

            if(mapIndex != -1){
                for(int i=-1*(blockWidth/2+blockWidth);i<-1*blockWidth/2;i++){
                    for(int j=0;j<blockHeight;j++){
                        Vector3Int cellPos = currentPos + new Vector3Int(i, j, 0);
                        if(tilemaps[mapIndex].HasTile(cellPos)) {
                            tilemaps[mapIndex].SetTile(cellPos, null);
                        } 
                    }
                }       
            }

  
        }
        else if(direction == PlayerDirection.Right){
            Vector3Int targetCell = foreTile.WorldToCell(transform.position) + new Vector3Int(blockWidth,0,0);
            int mapIndex = GetRemainedBlock(targetCell);
            if(mapIndex != -1){
                for(int i=blockWidth/2;i<blockWidth/2+blockWidth;i++){
                    for(int j=0;j<blockHeight;j++){
                        Vector3Int cellPos = currentPos+ new Vector3Int(i, j, 0);
                        if(tilemaps[mapIndex].HasTile(cellPos)) {
                            tilemaps[mapIndex].SetTile(cellPos, null);
                        } 
                    }
                }
            }
        }
    }

    public void RemoveTileBelow()
    {
        Vector3Int belowCell = foreTile.WorldToCell(transform.position) + new Vector3Int(0, -1, 0);
        int mapIndex = GetRemainedBlock(belowCell);
        if(mapIndex!=-1){
            Tilemap targetTileMap = tilemaps[mapIndex];
            for(int i=-blockWidth/2;i<blockWidth/2;i++){
            for(int j=1;j<=blockHeight;j++){
                Vector3Int cellPos = foreTile.WorldToCell(transform.position) + new Vector3Int(i, -j, 0);
                if(targetTileMap.HasTile(cellPos)) {
                    targetTileMap.SetTile(cellPos, null);
                } 
             }
            }
        }

        if(GetRemainedBlock(belowCell)==-1){
            targetPosition = foreTile.CellToWorld(foreTile.WorldToCell(transform.position) + new Vector3Int(0, -blockHeight, 0)) + new Vector3(0, 0.4f, 0); // 오프셋 추가
        }
      
    }

    public void OnAttackEnd()
    {
        RemoveTile();
        isAttacking = false;
    }

    public void OnJumpEnd()
    {
        RemoveTileBelow();
    }
}
