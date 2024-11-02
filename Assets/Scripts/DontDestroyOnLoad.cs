using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad instance;

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
}
