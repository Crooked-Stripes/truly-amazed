using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Tilemap mazeTileMap;
    public MazeTileSet mazeTileSet;

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
        Maze maze = new Maze(width: 30, height: 30);
        maze.renderTilemap(mazeTileMap, mazeTileSet);
    }
}
