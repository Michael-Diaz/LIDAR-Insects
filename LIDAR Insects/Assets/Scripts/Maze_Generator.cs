using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Generator : MonoBehaviour
{
    public GameObject roomPrefab;

    private GameObject[,] rooms;
    public int size;

    private Stack<Vector2> trace = new Stack<Vector2>();
    private Vector2 currPos = new Vector2(0, 0);
    private bool[,] roomsChecked;

    private bool countCheck = true;
    private int checkedTotal = 0;

    public float genDelay;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new GameObject[size, size];

        roomsChecked = new bool[size, size];

        StartCoroutine(Generate());
    }

    // Creates the Maze
    IEnumerator Generate()
    {
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                rooms[i, j] = Instantiate(roomPrefab, new Vector3(j * 4.0f, 0.0f, i * 4.0f), Quaternion.identity) as GameObject;

        int roomTotal = size * size;

        while (checkedTotal < roomTotal - 1)
        {
            roomsChecked[(int) currPos.x, (int) currPos.y] = true;
            if (countCheck)
                trace.Push(currPos);

            currPos = NextMove((int) currPos.x, (int) currPos.y);

            if (countCheck)
                checkedTotal++;

            yield return new WaitForSeconds(genDelay);
        }
    }

    // Chooses the next legal move from a given room
    Vector2 NextMove(int xPos, int yPos)
    {
        int availableMoves = 0;
        char[] possibleMoves = {'0', '0', '0', '0'};

        if (yPos + 1 > size - 1)
            possibleMoves[0] = '0';
        else if (roomsChecked[xPos, yPos + 1])
            possibleMoves[0] = '0';
        else
        {
            possibleMoves[0] = '1';
            availableMoves++;
        }

        if (yPos - 1 < 0)
            possibleMoves[1] = '0';
        else if (roomsChecked[xPos, yPos - 1])
            possibleMoves[1] = '0';
        else
        {
            possibleMoves[1] = '1';
            availableMoves++;
        }

        if (xPos + 1 > size - 1)
            possibleMoves[2] = '0';
        else if (roomsChecked[xPos + 1, yPos])
            possibleMoves[2] = '0';
        else
        {
            possibleMoves[2] = '1';
            availableMoves++;
        }

        if (xPos - 1 < 0)
            possibleMoves[3] = '0';
        else if (roomsChecked[xPos - 1, yPos])
            possibleMoves[3] = '0';
        else
        {
            possibleMoves[3] = '1';
            availableMoves++;
        }

        int choice = Random.Range(1, availableMoves + 1);
        int choiceCheck = 0;
        int result = -1;

        for (int i = 0; i < 4; i++)
        {
            if (possibleMoves[i] == '1')
                choiceCheck++;
            
            if (choiceCheck == choice)
            {
                result = i;
                break;
            }
        }

        Vector2 retval = new Vector2();

        if (result != -1)
        {
            countCheck = true;

            switch (result)
            {
                case 0:
                    EditMaze(0, xPos, yPos + 1);
                    return new Vector2(xPos, yPos + 1);
                case 1:
                    EditMaze(1, xPos, yPos - 1);
                    return new Vector2(xPos, yPos - 1);
                case 2:
                    EditMaze(2, xPos + 1, yPos);
                    return new Vector2(xPos + 1, yPos);
                case 3:
                    EditMaze(3, xPos - 1, yPos);
                    return new Vector2(xPos - 1, yPos);
            }
        }
        else
        {
            countCheck = false;

            trace.Pop();

            if (trace.Count == 0)
                return new Vector2(0, 0);
            else
                retval = trace.Peek();
        }

        return retval;
    }

    // Alters the walls of the maze as paths are generated
    void EditMaze(int dir, int newX, int newY)
    {
        switch (dir)
        {
            case 0:
                rooms[(int) currPos.x, (int) currPos.y].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                rooms[newX, newY].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 1:
                rooms[(int) currPos.x, (int) currPos.y].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                rooms[newX, newY].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case 2:
                rooms[(int) currPos.x, (int) currPos.y].transform.GetChild(1).gameObject.transform.GetChild(3).gameObject.SetActive(false);
                rooms[newX, newY].transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 3:
                rooms[(int) currPos.x, (int) currPos.y].transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                rooms[newX, newY].transform.GetChild(1).gameObject.transform.GetChild(3).gameObject.SetActive(false);
                break;
        }
    }
}