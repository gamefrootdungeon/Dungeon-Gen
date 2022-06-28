using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum infoTag
{
    LevelDisplay,
    NoteDisplay,
}

public class GridManager : MonoBehaviour
{
    private TileGenerator tileCalculator;
    public UIManager uiManager;

    public GameObject defaultCamera;
    public GameObject worldGrp;


    public string title;
    public string story;
    public bool flip;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;
    public Cell[,] grid;
    public Cell[] Grid1D;
    public List<Cell> ListOfTiles = new List<Cell>();

    public float offset;

    public GameObject tile;
    public GameObject emptySpace;


    [Header("PLAYER INFO")]
    public bool playerSpawned = false;
    public bool isFPSPlayer = true;
    private GameObject playerOBJ;
    public GameObject FPSPlayerOBJ;
    public GameObject TopDownPlayerOBJ;
    private GameObject player;
    private Vector3 playerStartCoord;
    private List<TileData> SpawnLocations = new List<TileData>();
    [SerializeField] private List<Cell> NumEdgePieces = new List<Cell>();

    public string JsonText = "";
    [SerializeField] private bool SpawnInNotes = false;

    public GameObject doorObj;
    public GameObject levelInfoObj;
    public GameObject displayNotesObj;
    public GameObject NFTChestObj;
    public GameObject PotAObj;
    public GameObject SkeletonOnGroundObj;

    [Header("DEBUG")]
    [SerializeField] private bool showGrid = false;
    [SerializeField] private bool showEmptySpace = false;
    private GameObject gridNumber;
    public List<int> VisualizernumberOfRooms = new List<int>();
    public HashSet<int> numberOfRooms = new HashSet<int>();

    public bool infoRoomBeenPlaced = false;

    //When add to the array group in the tile prefab remeber to change this number to match the size
    public int NumberOfDifferentRoomTypes;
    public Dictionary<int, int> roomType = new Dictionary<int, int>();

    private Vector2Int playerStartGrid;
    private int playerStatingRoomNumber;
    public void Start()
    {
        CheckWhatPlayerType();
    }

    #region Check whether FPSPlayer or TopDownPlayer
    private void CheckWhatPlayerType()
    {
        if (isFPSPlayer)
        {
            playerOBJ = FPSPlayerOBJ;
        }
        else
        {
            playerOBJ = TopDownPlayerOBJ;
        }
    }
    #endregion

    public void SetTextAsset(string asset)
    {
        JsonText = asset;
        //LoadJsonFile();
    }

    public void LoadJsonFile(string asset)
    {
        DestroyDungeon();
        JsonText = "";
        try
        {
            ConvertFromJsonToGridClass data = JsonUtility.FromJson<ConvertFromJsonToGridClass>(asset);
            JsonText = asset;
            title = data.title;
            story = data.story;

            Grid1D = new Cell[data.grid.Length];
            for (int i = 0; i < data.grid.Length; i++)
            {
                if (!numberOfRooms.Contains(data.grid[i].roomNumber))
                {
                    numberOfRooms.Add(data.grid[i].roomNumber);
                    VisualizernumberOfRooms.Add((int)data.grid[i].roomNumber);  
                }
                Cell newCell = new Cell(data.grid[i].position.x, data.grid[i].position.y);
                newCell.pieceType = data.grid[i].pieceType;
                newCell.content = data.grid[i].content;
                newCell.position = data.grid[i].position;
                newCell.rotation = data.grid[i].rotation;
                newCell.objectInTile = data.grid[i].tilePiece;

                newCell.roomNumber = data.grid[i].roomNumber;
                newCell.roomWidth = data.grid[i].roomWidth;
                newCell.roomLength = data.grid[i].roomLength;

                newCell.tile = data.grid[i].tile;
                newCell.door = data.grid[i].door;
                newCell.note = data.grid[i].note;
                newCell.column = data.grid[i].column;
                newCell.water = data.grid[i].water;

                Grid1D[i] = newCell;
            }
            FigureOutMinMax(Grid1D);
            grid = new Cell[(int)max.x, (int)max.y];
            ConvertTo2DArray(data);
            SetUpRoomTypes();
            Createtile();
            return;
        }
        catch
        {
            print("Not correct Json format!");
        }
    }



