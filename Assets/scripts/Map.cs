using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public float SizeX { get; internal set; }
    public float SizeZ { get; internal set; }

    public Room getStartingRoom()
    {
        return (Room) rooms[0];
    }

    private ArrayList rooms;

    // Use this for initialization
    void Start () {
        SizeX = 0.0f;
        SizeZ = 0.0f;
	}


    // Update is called once per frame
    void Update () {
	}

    internal void addRoom(Room r)
    {
        if( null == rooms) {
            rooms = new ArrayList();
        }
        rooms.Add(r);

    }

    internal int getNumberOfRooms()
    {
        return rooms != null ? rooms.Count : 0;
    }
}
