using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RoomPrefabs
{
    public List<GameObject> SquareEmptyPieceGroup;
    public List<GameObject> SquareEdgePieceGroup;
    public GameObject[] SquareCornerPieceGroup;
    public GameObject[] SquareHallwayPieceGroup;
    public GameObject[] SquareDoorwayPieceGroup;
    public GameObject[] SquareDoorwayLeftCornerPieceGroup;
    public GameObject[] SquareDoorwayRightCornerPieceGroup;
    public GameObject[] SquareDeadendPieceGroup;
}

[System.Serializable]
public class TileData : MonoBehaviour
{
    Cell cell;
    public RoomPrefabs[] roomPrefabs;
    //public List<RoomPrefabs> roomPrefabs = new List<RoomPrefabs> ();

    [Header("Hallway, DoorwayLeftCorner, DoorwayRightCorner")]
    [Header("Empty, Corner, Edge, Doorway")]

    public PieceType type;

    public GameObject[] SquareEmptyPieceGroup;
    public GameObject[] SquareEdgePieceGroup;
    public GameObject[] SquareCornerPieceGroup;
    public GameObject[] SquareHallwayPieceGroup;
    public GameObject[] SquareDoorwayPieceGroup;
    public GameObject[] SquareDoorwayLeftCornerPieceGroup;
    public GameObject[] SquareDoorwayRightCornerPieceGroup;
    public GameObject[] SquareDeadendPieceGroup;

    private GameObject SquareEmptyPiece;
    private GameObject SquareEdgePiece;
    private GameObject SquareCornerPiece;
    private GameObject SquareHallwayPiece;
    private GameObject SquareDoorwayPiece;
    private GameObject SquareDoorwayLeftCornerPiece;
    private GameObject SquareDoorwayRightCornerPiece;
    private GameObject SquareDeadendPiece;
    private GameObject tileMesh;
    public GameObject nullObj;

    public GameObject pieceToSpawn;
    public float rotation = 0;
    //public int tileX;
    //public int tileY;
    public Vector2Int tilePosition;
    private Vector2 minPosition;
    private Vector2 maxPosition;
    public bool isRound;
    public int roomType;
    private int prefabNum = 0;
    public Contents current, front, left, right, back;
    public void Initialize(Cell cell, int roomNum)
    {
        int num = roomNum - 1;
        FigureOutRoomTheme(cell, num);
        FigureOutPieceGroup();
        SpawnMesh();
        cell.prefabNumber = prefabNum;
    }
    public void Initialize(Cell cell)
    {
        FigureOutPiece();
        SpawnMesh();
    }
    private void SpawnMesh()
    {

        tileMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
        tileMesh.transform.parent = this.transform;
    }

