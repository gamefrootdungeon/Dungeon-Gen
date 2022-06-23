using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;


public class JsonConvertManager : MonoBehaviour
{

    public JsonLoader loader;
    public ConvertFromGridToJsonClass cellJsonCon;

    private string GridJsonText;
    private TextAsset GridDataJSON;

    public bool success = true;
    private TextAsset[] JSONFiles;

    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    public Cell[,] grid;
    public Cell[] Grid1D;

    public GameObject tileobj;
    public GameObject emptyobj;

    private string title = "";
    private string story = "";
    string newJson = "";


    public List<CoordData> Visualcoordinates = new List<CoordData>();
    public string testJson = "";
    private void Start()
    {
        
    }
    public string ManualConversion(string json)
    {
        cellJsonCon = new ConvertFromGridToJsonClass();
        Visualcoordinates.Clear();
        loader.clearAll();
        GridJsonText = "";
        testJson = "";
        title = "";
        story = "";
        testJson = json;
        loader.Intialize(json);
        title = loader.GetTitle;
        story = loader.GetStroy;
        this.min = loader.GetMin;
        this.max = loader.GetMax;

        grid = new Cell[(int)(max.x), (int)(max.y)];

        return SetUpGrid(loader.GetCoordinates);
        //return newJson;
    }
    private string SetUpGrid(List<CoordData> coordinates)
    {
        Visualcoordinates = coordinates;
        foreach (CoordData data in coordinates)
        {
            for (int x = 0; x < (max.x); x++)
            {
                for (int y = 0; y < (max.y); y++)
                {
                    Vector2 position = new Vector2(x, y);
                    Cell newCell = new Cell(x, y);

                    newCell.roomNumber = data.roomNumber;
                    newCell.roomWidth = data.roomWidth;
                    newCell.roomLength = data.roomLength;
                    if (data.gridPosition.x == x && data.gridPosition.y == y)
                    {
                        newCell.SetContents(Contents.Tile);

                        if (data.tile.isRound)
                        {
                            newCell.newTiles(position, true);
                        }
                        else
                        {
                            newCell.newTiles(position, false);

                            if (data.door != null)
                                if ((int)data.door.position.x == x && (int)data.door.position.y == y)
                                {
                                    if(data.door.type > 0)
                                    {
                                        newCell.newDoor(data.door.position, data.door.type, data.door.dir.dir);
                                    }
                                    else
                                    {
                                        newCell.newDoor(-Vector2.zero, -1, -Vector2.zero);
                                    }
                                }
                                
                            if (data.note != null)
                                if (data.note.pos.position.x == x && data.note.pos.position.y == y)
                                {
                                    if (data.note.exist == true)
                                    {
                                        newCell.NewNotes(data.note.text, data.note.reference, data.note.pos.position, data.note.pos.rawPosition, data.note.exist);
                                    }
                                    else
                                    {
                                        newCell.note = null;
                                    }
                                }

                            if (data.column != null)
                                if (data.column.position.x == x && data.column.position.y == y)
                                {
                                    newCell.NewColumn(data.column.position, data.column.exist);
                                }

                            if (data.water != null)
                                if (data.water.position.x == x && data.water.position.y == y)
                                {
                                    newCell.NewWater(data.water.position, data.water.exist);
                                }

                        }
                        grid[x, y] = newCell;
                    }
                    else
                    {
                        if (grid[x, y] != null)
                        {
                            if (grid[x, y].content == Contents.Tile)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            newCell.SetContents(Contents.Empty);
                            if (grid[x, y] == null)
                            {
                                newCell.newTiles(position, false);
                            }
                            grid[x, y] = newCell;
                        }
                    }
                }
            }
        }
        return ConvertGridTo1DArray();
    }

    private string ConvertGridTo1DArray()
    {
        int maxX = (int)max.x;
        int maxY = (int)max.y;
        Grid1D = new Cell[maxX * maxY];
        int j = 0;
        for (int x = 0; x < max.x; x++)
            for (int y = 0; y < max.y; y++)
            {
                Grid1D[j++] = grid[x, y];
            }
        cellJsonCon.grid = Grid1D;
        return ConvertJsonToText();
    }

    private string ConvertJsonToText()
    {
        cellJsonCon.title = title;
        cellJsonCon.story = story;
        GridJsonText = ConvertGridToJson();
        //newJson = GridJsonText;
        return GridJsonText;
    }
    private string ConvertGridToJson()
    {
        return JsonUtility.ToJson(cellJsonCon);
    }
}
