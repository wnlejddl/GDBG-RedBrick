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
    bool isJumping= false;
    bool isAttacking = false;


    private void Start()
    {
        targetPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isMoving && !isAttacking && !isJumping){
            if (Input.GetKeyDown(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                if(tileManager.HasTileLeft(transform)){
                    // 막힌 사운드 재생
                }
                else{
                    targetPosition = tileManager.GetNextMovementPos(transform, Vector3Int.left);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                if(tileManager.HasTileRight(transform)){
                    // 막힌 사운드 재생
                }
                else{
                    targetPosition = tileManager.GetNextMovementPos(transform,Vector3Int.right);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump"); 
                isJumping = true;
            }
            else if(Input.GetMouseButtonDown(0)){
                animator.SetTrigger("Attack");
                SoundController.instance.PlayAttackSound(true);
                isAttacking = true;
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
    
        if(!tileManager.HasTileBelow(transform)){
            isMoving = true;
            targetPosition = tileManager.GetNextMovementPos(transform, Vector3Int.down);
        }

    }


    public void OnAttackEnd()
    {
        SoundController.instance.StopSfx();
        Vector3Int direction = spriteRenderer.flipX ? Vector3Int.left : Vector3Int.right;
        if(!tileManager.isIndestructible(transform, direction)) {
            tileManager.RemoveTile(transform, direction);
        }
        else{
            // 깡 하는 소리
            Debug.Log("깰 수 없는 블록");
        }
        
        isAttacking = false;
    }

    public void OnJumpEnd()
    {
        SoundController.instance.PlayJumpDownSound();
        isJumping=false;

        if(!tileManager.isIndestructible(transform, Vector3Int.down)) {
            tileManager.RemoveTile(transform, Vector3Int.down);
            targetPosition = tileManager.GetNextMovementPos(transform, Vector3Int.down);

            if(targetPosition == Vector3.negativeInfinity) targetPosition = transform.position;
        }
        else{
            Debug.Log("깰 수 없는 블록");
        }

       
    }

    public void Dead(){
        animator.SetTrigger("Slide");
    }
}