    private void FigureOutRoomTheme(Cell cell, int roomNum)
    {
        roomType = roomNum;
    }
    public void CheckSurrounding(Cell[,] grid, Vector2 min, Vector2 max)
    {
        current = Contents.Tile;


        if (tilePosition.y - 1 < min.y)
        {
            front = Contents.Empty;
        }
        else
        {
            if (grid[tilePosition.x, tilePosition.y - 1].content == Contents.Tile)
            {
                front = Contents.Tile;
            }
            else
                front = Contents.Empty;
        }

        if (tilePosition.x + 1 >= max.x)
        {
            right = Contents.Empty;
        }
        else
        {
            if (grid[tilePosition.x + 1, tilePosition.y].content == Contents.Tile)
            {
                right = Contents.Tile;
            }
            else
                right = Contents.Empty;
        }

        if (tilePosition.y + 1 >= max.y)
        {
            back = Contents.Empty;
        }
        else
        {
            if (grid[tilePosition.x, tilePosition.y + 1].content == Contents.Tile)
            {
                back = Contents.Tile;
            }
            else
                back = Contents.Empty;
        }

        if (tilePosition.x - 1 < min.x)
        {
            left = Contents.Empty;
        }
        else
        {
            if (grid[tilePosition.x - 1, tilePosition.y].content == Contents.Tile)
            {
                left = Contents.Tile;
            }
            else
                left = Contents.Empty;
        }
    }
    private void FigureOutPieceGroup()
    {
        if (!isRound)
        {
            switch (type)
            {
                case PieceType.Empty:
                    int num = Random.Range(0, roomPrefabs[roomType].SquareEmptyPieceGroup.Count);
                    SquareEmptyPiece = roomPrefabs[roomType].SquareEmptyPieceGroup[num];
                    pieceToSpawn = SquareEmptyPiece;
                    prefabNum = num;
                    break;
                case PieceType.Corner:
                    int num2 = Random.Range(0, roomPrefabs[roomType].SquareCornerPieceGroup.Length);
                    SquareCornerPiece = roomPrefabs[roomType].SquareCornerPieceGroup[num2];
                    pieceToSpawn = SquareCornerPiece;
                    prefabNum = num2;
                    break;
                case PieceType.Edge:
                    int num3 = Random.Range(0, roomPrefabs[roomType].SquareEdgePieceGroup.Count);
                    SquareEdgePiece = roomPrefabs[roomType].SquareEdgePieceGroup[num3];
                    pieceToSpawn = SquareEdgePiece;
                    prefabNum = num3;
                    break;
                case PieceType.Hallway:
                    int num4 = Random.Range(0, roomPrefabs[roomType].SquareHallwayPieceGroup.Length);
                    SquareHallwayPiece = roomPrefabs[roomType].SquareHallwayPieceGroup[num4];
                    pieceToSpawn = SquareHallwayPiece;
                    prefabNum = num4;
                    break;
                case PieceType.Doorway:
                    int num5 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayPieceGroup.Length);
                    SquareDoorwayPiece = roomPrefabs[roomType].SquareDoorwayPieceGroup[num5];
                    pieceToSpawn = SquareDoorwayPiece;
                    prefabNum = num5;
                    break;
                case PieceType.DoorwayLeftCorner:
                    int num6 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayLeftCornerPieceGroup.Length);
                    SquareDoorwayLeftCornerPiece = roomPrefabs[roomType].SquareDoorwayLeftCornerPieceGroup[num6];
                    pieceToSpawn = SquareDoorwayLeftCornerPiece;
                    prefabNum = num6;
                    break;
                case PieceType.DoorwayRightCorner:
                    int num7 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayRightCornerPieceGroup.Length);
                    SquareDoorwayRightCornerPiece = roomPrefabs[roomType].SquareDoorwayRightCornerPieceGroup[num7];
                    pieceToSpawn = SquareDoorwayRightCornerPiece;
                    prefabNum = num7;
                    break;
                case PieceType.Deadend:
                    int num8 = Random.Range(0, roomPrefabs[roomType].SquareDeadendPieceGroup.Length);
                    SquareDeadendPiece = roomPrefabs[roomType].SquareDeadendPieceGroup[num8];
                    pieceToSpawn = SquareDeadendPiece;
                    prefabNum = num8;
                    break;
                case PieceType.Null:
                    pieceToSpawn = nullObj;
                    prefabNum = -1;
                    break;
            }
        }
    }

    private void FigureOutPiece()
    {
        if (!isRound)
        {
            switch (type)
            {
                case PieceType.Empty:
                    int num = Random.Range(0, SquareEmptyPieceGroup.Length);
                    SquareEmptyPiece = SquareEmptyPieceGroup[num];
                    pieceToSpawn = SquareEmptyPiece;
                    break;
                case PieceType.Corner:
                    int num2 = Random.Range(0, SquareCornerPieceGroup.Length);
                    SquareCornerPiece = SquareCornerPieceGroup[num2];
                    pieceToSpawn = SquareCornerPiece;
                    break;
                case PieceType.Edge:
                    int num3 = Random.Range(0, SquareEdgePieceGroup.Length);
                    SquareEdgePiece = SquareEdgePieceGroup[num3];
                    pieceToSpawn = SquareEdgePiece;
                    break;
                case PieceType.Hallway:
                    int num4 = Random.Range(0, SquareHallwayPieceGroup.Length);
                    SquareHallwayPiece = SquareHallwayPieceGroup[num4];
                    pieceToSpawn = SquareHallwayPiece;
                    break;
                case PieceType.Doorway:
                    int num5 = Random.Range(0, SquareDoorwayPieceGroup.Length);
                    SquareDoorwayPiece = SquareDoorwayPieceGroup[num5];
                    pieceToSpawn = SquareDoorwayPiece;
                    break;
                case PieceType.DoorwayLeftCorner:
                    int num6 = Random.Range(0, SquareDoorwayLeftCornerPieceGroup.Length);
                    SquareDoorwayLeftCornerPiece = SquareDoorwayLeftCornerPieceGroup[num6];
                    pieceToSpawn = SquareDoorwayLeftCornerPiece;
                    break;
                case PieceType.DoorwayRightCorner:
                    int num7 = Random.Range(0, SquareDoorwayRightCornerPieceGroup.Length);
                    SquareDoorwayRightCornerPiece = SquareDoorwayRightCornerPieceGroup[num7];
                    pieceToSpawn = SquareDoorwayRightCornerPiece;
                    break;
                case PieceType.Deadend:
                    int num8 = Random.Range(0, SquareDeadendPieceGroup.Length);
                    SquareDeadendPiece = SquareDeadendPieceGroup[num8];
                    pieceToSpawn = SquareDeadendPiece;
                    break;
                case PieceType.Null:
                    pieceToSpawn = nullObj;
                    break;
            }
        }
    }
    public void CheckAdjacentTiles(Cell center, Cell cellxp1, Cell cellxm1, Cell cellyp1, Cell cellym1)
    {
        print("Check adjacent tiles");
        bool pieceChecked = false;
        List<int> bannedNumbers = new List<int>();
        if (center.pieceType == PieceType.Empty)
        {
            List<GameObject> emptyPrefab = roomPrefabs[center.roomNumber].SquareEmptyPieceGroup;
            if(cellxp1 != null)
                if (cellxp1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellxp1.roomNumber);
            if(cellxm1 != null)
                if (cellxm1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellxm1.roomNumber);
            if(cellyp1 != null)
                if (cellyp1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellyp1.roomNumber);
            if(cellym1 != null)
                if (cellym1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellym1.roomNumber);
            int num = 0;
            while (!pieceChecked)
            {
                num = Random.Range(0, emptyPrefab.Count);
                if (bannedNumbers.Contains(num))
                    return;

                pieceChecked = true;
            }
            pieceToSpawn = emptyPrefab[num];
            Destroy(tileMesh);
            tileMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
            tileMesh.transform.parent = this.transform;
        }
        else if (center.pieceType == PieceType.Edge)
        {
            List<GameObject> edgePrefab = roomPrefabs[center.roomNumber].SquareEdgePieceGroup;
            if (cellxp1 != null)
                if (cellxp1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellxp1.roomNumber);
            if (cellxm1 != null)
                if (cellxm1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellxm1.roomNumber);
            if (cellyp1 != null)
                if (cellyp1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellyp1.roomNumber);
            if (cellym1 != null)
                if (cellym1.roomNumber == center.roomNumber)
                    bannedNumbers.Add(cellym1.roomNumber);
            int num = 0;
            while (!pieceChecked)
            {
                num = Random.Range(0, edgePrefab.Count);
                if (bannedNumbers.Contains(num))
                    return;

                pieceChecked = true;
            }
            pieceToSpawn = edgePrefab[num];
            Destroy(tileMesh);
            tileMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
            tileMesh.transform.parent = this.transform;
        }




    }
}
/*
                case PieceType.Empty:
                    //create temp list
                    List<GameObject> tempEmptyGroup = roomPrefabs[roomType].SquareEmptyPieceGroup;
int E_Xp1 = -1;
int E_Xm1 = -1;
int E_Yp1 = -1;
int E_Ym1 = -1;
bool E_RanNum = false;
int num = -1;
//check surrounding pieces if they are the same, remove from temp list if they are
if (grid[tilePosition.x + 1, tilePosition.y].pieceType == PieceType.Empty)
{
    E_Xp1 = (grid[tilePosition.x + 1, tilePosition.y].prefabNumber);
}
if (grid[tilePosition.x - 1, tilePosition.y].pieceType == PieceType.Empty)
{
    E_Xm1 = (grid[tilePosition.x - 1, tilePosition.y].prefabNumber);
}
if (grid[tilePosition.x, tilePosition.y + 1].pieceType == PieceType.Empty)
{
    E_Yp1 = (grid[tilePosition.x, tilePosition.y + 1].prefabNumber);
}
if (grid[tilePosition.x, tilePosition.y - 1].pieceType == PieceType.Empty)
{
    E_Ym1 = (grid[tilePosition.x, tilePosition.y - 1].prefabNumber);
}
// create random number with remaining list
while (E_RanNum == false)
{
    num = Random.Range(0, tempEmptyGroup.Count);
    if (num == E_Xp1)
        return;
    if (num == E_Xm1)
        return;
    if (num == E_Yp1)
        return;
    if (num == E_Ym1)
        return;
    E_RanNum = true;
}
SquareEmptyPiece = tempEmptyGroup[num];
pieceToSpawn = SquareEmptyPiece;
prefabNum = num;
break;
                case PieceType.Corner:
                    int num2 = Random.Range(0, roomPrefabs[roomType].SquareCornerPieceGroup.Length);
SquareCornerPiece = roomPrefabs[roomType].SquareCornerPieceGroup[num2];
pieceToSpawn = SquareCornerPiece;
prefabNum = num2;
break;
                case PieceType.Edge:
                    //create temp list
                    List<GameObject> tempEdgeGroup = roomPrefabs[roomType].SquareEdgePieceGroup;
int ED_Xp1 = -1;
int ED_Xm1 = -1;
int ED_Yp1 = -1;
int ED_Ym1 = -1;
bool ED_RanNum = false;
int num3 = -1;
//check surrounding pieces if they are the same, remove from temp list if they are
if (grid[tilePosition.x + 1, tilePosition.y].pieceType == PieceType.Edge)
{
    ED_Xp1 = (grid[tilePosition.x + 1, tilePosition.y].prefabNumber);
}
if (grid[tilePosition.x - 1, tilePosition.y].pieceType == PieceType.Edge)
{
    ED_Xm1 = (grid[tilePosition.x - 1, tilePosition.y].prefabNumber);
}
if (grid[tilePosition.x, tilePosition.y + 1].pieceType == PieceType.Edge)
{
    ED_Yp1 = (grid[tilePosition.x, tilePosition.y + 1].prefabNumber);
}
if (grid[tilePosition.x, tilePosition.y - 1].pieceType == PieceType.Edge)
{
    ED_Ym1 = (grid[tilePosition.x, tilePosition.y - 1].prefabNumber);
}
// create random number with remaining list
while (ED_RanNum == false)
{
    num = Random.Range(0, tempEdgeGroup.Count);
    if (num == ED_Xp1)
        return;
    if (num == ED_Xm1)
        return;
    if (num == ED_Yp1)
        return;
    if (num == ED_Ym1)
        return;
    E_RanNum = true;
}
SquareEdgePiece = tempEdgeGroup[num3];
pieceToSpawn = SquareEdgePiece;
prefabNum = num3;
break;
*/



