﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor,endColor, shopColor, altarColor;

    public int distanceToEnd;
    public bool includeShop, includeAltar;
    public int minDistanceToShop, maxDistanceToShop, minDistanceToAltar, maxDistanceToAltar;

    public Transform generationPoint;

    public bool makeOutline;

    public enum Direction { up, right, down, left};
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10;

    public LayerMask whatIsRoom;

    private GameObject endRoom, shopRoom, altarRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop, centerAltar;
    public RoomCenter[] potentialCenters;


    void Start()
    {
        Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for(int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if(i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count -1);

                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while(Physics2D.OverlapCircle(generationPoint.position, .2f, whatIsRoom))
            {
              MoveGenerationPoint();
            }
        }

        if(includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        if(includeAltar)
        {
            int altarSelector = Random.Range(minDistanceToAltar, maxDistanceToAltar + 1);
            altarRoom = layoutRoomObjects[altarSelector];
            layoutRoomObjects.RemoveAt(altarSelector);
            altarRoom.GetComponent<SpriteRenderer>().color = altarColor;
        }



        //Create room outlines
        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
        if(includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }
        if(includeAltar)
        {
            CreateRoomOutline(altarRoom.transform.position);
        }


        foreach(GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if(includeShop)
            {
                if(outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if(includeAltar)
            {
                if(outline.transform.position == altarRoom.transform.position)
                {
                    Instantiate(centerAltar, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if(generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.up:
                generationPoint.position += new Vector3(0f, yOffset, 0f);
                break;

            case Direction.down:
                generationPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generationPoint.position += new Vector3(xOffset, 0f, 0f);
                break;

            case Direction.left:
                generationPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
            bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
            bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
            bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
            bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);
            
            int directionCount = 0;
            if(roomAbove)
            {
                directionCount++;
            }
            if(roomBelow)
            {
                directionCount++;
            }
            if(roomLeft)
            {
                directionCount++;
            }
            if(roomRight)
            {
                directionCount++;
            }

            switch(directionCount)
            {
                case 0:
                    Debug.LogError("found noom exists!");
                    break;

                case 1:

                if(roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }

                if(roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }

                if(roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }

                if(roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }

                    break;

                case 2:

                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }

                if(roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }

                if(roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }

                if(roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }

                if(roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftDown, roomPosition, transform.rotation));
                }

                if(roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }

                break;

                case 3:

                if(roomAbove && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }

                if(roomLeft && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }

                if(roomAbove && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }

                if(roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }

                    break;

                case 4:

                if(roomAbove && roomBelow && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }

                    break;
                    
            }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleLeftDown, doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;
}
