using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public class Cell
    {
        public enum TerrainType { normal, blocked }

        public Vector2Int pos;
        public float elevation;
        public TerrainType terrainType;
        public Selectable selectable;

        public Cell(Vector2Int pos, float elevation)
        {
            this.pos = pos;
            this.elevation = elevation;
            this.terrainType = TerrainType.normal;
            this.selectable = null;
        }
    }

    public int gridX, gridZ;

    public int cellDims;

    public Cell[,] grid;

    public GameObject gridPlane;

    public Camera cam;

    public Selector selector;

    private Vector3 lastMPos;
    private Vector3 mPos;

    private List<Vector3> gizmoPositions = new List<Vector3>();

    void Start()
    {
        cam = FindObjectOfType<Camera>();

        selector = gameObject.GetComponent<Selector>();

        gridPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gridPlane.transform.localScale = new Vector3(gridX * cellDims / 10, 1f, gridZ * cellDims / 10);
        gridPlane.transform.position = new Vector3(gridX * cellDims * cellDims / 10, 0f, gridZ * cellDims * cellDims / 10);

        grid = new Cell[gridX, gridZ];
        for(int i = 0; i < gridX; i++)
        {
            for(int j = 0; j < gridZ; j++)
            {
                grid[i, j] = new Cell(new Vector2Int(i * cellDims, j * cellDims), 0f);
                //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //obj.transform.position = new Vector3(grid[i, j].pos.x + cellDims / 2, grid[i, j].elevation, grid[i, j].pos.y + cellDims / 2);
            }
        }
        UpdateGrid();
        lastMPos = cam.ScreenPointToRay(Input.mousePosition).origin;
    }

    void Update()
    {
        mPos = cam.ScreenPointToRay(Input.mousePosition).origin;
        
        if (lastMPos != mPos)
        {
            lastMPos = mPos;
            Vector3 v = new Vector3(mPos.x, cam.transform.position.y, mPos.z);
            Cell cell = FindCell(v, cam.ScreenPointToRay(Input.mousePosition).direction, 100);
            if (cell != null)
            {
                selector.MoveSelector(new Vector3(cell.pos.x + cellDims / 2, cell.elevation + 1.5f, cell.pos.y + cellDims / 2));
            }
            else
            {
                //Debug.Log("No cell found");
            }
        }
    }
    
    public Cell FindCell(Vector3 pos, Vector3 dir, int maxIterations)
    {
        gizmoPositions.Clear();
        Ray ray = new Ray(pos, dir);
        Cell cell = FindNearestCell(new Vector2(pos.x, pos.z));
        if(ray.origin.y < cell.elevation)
        {
            return cell;
        }

        for(int i = 0; i < maxIterations; i++)
        {
            ray.origin += dir * cellDims / 3;

            gizmoPositions.Add(ray.origin);
            
            cell = FindNearestCell(new Vector2(ray.origin.x, ray.origin.z));
            if(ray.origin.y < cell.elevation)
            {
                return cell;
            }
        }

        return null;
    }

    public Cell FindNearestCell(Vector2 pos)
    {
        Cell currCell = grid[0, 0];
        Cell returnCell = grid[0, 0];

        foreach (Cell c in grid)
        {
            Vector2 currCellAdjPos = new Vector2(currCell.pos.x + cellDims / 2, currCell.pos.y + cellDims / 2);
            Vector2 returnCellAdjPos = new Vector2(returnCell.pos.x + cellDims / 2, returnCell.pos.y + cellDims / 2);
            if (Vector2.Distance(pos, returnCellAdjPos) > Vector2.Distance(pos, currCellAdjPos))
            {
                returnCell = currCell;
            }
            currCell = c;
        }
        return returnCell;
    }

    public Cell GetCellAt(Vector2Int pos)
    {
        return grid[pos.x, pos.y];
    }

    public void UpdateGrid()
    {
        for(int i = 0; i < gridX; i++)
        {
            for(int j = 0; j < gridZ; j++)
            {
                grid[i, j].selectable = null;
            }
        }

        foreach(Selectable s in selector.selectables)
        {
            FindNearestCell(new Vector2(s.transform.position.x, s.transform.position.z)).selectable = s;

        }
    }

    private void OnDrawGizmos()
    {
        foreach(Vector3 v in gizmoPositions)
        {
            //Gizmos.DrawSphere(v, 0.1f);
        }


    }
}
