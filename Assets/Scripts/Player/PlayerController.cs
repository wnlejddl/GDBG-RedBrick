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


    private void Start()
    {
        targetPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isMoving && !isAttacking){
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
            }
            else if(Input.GetMouseButtonDown(0)){
                animator.SetTrigger("Attack");
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
    }


    public void OnAttackEnd()
    {
        tileManager.RemoveTile(transform, spriteRenderer.flipX ? Vector3Int.left : Vector3Int.right);
        isAttacking = false;
    }

    public void OnJumpEnd()
    {
        tileManager.RemoveTile(transform, Vector3Int.down);
        targetPosition = tileManager.GetNextMovementPos(transform, Vector3Int.down);
        Debug.Log(targetPosition);
        if(targetPosition == Vector3.negativeInfinity) targetPosition = transform.position;
    }
}
