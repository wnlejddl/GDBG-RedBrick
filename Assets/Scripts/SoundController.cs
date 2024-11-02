using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Android;


public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource bgm;
    public static SoundController instance;

   [SerializeField] AudioClip jumpDown;
   [SerializeField] AudioClip money;
   [SerializeField] AudioClip attack;

    private void Awake() {
        if (instance != null)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로 생성된 오브젝트 파괴
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 인스턴스가 처음 생성될 때만 유지
    }

    private void Start() {
        
    }

    public void PlayAttackSound(bool loop){
        sfx.clip = attack;
        sfx.loop=loop;
        sfx.Play();
        
    }

    public void StopSfx(){
        sfx.Stop();
    }

    public void PlayJumpDownSound(){
        sfx.clip = jumpDown;
        sfx.Play();
    }

    public void PlayMoneySound(){
        sfx.loop = false;
        sfx.clip = money;
        sfx.Play();
    }

}
