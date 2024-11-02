using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    Collider2D playerCollider;
    PlayerController playerController;
    CameraController cameraController;
    private Camera mainCamera;

    private int score;

    public static GameManager instance;

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

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<CameraController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && IsPlayerOutOfCamera())
        {
            UIManager.i.ShowRestartButton();
            playerController.Dead();
            cameraController.Stop();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 Player 오브젝트를 찾음
        player = GameObject.FindWithTag("Player");
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<CameraController>();

        if (player == null)
        {
            Debug.LogWarning("Player not found in the current scene!");
        }
        else
        {
            Debug.Log("Player assigned successfully.");
            playerCollider = player.GetComponent<Collider2D>();
            playerController = player.GetComponent<PlayerController>();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private bool IsPlayerOutOfCamera()
    {
        // 캐릭터의 상단 위치 계산
        float playerTop = playerCollider.bounds.max.y;
        

        // 카메라의 상단 경계 계산
        float cameraTop = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, mainCamera.nearClipPlane)).y;


        return playerTop > cameraTop;
    }


    public void RestartGame()
    {
        SoundController.instance.StopSfx();
        SceneManager.LoadScene("Ice Stage");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Ice Stage");
    }

    public void AddScore(int pt){
        score += pt;
        Debug.Log("현재 점수 : " + score);
    }

}