//if (center.pieceType == PieceType.Empty)
//{
//    List<GameObject> emptyPrefab = roomPrefabs[center.roomNumber].SquareEmptyPieceGroup;
//    if (cellxp1.roomNumber == center.roomNumber)
//        emptyPrefab.RemoveAt(cellxp1.roomNumber);
//    if (cellxm1.roomNumber == center.roomNumber)
//        emptyPrefab.RemoveAt(cellxm1.roomNumber);
//    if (cellyp1.roomNumber == center.roomNumber)
//        emptyPrefab.RemoveAt(cellyp1.roomNumber);
//    if (cellym1.roomNumber == center.roomNumber)
//        emptyPrefab.RemoveAt(cellym1.roomNumber);
//    int num = Random.Range(0, emptyPrefab.Count);
//    pieceToSpawn = emptyPrefab[num];
//    Destroy(tileMesh);
//    tileMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
//    tileMesh.transform.parent = this.transform;

//}
//else if (center.pieceType == PieceType.Edge)
//{
//    List<GameObject> edgePrefab = roomPrefabs[center.roomNumber].SquareEdgePieceGroup;
//    if (cellxp1.roomNumber == center.roomNumber)
//        edgePrefab.RemoveAt(cellxp1.roomNumber);
//    if (cellxm1.roomNumber == center.roomNumber)
//        edgePrefab.RemoveAt(cellxm1.roomNumber);
//    if (cellyp1.roomNumber == center.roomNumber)
//        edgePrefab.RemoveAt(cellyp1.roomNumber);
//    if (cellym1.roomNumber == center.roomNumber)
//        edgePrefab.RemoveAt(cellym1.roomNumber);
//    int num = Random.Range(0, edgePrefab.Count);
//    pieceToSpawn = edgePrefab[num];
//    Destroy(tileMesh);
//    tileMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
//    tileMesh.transform.parent = this.transform;
//}