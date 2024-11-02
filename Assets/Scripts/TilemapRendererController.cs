using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class DisableInactiveTilemapRenderers : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드된 후 비활성화 지연 실행
        Invoke("DisableTilemapRenderersInInactiveScenes", 0.1f); // 0.1초 후 실행
    }

    void DisableTilemapRenderersInInactiveScenes()
    {
        // 활성화된 씬을 확인하고, 나머지 씬에서 TilemapRenderer를 비활성화
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene otherScene = SceneManager.GetSceneAt(i);
            if (!otherScene.isLoaded) // 비활성화된 씬만
            {
                GameObject[] rootObjects = otherScene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    // GameObject가 Grid인지 확인
                    Grid grid = obj.GetComponent<Grid>();
                    if (grid != null)
                    {
                        // Grid의 자식 타일맵을 찾음
                        TilemapRenderer[] renderers = obj.GetComponentsInChildren<TilemapRenderer>(true);
                        foreach (TilemapRenderer renderer in renderers)
                        {
                            renderer.enabled = false; // TilemapRenderer 비활성화
                            Debug.Log($"Disabled TilemapRenderer in scene: {otherScene.name} - {renderer.name}");
                        }
                    }
                }
            }
        }
    }
}
