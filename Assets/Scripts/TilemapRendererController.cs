using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapRendererController : MonoBehaviour
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
        // 씬 이름에 따라 비활성화할 씬 배열 정의
        switch (scene.name)
        {
            case "Ice Stage":
                DisableOtherTilemapRenderers("Earth Stage", "Lava Stage", "Goal Stage");
                break;
            case "Earth Stage":
                DisableOtherTilemapRenderers("Ice Stage", "Lava Stage", "Goal Stage");
                break;
            case "Lava Stage":
                DisableOtherTilemapRenderers("Ice Stage", "Earth Stage", "Goal Stage");
                break;
            case "Goal Stage":
                DisableOtherTilemapRenderers("Ice Stage", "Earth Stage", "Lava Stage");
                break;
            default:
                break;
        }
    }

    void DisableOtherTilemapRenderers(params string[] sceneNames)
    {
        foreach (string sceneName in sceneNames)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded) // 씬이 로드되어 있는지 확인
            {
                GameObject[] sceneObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in sceneObjects)
                {
                    TilemapRenderer[] renderers = obj.GetComponentsInChildren<TilemapRenderer>(true);
                    foreach (TilemapRenderer renderer in renderers)
                    {
                        renderer.enabled = false; // 타일맵 렌더러 비활성화
                    }
                }
                Debug.Log($"Tilemap Renderers disabled in scene: {sceneName}");
            }
        }
    }
}