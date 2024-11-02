using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{    
    [SerializeField] Button restart;


    public static UIManager i;

    private void Awake()
    {
        if (i != null)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로 생성된 오브젝트 파괴
            return;
        }

        i = this;
        DontDestroyOnLoad(gameObject); // 인스턴스가 처음 생성될 때만 유지
    }

    void Start()
    {
        if(i==null){
            i = this;
        }
        
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      restart.gameObject.SetActive(false);
    }
 

    public void ShowRestartButton()
    {
        restart.gameObject.SetActive(true);
    }

    public void HideRestartButton(){
        restart.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}