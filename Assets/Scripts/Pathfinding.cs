using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static Pathfinding Instance { get; private set; }

    //Cost for moving in a certian direction
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> closedList;
    private List<PathNode> openList;
    public Pathfinding(int width,int height)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 5f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    //Finds the path from 2 points and thier x and y values 

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }
    public List<PathNode> FindPath(int startX,int startY,int endX,int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);


        
        closedList = new List<PathNode> { startNode };
        openList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFcost();
                pathNode.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFcost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighboursList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFcost();
                }
            }
        }
        return null;
    }

    private List<PathNode> GetNeighboursList(PathNode pathNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (pathNode.x-1 >= 0)
        {
            //Left Node
            neighbourList.Add(GetNode(pathNode.x - 1, pathNode.y - 1));
            //Left Down Node
            if (pathNode.y -1 >= 0) neighbourList.Add(GetNode(pathNode.x - 1, pathNode.y - 1));
            //Left Up Node
            if (pathNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(pathNode.x - 1, pathNode.y + 1));
        }
        if (pathNode.x < grid.GetWidth())
        {
            //Right Node
            neighbourList.Add(GetNode(pathNode.x + 1, pathNode.y));
            //Right Down Node
            if (pathNode.y - 1 >= 0) neighbourList.Add(GetNode(pathNode.x + 1, pathNode.y - 1));
            //Right Up Node
            if (pathNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(pathNode.x + 1, pathNode.y + 1));
        }
        //Down Node
        if (pathNode.y - 1 >= 0) neighbourList.Add(GetNode(pathNode.x, pathNode.y - 1));
        //Up Node
        if (pathNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(pathNode.x, pathNode.y + 1));

        return neighbourList;
    }

    private PathNode GetNode(int x,int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for (int i = 1; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }
        return lowestFCostNode;
    }
}
