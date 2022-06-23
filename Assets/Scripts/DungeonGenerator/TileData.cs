using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RoomPrefabs
{
    public GameObject[] SquareEmptyPieceGroup;
    public GameObject[] SquareEdgePieceGroup;
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

    public GameObject nullObj;

    private GameObject pieceToSpawn;
    public float rotation = 0;
    //public int tileX;
    //public int tileY;
    public Vector2Int tilePosition;
    private Vector2 minPosition;
    private Vector2 maxPosition;
    public bool isRound;
    public int roomType;

    public Contents current, front, left, right, back;
    public void Initialize(Cell cell, int roomNum)
    {
        int num = roomNum - 1;
        FigureOutRoomTheme(cell, num);
        FigureOutPieceGroup();
        SpawnMesh();
    }
    public void Initialize(Cell cell)
    {
        FigureOutPiece();
        SpawnMesh();
    }
    private void SpawnMesh()
    {

        GameObject newMesh = Instantiate(pieceToSpawn, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.Euler(0, rotation, 0)) as GameObject;
        newMesh.transform.parent = this.transform;
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
                    int num = Random.Range(0, roomPrefabs[roomType].SquareEmptyPieceGroup.Length);
                    SquareEmptyPiece = roomPrefabs[roomType].SquareEmptyPieceGroup[num];
                    pieceToSpawn = SquareEmptyPiece;
                    break;
                case PieceType.Corner:
                    int num2 = Random.Range(0, roomPrefabs[roomType].SquareCornerPieceGroup.Length);
                    SquareCornerPiece = roomPrefabs[roomType].SquareCornerPieceGroup[num2];
                    pieceToSpawn = SquareCornerPiece;
                    break;
                case PieceType.Edge:
                    int num3 = Random.Range(0, roomPrefabs[roomType].SquareEdgePieceGroup.Length);
                    SquareEdgePiece = roomPrefabs[roomType].SquareEdgePieceGroup[num3];
                    pieceToSpawn = SquareEdgePiece;
                    break;
                case PieceType.Hallway:
                    int num4 = Random.Range(0, roomPrefabs[roomType].SquareHallwayPieceGroup.Length);
                    SquareHallwayPiece = roomPrefabs[roomType].SquareHallwayPieceGroup[num4];
                    pieceToSpawn = SquareHallwayPiece;
                    break;
                case PieceType.Doorway:
                    int num5 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayPieceGroup.Length);
                    SquareDoorwayPiece = roomPrefabs[roomType].SquareDoorwayPieceGroup[num5];
                    pieceToSpawn = SquareDoorwayPiece;
                    break;
                case PieceType.DoorwayLeftCorner:
                    int num6 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayLeftCornerPieceGroup.Length);
                    SquareDoorwayLeftCornerPiece = roomPrefabs[roomType].SquareDoorwayLeftCornerPieceGroup[num6];
                    pieceToSpawn = SquareDoorwayLeftCornerPiece;
                    break;
                case PieceType.DoorwayRightCorner:
                    int num7 = Random.Range(0, roomPrefabs[roomType].SquareDoorwayRightCornerPieceGroup.Length);
                    SquareDoorwayRightCornerPiece = roomPrefabs[roomType].SquareDoorwayRightCornerPieceGroup[num7];
                    pieceToSpawn = SquareDoorwayRightCornerPiece;
                    break;
                case PieceType.Deadend:
                    int num8 = Random.Range(0, roomPrefabs[roomType].SquareDeadendPieceGroup.Length);
                    SquareDeadendPiece = roomPrefabs[roomType].SquareDeadendPieceGroup[num8];
                    pieceToSpawn = SquareDeadendPiece;
                    break;
                case PieceType.Null:
                    pieceToSpawn = nullObj;
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
}
