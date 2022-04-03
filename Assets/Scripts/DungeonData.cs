using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IP
{
    [CreateAssetMenu(fileName = "NewDungeonData", menuName = "DungeonData")]
    public class DungeonData : ScriptableObject
    {
        public TileBase[] RoomGroundTiles;
        public TileBase[] RoomWallTiles;
        public TileBase RoomNorthDoor;
        public TileBase RoomSouthDoor;
        public TileBase RoomEastDoor;
        public TileBase RoomWestDoor;
        public GameObject[] Enemies;
    }
}