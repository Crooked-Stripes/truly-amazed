using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public Tilemap mazeTilemap;
    public Tile wallTile;
    public Tile floorTile;
    public Tile doorTile;
    public Tile keyTile;
    public GameObject player;
    public GameObject keyPrefab;
    public GameObject doorPrefab;

    private int mazeWidth = 40;
    private int mazeHeight = 25;
    private bool[,] maze;
    private bool[,] algVisited;
    private bool[,] playerVisited;
    private Vector2Int startCell;
    private Vector2Int endCell;
    private Vector2Int keyCell;
    private void Start()
    {
        GenerateMaze();
        AddWallsToMaze();
        PlaceKey();
        PlaceDoor();
        PlacePlayer();
        MazeGenerated();
    }

    //private void GenerateMaze()
    //{

    //    algVisited = new bool[mazeWidth, mazeHeight];
    //    startCell = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));
    //    DFS(startCell);
    //    endCell = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));
    //}

    private void GenerateMaze()
    {
        // Create the maze array
        maze = new bool[mazeWidth, mazeHeight];
        algVisited = new bool[mazeWidth, mazeHeight];
        playerVisited = new bool[mazeWidth, mazeHeight];
        startCell = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));
        endCell = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));

        // Initialize the maze array to contain all walls
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = true;
            }
        }

        // Choose a random starting cell
        Vector2Int currentCell = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));

        // Mark the starting cell as visited
        algVisited[currentCell.x, currentCell.y] = true;

        // Create a stack to keep track of the path through the maze
        Stack<Vector2Int> cellStack = new Stack<Vector2Int>();
        cellStack.Push(currentCell);

        // Continue while there are still unvisited cells
        while (!AllCellsVisited(algVisited))
        {
            // Get the current cell
            currentCell = cellStack.Peek();

            // Get the neighbors of the current cell that have not been visited
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(currentCell);

            // If there are any unvisited neighbors
            if (unvisitedNeighbors.Count > 0)
            {
                // Choose a random unvisited neighbor
                Vector2Int chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];

                // Remove the wall between the current cell and the chosen neighbor
                RemoveWall(currentCell, chosenNeighbor);

                // Mark the chosen neighbor as visited and push it onto the stack
                algVisited[chosenNeighbor.x, chosenNeighbor.y] = true;
                cellStack.Push(chosenNeighbor);
            }
            else
            {
                // If there are no unvisited neighbors, pop the current cell off the stack
                cellStack.Pop();
            }
        }

        // Add the walls to the tilemap
        AddWallsToMaze();
    }

    private bool AllCellsVisited(bool[,] visitedCells)
    {
        for (int x = 0; x < visitedCells.GetLength(0); x++)
        {
            for (int y = 0; y < visitedCells.GetLength(1); y++)
            {
                if (!visitedCells[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }


    private void DFS(Vector2Int currentCell)
    {
        algVisited[currentCell.x, currentCell.y] = true;
        List<Vector2Int> neighbors = GetUnvisitedNeighbors(currentCell);
        while (neighbors.Count > 0)
        {
            int randomIndex = Random.Range(0, neighbors.Count);
            Vector2Int nextCell = neighbors[randomIndex];
            neighbors.RemoveAt(randomIndex);
            if (!algVisited[nextCell.x, nextCell.y])
            {
                RemoveWall(currentCell, nextCell);
                DFS(nextCell);
            }
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (cell.x > 0 && !algVisited[cell.x - 1, cell.y]) neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
        if (cell.x < mazeWidth - 1 && !algVisited[cell.x + 1, cell.y]) neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
        if (cell.y > 0 && !algVisited[cell.x, cell.y - 1]) neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
        if (cell.y < mazeHeight - 1 && !algVisited[cell.x, cell.y + 1]) neighbors.Add(new Vector2Int(cell.x, cell.y + 1));
        return neighbors;
    }

    private void RemoveWall(Vector2Int currentCell, Vector2Int nextCell)
    {
        Vector2Int delta = nextCell - currentCell;
        if (delta.x == -1) // next cell is left of current cell
        {
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2, currentCell.y * 2 + 1, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 - 1, currentCell.y * 2 + 1, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 - 1, currentCell.y * 2, 0), floorTile);
        }
        else if (delta.x == 1) // next cell is right of current cell
        {
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 1, currentCell.y * 2 + 1, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 2, currentCell.y * 2 + 1, 0), floorTile);


            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 2, currentCell.y * 2, 0), floorTile);
        }
        else if (delta.y == -1) // next cell is below current cell
        {
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 1, currentCell.y * 2, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 1, currentCell.y * 2 - 1, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2, currentCell.y * 2 - 1, 0), floorTile);
        }
        else if (delta.y == 1) // next cell is above current cell
        {
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 1, currentCell.y * 2 + 2, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2 + 1, currentCell.y * 2 + 1, 0), floorTile);
            mazeTilemap.SetTile(new Vector3Int(currentCell.x * 2, currentCell.y * 2 + 1, 0), floorTile);
        }
    }

    private void AddWallsToMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (maze[x, y])
                {
                    Vector3Int tilePosition = new Vector3Int(x * 2 + 1, y * 2 + 1, 0);
                    mazeTilemap.SetTile(tilePosition, wallTile);
                }
            }
        }
    }



    private Vector2Int GetRandomUnvisitedCell()
    {
        List<Vector2Int> unvisitedCells = new List<Vector2Int>();
        
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (!playerVisited[x, y])
                {
                    unvisitedCells.Add(new Vector2Int(x, y));
                }
            }
        }
        
        if (unvisitedCells.Count == 0)
        {
            return Vector2Int.zero;
        }
        
        return unvisitedCells[Random.Range(0, unvisitedCells.Count)];
    }

    public void MazeGenerated()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.ResizeCamera();
        }
    }


    private void PlaceKey()
    {
        keyCell = GetRandomUnvisitedCell();
        Vector3 position = new Vector3(keyCell.x * 2 + 1, keyCell.y * 2 + 1, 1);
        Instantiate(keyPrefab, position, Quaternion.identity);
        mazeTilemap.SetTile(new Vector3Int(keyCell.x * 2 + 1, keyCell.y * 2 + 1, 0), keyTile);
    }

    private void PlaceDoor()
    {
        Vector3 position = new Vector3(endCell.x * 2 + 1, endCell.y * 2 + 1, 1);
        Instantiate(doorPrefab, position, Quaternion.identity);
        mazeTilemap.SetTile(new Vector3Int(endCell.x * 2 + 1, endCell.y * 2 + 1, 0), doorTile);
    }

    private void PlacePlayer()
    {
        player.transform.position = new Vector3(startCell.x * 2 + 1, startCell.y * 2 + 1, 1);
    }

    public void UnlockDoor()
    {
        mazeTilemap.SetTile(new Vector3Int(endCell.x * 2 + 1, endCell.y * 2 + 1, 0), floorTile);
    }

    public void RemoveKey()
    {
        mazeTilemap.SetTile(new Vector3Int(keyCell.x * 2 + 1, keyCell.y * 2 + 1, 0), floorTile);
    }
}