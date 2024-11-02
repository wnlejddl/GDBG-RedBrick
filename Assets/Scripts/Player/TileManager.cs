
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    [SerializeField] Tilemap foreTile;
    [SerializeField] Tilemap middleTile;
    [SerializeField] Tilemap lastTile;
    [SerializeField] GameObject minPoint;

    private Color indestructibleColor = Color.black;

    List<Tilemap> tilemaps;
    
    private int blockWidth = 4; // 블록의 가로 크기 (타일 개수)
    private int blockHeight = 3;


    private void Start() {
        tilemaps = new List<Tilemap>(){foreTile,middleTile, lastTile};
        InitializeTileBlocks();
    }

    private void InitializeTileBlocks()
    {
        // 타일맵의 범위를 가져옵니다.
        BoundsInt bounds = foreTile.cellBounds;

        Vector3Int minCell = foreTile.WorldToCell(minPoint.transform.position);

        for (int x = minCell.x; x <= bounds.xMax - blockWidth; x += blockWidth)
        {
            for (int y = minCell.y; y <= bounds.yMax - blockHeight; y += blockHeight)
            {
                // 랜덤하게 4x3 블록을 검정색으로 설정
                if (Random.value < 0.2f) 
                {
                    SetIndestructibleBlock(x, y);
                }
                else if (Random.value < 0.2f) 
                {
                    SetEmptyBlock(x, y);
                }
                else if (Random.value < 0.3f) 
                {
                    SetItems(x+2, y+1);
                }
            }
        }
    }

    private void SetItems(int x, int y){
        Vector3 itemPos = foreTile.CellToWorld(new Vector3Int(x,y,0)) + new Vector3(0,0.5f,0) ;
        GameObject item = ItemManager.instance.GetRandomItem();
        Instantiate(item, itemPos, Quaternion.identity);
    }

    private void SetEmptyBlock(int startX, int startY){
        for (int x = startX; x < startX + blockWidth; x++)
        {
            for (int y = startY; y < startY + blockHeight; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (foreTile.HasTile(pos))
                {
                    foreach(Tilemap map in tilemaps){
                        map.SetTile(pos, null); 
                    }
                }
            }
        }
    }

    private void SetIndestructibleBlock(int startX, int startY)
    {
        
        for (int x = startX; x < startX + blockWidth; x++)
        {
            for (int y = startY; y < startY + blockHeight; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (foreTile.HasTile(pos))
                {
                    foreTile.SetTileFlags(pos, TileFlags.None);
                    foreTile.SetColor(pos, indestructibleColor); // 타일 색상 설정
                    foreTile.SetTileFlags(pos, TileFlags.LockColor); // 부술 수 없도록 설정
                }
            }
        }
    }


    public bool isIndestructible(Vector3 currentPos, Vector3Int direction){

        Vector3Int targetCell = foreTile.WorldToCell(currentPos);
        if(direction == Vector3Int.left || direction == Vector3Int.right){
            targetCell += blockWidth * direction;
        }
        else{
            targetCell += blockHeight * direction;
        }

        if (foreTile.HasTile(targetCell))
        {
            // 해당 위치의 타일 색상 가져오기
            Color tileColor = foreTile.GetColor(targetCell);

            // 타일 색상이 indestructibleColor인지 확인
            if (tileColor == indestructibleColor)
            {
                Debug.Log("깰 수 없어요");
                return true;
            }
        }

        return false;
    }


    int GetTilemapIndex(Vector3Int targetCell){

        for(int i=0;i<3;i++){
            if(tilemaps[i].HasTile(targetCell)) return i;
        }

        return -1;
    }

    Vector3Int GetCheckCell(Vector3 currentPos, Vector3Int direction){
        Vector3Int currentCell = foreTile.WorldToCell(currentPos); 
        return currentCell + blockHeight * direction;
    }


    public Vector3 GetNextMovementPos(Vector3 currentPos, Vector3Int direction){
        Vector3Int currentCell = foreTile.WorldToCell(currentPos); 

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
                return currentPos;
            }
        }
        else if (direction == Vector3Int.down){
           
            Vector3Int belowCell = lastTile.WorldToCell(currentPos) + new Vector3Int(0, -1, 0);
            
            while (!lastTile.HasTile(belowCell))
            {
                belowCell += new Vector3Int(0, -1, 0);
            }
            

            return lastTile.CellToWorld(belowCell)+ new Vector3Int(0, 1, 0) + new Vector3(0, 0.4f, 0);
        }

        return Vector3.negativeInfinity;
    }

    public bool HasTileLeft(Vector3 currentPos){
        Vector3Int checkCell = GetCheckCell(currentPos, Vector3Int.left);

        foreach(Tilemap tile in tilemaps){
            if(tile.HasTile(checkCell)) return true;
        }
        
        return false;
    }

    public bool HasTileRight(Vector3 currentPos){
        Vector3Int checkCell = GetCheckCell(currentPos, Vector3Int.right);

        foreach(Tilemap tile in tilemaps){
            if(tile.HasTile(checkCell)) return true;
        }
        
        return false;
    }
    public bool HasTileBelow(Vector3 currentPos){
        Vector3Int checkCell = GetCheckCell(currentPos, Vector3Int.down);

        foreach(Tilemap tile in tilemaps){
            if(tile.HasTile(checkCell)) return true;
        }
        
        return false;
    }



    public void RemoveTile(Vector3 currentPos, Vector3Int direction)
    {

        Vector3Int currentCell = foreTile.WorldToCell(currentPos); 
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
        else{
            
        }
    }
}