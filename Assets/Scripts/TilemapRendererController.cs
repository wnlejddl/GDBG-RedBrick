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
        // �� �̸��� ���� ��Ȱ��ȭ�� �� �迭 ����
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
            if (scene.isLoaded) // ���� �ε�Ǿ� �ִ��� Ȯ��
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