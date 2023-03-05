using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Tilemap mazeTileMap;
    public MazeTileSet mazeTileSet;
    public Maze maze;

    public Tile wall;
    public Tile path;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMaze(){
        mazeTileSet = new MazeTileSet();
        mazeTileSet.path = path;
        mazeTileSet.wall = wall;
        this.maze = new Maze(width: 30, height: 30);
        this.maze.renderTilemap(mazeTileMap, mazeTileSet);
    }
}
