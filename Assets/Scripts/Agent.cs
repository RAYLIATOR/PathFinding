using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //Array of points
    GameObject[] points;
    //Array of points scripts
    Point[] pointScripts;
    //Table of connections
    float[,] table;
    //Waitinglist
    Queue<int> waitingList;
    //start and end index
    int startIndex;
    int endIndex;
    int prevIndex;
    //start and end points
    GameObject startPoint;
    GameObject endPoint;
    //path to follow
    Stack<GameObject> path;

    Transform target;

    //bool update;

	void Start ()
    {
        //Initialize points array
        points = new GameObject[5];
        //Assign points
        points = GameObject.FindGameObjectsWithTag("Point");
        //Initialize points scripts
        pointScripts = new Point[5];
        //Assign points scripts
        for(int i = 0; i< points.Length; i++)
        {
            pointScripts[i] = points[i].GetComponent<Point>();
        }
        //Fill Table
        FillTable();
        //Initialize path
        path = new Stack<GameObject>();
        FindStartAndEnd();
        //Set initial target

        //update = true;
    }
	
	void Update ()
    {     
        FindPath();
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.1f);
    }

    void FindPath()
    {
        //Assign start and end points
        FindStartAndEnd();
        waitingList = new Queue<int>();
        waitingList.Enqueue(startIndex);
        pointScripts[startIndex].aSD = 0;
        pointScripts[startIndex].from = null;
        while(waitingList.Count != 0)
        {
            int r = waitingList.Dequeue();
            for(int c = 0; c < points.Length; c++)
            {
                if(table[r,c] != -1 && !pointScripts[c].visited)
                {
                    waitingList.Enqueue(c);
                    if ((table[r, c] + pointScripts[r].aSD) <= pointScripts[c].aSD)
                    {
                        pointScripts[c].aSD = table[r, c] + pointScripts[r].aSD;
                        pointScripts[c].from = points[r];
                    }
                }
            }
            pointScripts[r].visited = true;
        }
        path = new Stack<GameObject>();
        GameObject i = endPoint;
        path.Push(i);

        while(i.GetComponent<Point>().from!=null)
        {
            i = i.GetComponent<Point>().from;
            path.Push(i);
        }

        while(path.Peek().GetComponent<Point>().passed)
        {
            path.Pop();
        }

        target = path.Peek().transform;

        if (Vector3.Distance(transform.position, target.transform.position)<=0.5f)
        {
            path.Peek().GetComponent<Point>().passed = true;
            target = path.Peek().transform;
            print(target.name);
        }
        
        //if (path.Count != 0)
        //{
        //    while (path.Peek().GetComponent<Point>().passed)
        //    {
        //        path.Pop();
        //    }
        //}

        //if (!path.Peek().GetComponent<Point>().passed)
        //{
        //    //update = false;
        //    target = path.Peek().transform;
        //    if ((Vector3.Distance(transform.position, target.transform.position)) <= 2)
        //    {
        //        path.Peek().GetComponent<Point>().passed = true;
        //        path.Pop();
        //    }
        //    print(target);
        //}

    }

    void FillTable()
    {
        //Initialize table
        table = new float[points.Length, points.Length];
        for (int r = 0; r < points.Length; r++)
        {
            for(int c = 0; c < points.Length; c++)
            {
                if(IsConnection(points[r], points[c]))
                {
                    table[r, c] = Vector3.Distance(points[r].transform.position, points[c].transform.position);
                }
                else
                {
                    table[r, c] = -1;
                }
            }
        }
    }

    bool IsConnection(GameObject a, GameObject b)
    {
        //Checks if both objects are same
        if(a == b)
        {
            return false;
        }
        //Gets direction between points a and b
        Vector3 dir = b.transform.position - a.transform.position;
        RaycastHit hit;
        if(Physics.Raycast(a.transform.position, dir, out hit, Mathf.Infinity))
        {
            //Checks for direct connection
            if(hit.collider.gameObject == b.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void FindStartAndEnd()
    {
        for(int i = 0; i<points.Length; i++)
        {
            //Assign default start point
            if (startPoint == null)
            {
                startPoint = points[i];
                startIndex = i;
            }
            //Check for start point closest to agent
            else if (pointScripts[i].disToAgent < startPoint.GetComponent<Point>().disToAgent)
            {
                startPoint = points[i];
                startIndex = i;
            }
        }
        for (int i = 0; i < points.Length; i++)
        {
            //Assign default end point
            if (endPoint == null)
            {
                endPoint = points[i];
                endIndex = i;
            }
            //Check for end point closest to target
            else if (pointScripts[i].disToTarget < endPoint.GetComponent<Point>().disToTarget)
            {
                prevIndex = endIndex;
                endPoint = points[i];
                endIndex = i;
            }
        }

        for (int i = 0; i < points.Length; i++)
        {
            if(points[i] == endPoint && i != prevIndex)
            {
                pointScripts[i].passed = false;
                //update = true;
            }
        }
    }

}
