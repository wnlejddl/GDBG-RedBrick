using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] GameObject gem;
    [SerializeField] GameObject strongTool;
    [SerializeField] GameObject bomb;

    List<GameObject> items;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로 생성된 오브젝트 파괴
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 인스턴스가 처음 생성될 때만 유지
    }

    void Start(){
        items  = new List<GameObject>(){gem, strongTool, bomb};
    }

    public GameObject GetGemPrefab(){
        return gem;
    }

    
    public GameObject GetRandomItem(){

        // 리스트에서 랜덤 아이템 뽑기로 추후 수정
        
        return gem;
    }


}