    public void SetUpRoomTypes()
    {
        HashSet<int> temp = new HashSet<int>();
        for (int i = 0; i < numberOfRooms.Count + 1; i++)
        {
            print("Count "+numberOfRooms.Count);
            int randomNum;
            randomNum = Random.Range(1, NumberOfDifferentRoomTypes +1);

            print("Room number " + (i));
            print("number of rooms " + randomNum);
            temp.Add(randomNum);
            roomType.Add(i, randomNum);
            
        }
        //print("index 0" + roomType[0]);
        //print("index 1" + roomType[1]);
        //print("index 2" + roomType[2]);
        //print("index 3" + roomType[3]);
        //print("index 4" + roomType[4]);
        //print("index 5" + roomType[5]);
    }
    private void ConvertTo2DArray(ConvertFromJsonToGridClass data)
    {
        foreach (ConvertFromJsonToGridClass.Grid newGrid in data.grid)
        {
            int tileX = (int)newGrid.position.x;
            int tileY = (int)newGrid.position.y;
            for (int x = 0; x < max.x; x++)
            {
                for (int y = 0; y < max.y; y++)
                {
                    if (tileX == x && tileY == y)
                    {
                        Cell newCell = new Cell(newGrid.position.x, newGrid.position.y);
                        newCell.pieceType = newGrid.pieceType;
                        newCell.content = newGrid.content;
                        newCell.position = newGrid.position;
                        newCell.rotation = newGrid.rotation;
                        newCell.objectInTile = newGrid.tilePiece;

                        newCell.roomNumber = newGrid.roomNumber;
                        newCell.roomWidth = newGrid.roomWidth;
                        newCell.roomLength = newGrid.roomLength;

                        newCell.tile = newGrid.tile;
                        newCell.door = newGrid.door;
                        newCell.note = newGrid.note;
                        newCell.column = newGrid.column;
                        newCell.water = newGrid.water;

                        grid[x, y] = newCell;
                    }
                }
            }
        }
    }

    private void FigureOutMinMax(Cell[] newGrid)
    {
        float lowX = 0;
        float highX = 0;
        float lowY = 0;
        float highY = 0;
        foreach (Cell grid in newGrid)
        {
            if (grid.position.x <= lowX)
                lowX = grid.position.x;
            else if (grid.position.x >= highX)
                highX = grid.position.x;
            if (grid.position.y <= lowY)
                lowY = grid.position.y;
            else if (grid.position.y >= highY)
                highY = grid.position.y;
        }
        min.x = lowX;
        max.x = highX + 1;
        min.y = lowY;
        max.y = highY + 1;
    }
    public void SpawnPlayer() 
    {
        if(playerStartCoord == Vector3.zero)
        {
            print("No Dungeon to Spawn Player");
        }
        else
        {
            if (playerSpawned == false)
            {
                defaultCamera.SetActive(false);
                playerSpawned = true;
                player = Instantiate(playerOBJ, playerStartCoord, Quaternion.identity);
                //player.GetComponentInChildren<FPSController>().CheckIfWallInFront();
                //uiManager.StartGame();
            }
        }

    }

    private void DestroyPlayer()
    {
        playerSpawned = false;
        defaultCamera.SetActive(true);
        Destroy(player.gameObject);
    }

