using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class AddColliderToTilemapsEditor : EditorWindow
{
    [MenuItem("Window/Add Tilemap Colliders")]
    static void AddTilemapColliders()
    {
        // 선택된 오브젝트를 가져옴
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            // GameObject가 Grid인지 확인
            if (obj.GetComponent<Grid>() != null)
            {
                // Grid의 자식 타일맵을 찾음
                Tilemap[] tilemaps = obj.GetComponentsInChildren<Tilemap>();

                foreach (Tilemap tilemap in tilemaps)
                {
                    // 타일맵에 Tilemap Collider 2D 추가
                    if (tilemap.GetComponent<TilemapCollider2D>() == null)
                    {
                        tilemap.gameObject.AddComponent<TilemapCollider2D>();
                        Debug.Log($"Added Tilemap Collider 2D to {tilemap.name} in {obj.name}");
                    }
                }
            }
        }
    }
}
