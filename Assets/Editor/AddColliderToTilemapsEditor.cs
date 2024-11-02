using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class AddColliderToTilemapsEditor : EditorWindow
{
    [MenuItem("Window/Add Tilemap Colliders")]
    static void AddTilemapColliders()
    {
        // ���õ� ������Ʈ�� ������
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject obj in selectedObjects)
        {
            // GameObject�� Grid���� Ȯ��
            if (obj.GetComponent<Grid>() != null)
            {
                // Grid�� �ڽ� Ÿ�ϸ��� ã��
                Tilemap[] tilemaps = obj.GetComponentsInChildren<Tilemap>();

                foreach (Tilemap tilemap in tilemaps)
                {
                    // Ÿ�ϸʿ� Tilemap Collider 2D �߰�
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
