using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{

    public int width;
    public int height;
    public bool[,] grid;
    public Vector2Int start;

    public Maze(int width, int height) {
        this.width = width;
        this.height = height;
        this.grid = new bool[width, height];
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                this.grid[i, j] = false;
            }
        }

        int dfs_width = (width - 1) / 2;
        int dfs_height = (height - 1) / 2;

        System.Random rand = new System.Random();
        int start_x, start_y;
        if (rand.Next(2) == 0){
            start_x = 0;
            start_y = rand.Next(dfs_height);
        } else {
            start_x = rand.Next(dfs_width);
            start_y = 0;
        }

        this.start = new Vector2Int(start_x*2 + 1, start_y*2 + 1);

        //   Console.WriteLine("hello");
        //   Console.WriteLine("The start = {0}", this.start);
        DFS dfs = new DFS(dfs_width, dfs_height);
        List<Cell> cells = dfs.run(start_x, start_y);

        // TODO: Translate cells to grid
    }


    
   private class Cell {
        int x,y;
        bool left, right, up, down;

        // The previously visited neighbouring cell
        Cell prev;

        public Cell(int x, int y, Cell prev = null) {
            this.x = x;
            this.y = y;

            this.left = true;
            this.right = true;
            this.up = true;
            this.down = true;

            this.prev = prev;
        }

        public bool hasWall(Vector2Int direction) {
            if(direction == Vector2Int.up){
                return this.up;
            }else if(direction == Vector2Int.down){
                return this.down;
            }else if(direction == Vector2Int.right){
                return this.right;
            }else if(direction == Vector2Int.left){
                return this.left;
            }

            return false;
        }

        public void setWall(Vector2Int direction, bool wallValue) {
            if(direction == Vector2Int.up){
                this.up = wallValue;
            }else if(direction == Vector2Int.down){
                this.down = wallValue;
            }else if(direction == Vector2Int.right){
                this.right = wallValue;
            }else if(direction == Vector2Int.left){
                this.left = wallValue;
            }
        }
   }

   

    private class DFS {
        int width;
        int height;
        bool[,] visited;

        // Initialize direction vectors
        static Vector2Int[] directions = {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

        public DFS(int width, int height){
            this.width = width;
            this.height = height;
            this.visited = new bool[width, height];
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    this.visited[i, j] = false;
                }
            }
        }

        public List<Cell> run(int start_x, int start_y){
            List<Cell> cells = new List<Cell>();

            if(!isValid(start_x, start_y)){
                // Return an empty list of cells if start is invalid
                return cells;
            }

            System.Random rnd = new System.Random();

            // Initialize a stack of pairs and
            // push the starting cell into it
            Stack st = new Stack();
            st.Push(new Cell(start_x, start_y));
        
            // Iterate until the
            // stack is not empty
            while (st.Count > 0)
            {
                
                // Pop the top pair
                Cell current = (Cell)st.Pop();
        
                // Check if the current popped
                // cell is a valid cell or not
                if (!isValid(current.x, current.y))
                    continue;
        
                // Mark the current
                // cell as visited
                this.visited[current.x, current.y] = true;

                if(current.prev != null){
                    Vector2Int prevDirection = new Vector2Int(current.prev.x - current.x, current.prev.y - current.y);
                    current.setWall(prevDirection, false);
                }
        
                // Add cell to list
                cells.Add(current);

                int[] randIndexes = System.Linq.Enumerable.Range(0, 4).OrderBy(c => rnd.Next()).ToArray();
                // Push all the adjacent cells
                for(int i = 0; i < 4; i++)
                {
                    int adjx = current.x + directions[randIndexes[i]].x;
                    int adjy = current.y + directions[randIndexes[i]].y;
                    st.Push(new Cell(adjx, adjy, current));
                }
            }
        }

        public bool isValid(int x, int y)
        {
            
            // If coord is out of bounds
            if (x < 0 || y < 0 ||
                x >= this.width || y >= this.height)
                return false;
        
            // If the cell is already visited
            if (this.visited[x,y])
                return false;
        
            // Otherwise, it can be visited
            return true;
        }
    }

}
