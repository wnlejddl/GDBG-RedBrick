using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{

    [SerializeField] TileManager tileManager;

    private SpriteRenderer spriteRenderer;

    Vector3 targetPosition;
    float moveSpeed = 10f;

    private Coroutine moveCoroutine;
    

    // 애니메이션
    private Animator animator; 

    bool isMoving= false;

    bool isAttacking = false;

    Vector3 jumpStartPos;
    Vector3 startPos;

    private void Start()
    {
        targetPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        
    }

    void Instantiate(){
       if(!tileManager.HasTileBelow(transform.position)){
            targetPosition = tileManager.GetNextMovementPos(transform.position, Vector3Int.down);
        }
    }

    void Update()
    {
        if(!isMoving && !isAttacking){
            if (Input.GetKey(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                startPos = transform.position;
                
                if(Input.GetKeyDown(KeyCode.Space) ){
                    
                    animator.SetTrigger("Attack");
                    SoundController.instance.PlayAttackSound(true);
                    isAttacking = true;
                }
                else{
                    if(tileManager.HasTileLeft(startPos)){
                        // 막힌 사운드 재생
                    }
                    else{
                        targetPosition = tileManager.GetNextMovementPos(startPos, Vector3Int.left);
                    }
                }


            }
            else if (Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                startPos = transform.position;

                if(Input.GetKeyDown(KeyCode.Space)){
                    
                    animator.SetTrigger("Attack");
                    SoundController.instance.PlayAttackSound(true);
                    isAttacking = true;
                }
                
                if(tileManager.HasTileRight(startPos)){
                    // 막힌 사운드 재생
                }
                else{
                    targetPosition = tileManager.GetNextMovementPos(startPos,Vector3Int.right);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpStartPos = transform.position;
                animator.SetTrigger("Jump"); 
            }
        }

        if(targetPosition == Vector3.negativeInfinity) targetPosition = transform.position;
        

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

        moveCoroutine = null;
        isMoving = false;
    
        if(!tileManager.HasTileBelow(transform.position)){
            targetPosition = tileManager.GetNextMovementPos(transform.position, Vector3Int.down);
        }
    }


    public void OnAttackEnd()
    {
        SoundController.instance.StopSfx();
        Vector3Int direction = spriteRenderer.flipX ? Vector3Int.left : Vector3Int.right;
        if(!tileManager.isIndestructible(transform.position, direction)) {
            tileManager.RemoveTile(transform.position, direction);
        }
        else{
            // 깡 하는 소리
            Debug.Log("깰 수 없는 블록");
        }
        
        isAttacking = false;
    }

    public void OnJumpEnd()
    {
        if(!tileManager.HasTileBelow(jumpStartPos)) return;

        SoundController.instance.PlayJumpDownSound();

        if(!tileManager.isIndestructible(jumpStartPos, Vector3Int.down)) {
            tileManager.RemoveTile(jumpStartPos, Vector3Int.down);
            targetPosition = tileManager.GetNextMovementPos(transform.position, Vector3Int.down);

            if(targetPosition == Vector3.negativeInfinity) {
                targetPosition = transform.position;
            }
        }
        else{
            Debug.Log("깰 수 없는 블록");
        }

       
    }

    public void Dead(){
        animator.SetTrigger("Slide");
    }

    public void BreakAllBelow(int range){
        tileManager.RemoveAllTile(targetPosition, range);
    }
}
