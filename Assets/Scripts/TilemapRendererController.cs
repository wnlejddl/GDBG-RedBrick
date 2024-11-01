using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapRendererController : MonoBehaviour
{
    // ���� ���� ���� Ȱ��ȭ�� ���� Ȯ���ϰ� ��Ȱ��ȭ�� ������ ����
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
        // �� �̸��� ���� ��Ȱ��ȭ�� �� �迭 ����
        switch (scene.name)
        {
            case "Ice Stage":
                DisableTilemapRenderers("Earth Stage", "Lava Stage", "Goal Stage");
                break;
            case "Earth Stage":
                DisableTilemapRenderers("Ice Stage", "Lava Stage", "Goal Stage");
                break;
            case "Lava Stage":
                DisableTilemapRenderers("Ice Stage", "Earth Stage", "Goal Stage");
                break;
            case "Goal Stage":
                DisableTilemapRenderers("Ice Stage", "Earth Stage", "Lava Stage");
                break;
            default:
                break;
        }
    }

    void DisableTilemapRenderers(params string[] sceneNames)
    {
        foreach (string sceneName in sceneNames)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                GameObject[] sceneObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in sceneObjects)
                {
                    TilemapRenderer[] renderers = obj.GetComponentsInChildren<TilemapRenderer>(true);
                    foreach (TilemapRenderer renderer in renderers)
                    {
                        renderer.enabled = false; // Ÿ�ϸ� ������ ��Ȱ��ȭ
                    }
                }
                Debug.Log($"Tilemap Renderers disabled in scene: {sceneName}");
            }
        }
    }
}