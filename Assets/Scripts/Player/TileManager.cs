
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    [SerializeField] Tilemap foreTile;
    [SerializeField] Tilemap middleTile;
    [SerializeField] Tilemap lastTile;

    List<Tilemap> tilemaps;
    
    private int blockWidth = 4; // 블록의 가로 크기 (타일 개수)
    private int blockHeight = 3;


    private void Start() {
        tilemaps = new List<Tilemap>(){foreTile,middleTile, lastTile};
    }


    int GetTilemapIndex(Vector3Int targetCell){

        for(int i=0;i<3;i++){
            if(tilemaps[i].HasTile(targetCell)) return i;
        }

        return -1;
    }

    Vector3Int GetCheckCell(Transform currentPos, Vector3Int direction){
        Vector3Int currentCell = foreTile.WorldToCell(currentPos.position); 
        return currentCell + blockHeight * direction;
    }


    public Vector3 GetNextMovementPos(Transform currentPos, Vector3Int direction){
        Vector3Int currentCell = foreTile.WorldToCell(currentPos.position); 

        float cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, Camera.main.nearClipPlane)).x;
        float cameraRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane)).x;


        if(direction == Vector3Int.left || direction == Vector3Int.right){
            Vector3Int targetCell = currentCell + blockWidth * direction;
            Vector3 targetWorldPos = foreTile.CellToWorld(targetCell) + new Vector3(0, 0.4f, 0);

            // 이동할 위치가 카메라의 좌우 경계 내에 있는지 확인
            if (targetWorldPos.x >= cameraLeft && targetWorldPos.x <= cameraRight)
            {
                return targetWorldPos;
            }
            else
            {
                // 시야 밖으로 벗어날 경우 현재 위치 반환
                return currentPos.position;
            }
        }
        else if (direction == Vector3Int.down){
           
            Vector3Int belowCell = lastTile.WorldToCell(currentPos.position) + new Vector3Int(0, -1, 0);
            
            while (!lastTile.HasTile(belowCell))
            {
                belowCell += new Vector3Int(0, -1, 0);
            }
            

            return lastTile.CellToWorld(belowCell)+ new Vector3Int(0, 1, 0) + new Vector3(0, 0.4f, 0);
        }

        return Vector3.negativeInfinity;
    }

    public bool HasTileLeft(Transform currentPos){
        Vector3Int checkCell = GetCheckCell(currentPos, Vector3Int.left);

        foreach(Tilemap tile in tilemaps){
            if(tile.HasTile(checkCell)) return true;
        }
        
        return false;
    }

    public bool HasTileRight(Transform currentPos){
        Vector3Int checkCell = GetCheckCell(currentPos, Vector3Int.right);

        foreach(Tilemap tile in tilemaps){
            if(tile.HasTile(checkCell)) return true;
        }
        
        return false;
    }
    public bool HasTileBelow(Transform currentPos){
        Vector3Int checkCell = GetCheckCell(currentPos, Vector3Int.down);

        foreach(Tilemap tile in tilemaps){
            if(tile.HasTile(checkCell)) return true;
        }
        
        return false;
    }



    public void RemoveTile(Transform currentPos, Vector3Int direction)
    {

        Vector3Int currentCell = foreTile.WorldToCell(currentPos.position); 
        Vector3Int checkCell = currentCell + blockHeight * direction;

        int mapIndex = GetTilemapIndex(checkCell);

        if(mapIndex != -1){
            if(direction == Vector3Int.left){
                for(int i=-1*(blockWidth/2+blockWidth);i<-1*blockWidth/2;i++){
                    for(int j=0;j<blockHeight;j++){
                        Vector3Int targetCell = currentCell + new Vector3Int(i, j, 0);
                        tilemaps[mapIndex].SetTile(targetCell, null);
                    }
                }       
            }
            else if(direction == Vector3Int.right){
                for(int i=blockWidth/2;i<blockWidth/2+blockWidth;i++){
                    for(int j=0;j<blockHeight;j++){
                        Vector3Int targetCell = currentCell+ new Vector3Int(i, j, 0);
                        tilemaps[mapIndex].SetTile(targetCell, null);
                    }
                }
            }
            else if(direction == Vector3Int.down){
                for(int i=-blockWidth/2;i<blockWidth/2;i++){
                    for(int j=1;j<=blockHeight;j++){
                        Vector3Int targetCell = currentCell + new Vector3Int(i, -j, 0);
                        tilemaps[mapIndex].SetTile(targetCell, null);
                    }
                }
            }
        }
    }
}