    public void BackToMenu()
    {
        if (playerSpawned)
        {
            DestroyPlayer();
        }
    }
    public void DestroyDungeon()
    {
        GameObject World;
        if (World = GameObject.FindGameObjectWithTag("World_Grp"))
        {
            roomType.Clear();
            World = GameObject.FindGameObjectWithTag("World_Grp");
            tileCalculator = null;
            //loader = null;
            this.min = Vector2.zero;
            this.max = Vector2.zero;
            playerStartCoord = Vector3.zero;
            grid = null;
            Destroy(World.gameObject);
        }
    }
    public void Createtile()
    {
        print("Create tile call");
        worldGrp = new GameObject();
        worldGrp.tag = "World_Grp";
        worldGrp.name = "World_Grp";
        float yOffset = 0;
        int flipDirection = 1;
        if (flip)
        {
            yOffset = max.y;
            flipDirection = -1;
        }
        //CheckGrid();
        tileCalculator = new TileGenerator(grid, min, max);

        #region Debugging
        //Debug to show grid value
        if (showGrid)
        {
            DisplayGridValues(yOffset, flipDirection);
        }
        if (showEmptySpace)
        {
            DisplayEmptyGrid(yOffset, flipDirection);
        }
        #endregion

        //Creating the tiles in the grid
        foreach (Cell cell in grid)
        {
            if (cell != null)
            {
                if (cell.content == Contents.Tile)
                {
                    if (cell.tile.isRound)
                    {
                        InstantiateTilePiece(cell, yOffset, flipDirection, true);
                    }
                    else
                    {
                        InstantiateTilePiece(cell, yOffset, flipDirection, false);
                    }
                }
                //Spawn in notes from JSON file
                if (SpawnInNotes)
                {
                    //if the string on the cell has something on it then instantiate a displaynote object
                    if (!cell.note.text.Equals(""))
                    {
                        //print("Note Text " + cell.note.text);
                        GameObject levelInfo = Instantiate(displayNotesObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                        levelInfo.GetComponent<InfoDisplayTrigger>().SetUpInfo(cell.note.text, infoTag.LevelDisplay);
                        levelInfo.transform.SetParent(worldGrp.transform);
                        cell.AddTileData(levelInfo);
                    }
                }
                //Instantiate a door into the grid
                if (cell.door.type > 0)
                {
                    GameObject newDoor = Instantiate(doorObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                    if (cell.door.dir.dir == new Vector2(1, 0))
                    {
                        if(cell.pieceType == PieceType.Deadend)
                        {
                            newDoor.transform.position = new Vector3(newDoor.transform.position.x - 2.5f, newDoor.transform.position.y, newDoor.transform.position.z);
                            newDoor.GetComponent<SetDoorActive>().SetTriggersoff();
                        }
                        newDoor.transform.Rotate(transform.up * 90);
                    }
                    else if (cell.door.dir.dir == new Vector2(0, 1))
                    {
                        if (cell.pieceType == PieceType.Deadend)
                        {
                            newDoor.transform.position = new Vector3(newDoor.transform.position.x, newDoor.transform.position.y, newDoor.transform.position.z - 2.5f);
                            newDoor.GetComponent<SetDoorActive>().SetTriggersoff();
                        }
                        newDoor.transform.Rotate(transform.up * 0);
                    }
                    newDoor.transform.SetParent(worldGrp.transform);
                }
            }
        }
        print("Tiles spawned ");
        SetPlayerSpawn(yOffset, flipDirection);
        //SpawnInLevelDataObject(yOffset, flipDirection, levelInfoObj);
        //SpawnInObject(yOffset, flipDirection, NFTChestObj);
        FigureOutPlayerStartingRoomNumber();
        SpawnNFTChest(yOffset, flipDirection);
        SpawnInfoDesk(yOffset, flipDirection);
        //SpawnRandomObjects(yOffset, flipDirection);

    }

    //Currently disabled, spawn given objects randomly around the level
    private void SpawnRandomObjects(float yOffset, float flipDirection)
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnInObject(yOffset, flipDirection, PotAObj);
            SpawnInObject(yOffset, flipDirection, SkeletonOnGroundObj);
        }
    }
    private void SpawnInLevelDataObject(float yOffset, float flipDirection, GameObject objectToSpawn)
    {
        int n = Random.Range(0, ListOfTiles.Count);
        Cell cell = ListOfTiles[n];
        if (cell.objectInTile != null)
        {
            SpawnInLevelDataObject(yOffset, flipDirection, objectToSpawn);
        }
        else
        {
            //Checking to see if given cell position in on a doorway piece, recalls the method and creates another random number if is on one
            if (cell.pieceType == PieceType.Doorway || cell.pieceType == PieceType.DoorwayLeftCorner || cell.pieceType == PieceType.DoorwayRightCorner)
            {
                SpawnInLevelDataObject(yOffset, flipDirection, objectToSpawn);
            }
            else
            {
                GameObject NewSpawnedobject = Instantiate(objectToSpawn, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                NewSpawnedobject.GetComponent<InfoDisplayTrigger>().SetUpInfo(title, story, infoTag.LevelDisplay);
                NewSpawnedobject.transform.SetParent(worldGrp.transform);
                cell.AddTileData(NewSpawnedobject);
            }
        }
    }
    private void SpawnInfoDesk(float yOffset, float flipDirection)
    {
        int num = Random.Range(0, NumEdgePieces.Count);
        Cell cell = NumEdgePieces[num];
        playerStartCoord = new Vector3(NumEdgePieces[num].position.x * offset, 0.5f, ((NumEdgePieces[num].position.y * flipDirection) + yOffset) * offset);
        if (cell.objectInTile != null)
        {
            SpawnNFTChest(yOffset, flipDirection);
        }
        else
        {
            GameObject NewSpawnedobject = Instantiate(levelInfoObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
            NewSpawnedobject.transform.SetParent(worldGrp.transform);
            cell.AddTileData(NewSpawnedobject);
            grid[(int)NumEdgePieces[num].position.x, (int)NumEdgePieces[num].position.y].AddTileData(NewSpawnedobject);
            NumEdgePieces.Remove(cell);
        }
    }
    private void SpawnNFTChest(float yOffset, float flipDirection)
    {
        int num = Random.Range(0, NumEdgePieces.Count);
        Cell cell = NumEdgePieces[num];
        playerStartCoord = new Vector3(NumEdgePieces[num].position.x * offset, 0.5f, ((NumEdgePieces[num].position.y * flipDirection) + yOffset) * offset);
        if (cell.objectInTile != null)
        {
            SpawnNFTChest(yOffset, flipDirection);
        }
        else
        {
            GameObject NewSpawnedobject = Instantiate(NFTChestObj, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
            NewSpawnedobject.transform.SetParent(worldGrp.transform);
            cell.AddTileData(NewSpawnedobject);
            grid[(int)NumEdgePieces[num].position.x, (int)NumEdgePieces[num].position.y].AddTileData(NewSpawnedobject);
            NumEdgePieces.Remove(cell);
        }
    }
    private void SpawnInObject(float yOffset, float flipDirection, GameObject objectToSpawn)
    {
        int n = Random.Range(0, ListOfTiles.Count);
        Cell cell = ListOfTiles[n];
        if(cell.objectInTile != null)
        {
            SpawnInObject(yOffset, flipDirection, objectToSpawn);
        }
        else
        {
            if (cell.pieceType == PieceType.Doorway ||
                cell.pieceType == PieceType.DoorwayLeftCorner ||
                cell.pieceType == PieceType.DoorwayRightCorner ||
                cell.pieceType == PieceType.Hallway)
            {
                SpawnInObject(yOffset, flipDirection, objectToSpawn);
            }
            else
            {
                GameObject NewSpawnedobject = Instantiate(objectToSpawn, new Vector3(cell.position.x * offset, 1, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                NewSpawnedobject.transform.SetParent(worldGrp.transform);
                cell.AddTileData(NewSpawnedobject);
            }
        }
    }

    private void FigureOutPlayerStartingRoomNumber()
    {
        print("room numbers around player");
        if (grid[playerStartGrid.x + 1, playerStartGrid.y].content == Contents.Tile)
        {
            print(grid[playerStartGrid.x + 1, playerStartGrid.y].roomNumber);
            playerStatingRoomNumber = grid[playerStartGrid.x + 1, playerStartGrid.y].roomNumber;
        }
        if (grid[playerStartGrid.x - 1, playerStartGrid.y].content == Contents.Tile)
        {
            print(grid[playerStartGrid.x - 1, playerStartGrid.y].roomNumber);
            playerStatingRoomNumber = grid[playerStartGrid.x - 1, playerStartGrid.y].roomNumber;
        }
        if (grid[playerStartGrid.x, playerStartGrid.y + 1].content == Contents.Tile)
        {
            print(grid[playerStartGrid.x, playerStartGrid.y + 1].roomNumber);
            playerStatingRoomNumber = grid[playerStartGrid.x, playerStartGrid.y + 1].roomNumber;
        }
        if (grid[playerStartGrid.x, playerStartGrid.y - 1].content == Contents.Tile)
        {
            print(grid[playerStartGrid.x, playerStartGrid.y - 1].roomNumber);
            playerStatingRoomNumber = grid[playerStartGrid.x, playerStartGrid.y - 1].roomNumber;
        }
    }

    private void SetPlayerSpawn(float yOffset, float flipDirection)
    {
        int num = Random.Range(0, SpawnLocations.Count);
        playerStartGrid = new Vector2Int((int)SpawnLocations[num].tilePosition.x, (int)SpawnLocations[num].tilePosition.y);
        playerStartCoord = new Vector3(SpawnLocations[num].tilePosition.x * offset, 0.5f, ((SpawnLocations[num].tilePosition.y * flipDirection) + yOffset) * offset);

    }
    void InstantiateTilePiece(Cell cell, float yOffset, float flipDirection, bool isRound)
    {
        float posX = cell.position.x;
        float posY = (cell.position.y * flipDirection) + yOffset;
        GameObject newtile = Instantiate(tile, new Vector3(posX * offset, 2, posY * offset), Quaternion.identity);

        newtile.transform.SetParent(worldGrp.transform);

        TileData tileData = newtile.GetComponent<TileData>();

        tileData.tilePosition.x = (int)cell.position.x;
        tileData.tilePosition.y = (int)cell.position.y;

        tileData.type = tileCalculator.CheckPiece(tileData.tilePosition);
        if (!flip)
        {
            float flipValue = tileData.rotation += 90;
            newtile.transform.rotation = Quaternion.Euler(newtile.transform.rotation.x, newtile.transform.localRotation.y + 180, newtile.transform.rotation.z);
        }
        //Checking if the current tile is a deadend piece, if it is add to a list that will be used for potential player spawn locations
        if(tileData.type == PieceType.Deadend)
        {
            SpawnLocations.Add(tileData);
        }
        if(tileData.type == PieceType.Edge)
        {
            NumEdgePieces.Add(cell);
        }
        tileData.rotation = tileCalculator.Rotation;
        tileData.isRound = isRound;
        tileData.CheckSurrounding(grid, min, max);

        //print(roomType[cell.roomNumber]);
        tileData.Initialize(cell, (roomType[cell.roomNumber]));
        //tileData.Initialize(cell);
        cell.pieceType = tileData.type;

        ListOfTiles.Add(cell);
    }
    #region Debugging
    //Used for debugging and having something in scene that displays the grid number
    public void CheckGrid()
    {
        for (int x = 0; x < max.x; x ++)
        {
            for (int y = 0; y < max.y; y++)
            {
                GameObject number = Instantiate(gridNumber, new Vector3(grid[x, y].position.x * offset, 10, grid[x, y].position.y * offset), Quaternion.identity);
                TextMeshProUGUI text = number.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "x: " + x + " y: " + y + "   " + grid[x,y].content;
                print("grid position x " + (grid[x, y].position.x) + " x " + x);
                print("grid position y " + (grid[x, y].position.y) + " y " + y);
            }
        }
    }


    private void DisplayEmptyGrid(float yOffset, float flipDirection) 
    {
        foreach (Cell cell in grid)
        {
            if (cell != null)
            {
                if (cell.content == Contents.Empty)
                {
                    Instantiate(emptySpace, new Vector3(cell.position.x * offset, 5.1f, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                }
            }
        }
    }
    private void DisplayGridValues(float yOffset, float flipDirection)
    {
        foreach (Cell cell in grid)
        {
            if (cell != null)
            {
                GameObject number = Instantiate(gridNumber, new Vector3(cell.position.x * offset, 5.1f, ((cell.position.y * flipDirection) + yOffset) * offset), Quaternion.identity);
                TextMeshProUGUI text = number.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "x: " + cell.position.x + " y: " + cell.position.y + "   " + cell.content;
            }
        }
    }

    #endregion
}

