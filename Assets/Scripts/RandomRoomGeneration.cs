using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IP
{
    public class RandomRoomGeneration : MonoBehaviour
    {
        public AllDungeonData allData;
        public Tilemap tilemap;
        public Tilemap collidingTilemap;

        private Transform playerTransform;
        private int[] StartingRoomConnections;
        private Vector3Int NorthEntrance = Vector3Int.zero;
        private Vector3Int SouthEntrance = Vector3Int.zero;
        private Vector3Int EastEntrance = Vector3Int.zero;
        private Vector3Int WestEntrance = Vector3Int.zero;

        private void Start()
        {
            playerTransform = GameObject.FindWithTag("Player").transform;

            int DungeonDataIndex = CreateStartingRoom();

            for (int a = 0; a < StartingRoomConnections.Length; a++)
            {
                switch (StartingRoomConnections[a])
                {
                    case 1:
                        CreateNorthernRooms(DungeonDataIndex);
                        break;
                    case 2:
                        CreateSouthernRooms(DungeonDataIndex);
                        break;
                    case 3:
                        CreateEasternRooms(DungeonDataIndex);
                        break;
                    case 4:
                        CreateWesternRooms(DungeonDataIndex);
                        break;
                }
            }
        }

        private int CreateStartingRoom()
        {
            int DungeonDataIndex = Random.Range(0, allData.dungeonDatas.Length);

            int[] roomConnections = new int[Random.Range(1, 5)];
            int StartX = Random.Range(0, 50);
            int StartY = Random.Range(0, 50);
            int EndX = Random.Range(30, 60) + StartX;
            int EndY = Random.Range(30, 60) + StartY;
            int DiffX = EndX - StartX;
            int DiffY = EndY - StartY;

            int PlayerStartX = Random.Range(StartX, EndX);
            int PlayerStartY = Random.Range(StartY, EndY);
            playerTransform.position = new Vector3(PlayerStartX + 0.5f, PlayerStartY + 0.5f, 0.0f);
            transform.position = new Vector3(PlayerStartX + 0.5f, PlayerStartY + 0.5f, -10.0f);

            int NumberOfEnemies = Random.Range(3, 9);
            for (int i = 0; i < NumberOfEnemies; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(StartX, EndX), Random.Range(StartY, EndY), 0.0f);
                GameObject newEnemy = Instantiate(allData.dungeonDatas[DungeonDataIndex].Enemies[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].Enemies.Length)], randomPos, Quaternion.identity);
            }

            Vector3Int positions = new Vector3Int(StartX, StartY, 0);
            for (int j = 0; j < DiffY; j++)
            {
                for (int i = 0; i < DiffX; i++)
                {
                    tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                    positions.x++;
                }
                positions.x = StartX;
                positions.y++;
            }

            positions = new Vector3Int(StartX - 1, StartY - 1, 0);
            for (int i = 0; i < DiffX + 2; i++)
            {
                collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                positions.x++;
            }
            positions = new Vector3Int(StartX - 1, EndY, 0);
            for (int i = 0; i < DiffX + 2; i++)
            {
                collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                positions.x++;
            }
            positions = new Vector3Int(StartX - 1, StartY, 0);
            for (int i = 0; i < DiffY; i++)
            {
                collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                positions.y++;
            }
            positions = new Vector3Int(EndX, StartY, 0);
            for (int i = 0; i < DiffY; i++)
            {
                collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                positions.y++;
            }

            int[] usedUpConnections = new int[roomConnections.Length];
            for (int i = 0; i < usedUpConnections.Length; i++)
            {
                usedUpConnections[i] = 0;
            }

            for (int i = 0; i < roomConnections.Length; i++)
            {
                bool ConnectionUsed = true;
                while (ConnectionUsed)
                {
                    ConnectionUsed = false;
                    roomConnections[i] = Random.Range(1, 5);
                    for (int j = 0; j < usedUpConnections.Length; j++)
                    {
                        if (roomConnections[i] == usedUpConnections[j])
                        {
                            ConnectionUsed = true;
                        }
                    }
                }
                usedUpConnections[i] = roomConnections[i];

                switch (roomConnections[i])
                {
                    case 1: //North
                        positions = new Vector3Int(Random.Range(StartX, EndX), EndY, 0);
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x++;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x -= 2;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);

                        //Make Corridor
                        positions.y++;
                        int LengthOfNorthCorridor = Random.Range(10, 21);
                        for (int k = 0; k < LengthOfNorthCorridor; k++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                                positions.x++;
                            }
                            positions.x -= 3;
                            positions.y++;
                        }
                        positions.x--;
                        positions.y -= LengthOfNorthCorridor;
                        for (int k = 0; k < LengthOfNorthCorridor; k++)
                        {
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.x += 4;
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.x -= 4;
                            positions.y++;
                        }
                        positions.x += 2;
                        NorthEntrance = positions;

                        break;
                    case 2: //South
                        positions = new Vector3Int(Random.Range(StartX, EndX), StartY - 1, 0);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        collidingTilemap.SetTile(positions, null);
                        positions.x++;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x -= 2;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);

                        //Make Corridor
                        positions.y--;
                        int LengthOfSouthCorridor = Random.Range(10, 21);
                        for (int k = 0; k < LengthOfSouthCorridor; k++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                                positions.x++;
                            }
                            positions.x -= 3;
                            positions.y--;
                        }
                        positions.x--;
                        positions.y += LengthOfSouthCorridor;
                        for (int k = 0; k < LengthOfSouthCorridor; k++)
                        {
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.x += 4;
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.x -= 4;
                            positions.y--;
                        }
                        positions.x += 2;
                        SouthEntrance = positions;

                        break;
                    case 3: //East
                        positions = new Vector3Int(EndX, Random.Range(StartY, EndY), 0);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        collidingTilemap.SetTile(positions, null);
                        positions.y++;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.y -= 2;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);

                        //Make Corridor
                        positions.x++;
                        int LengthOfEastCorridor = Random.Range(10, 21);
                        for (int k = 0; k < LengthOfEastCorridor; k++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                                positions.y++;
                            }
                            positions.y -= 3;
                            positions.x++;
                        }
                        positions.y--;
                        positions.x -= LengthOfEastCorridor;
                        for (int k = 0; k < LengthOfEastCorridor; k++)
                        {
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.y += 4;
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.y -= 4;
                            positions.x++;
                        }
                        positions.y += 2;
                        EastEntrance = positions;

                        break;
                    case 4: //West
                        positions = new Vector3Int(StartX - 1, Random.Range(StartY, EndY), 0);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        collidingTilemap.SetTile(positions, null);
                        positions.y++;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.y -= 2;
                        collidingTilemap.SetTile(positions, null);
                        tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);

                        //Make Corridor
                        positions.x--;
                        int LengthOfWestCorridor = Random.Range(10, 21);
                        for (int k = 0; k < LengthOfWestCorridor; k++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                tilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomGroundTiles.Length)]);
                                positions.y++;
                            }
                            positions.y -= 3;
                            positions.x--;
                        }
                        positions.y--;
                        positions.x += LengthOfWestCorridor;
                        for (int k = 0; k < LengthOfWestCorridor; k++)
                        {
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.y += 4;
                            collidingTilemap.SetTile(positions, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[DungeonDataIndex].RoomWallTiles.Length)]);
                            positions.y -= 4;
                            positions.x--;
                        }
                        positions.y += 2;
                        WestEntrance = positions;

                        break;
                }
            }

            StartingRoomConnections = roomConnections;

            return DungeonDataIndex;
        }

        private void CreateNorthernRooms(int dungeonDataIndex)
        {
            int AnotherRoom = 1;
            while (AnotherRoom == 1)
            {
                int StartX = NorthEntrance.x - Random.Range(2, 30);
                int StartY = NorthEntrance.y;
                int EndX = Random.Range(30, 60) + StartX;
                int EndY = Random.Range(30, 60) + StartY;
                int DiffX = EndX - StartX;
                int DiffY = EndY - StartY;

                int NumberOfEnemies = Random.Range(3, 9);
                for (int i = 0; i < NumberOfEnemies; i++)
                {
                    Vector3 randomPos = new Vector3(Random.Range(StartX, EndX), Random.Range(StartY, EndY), 0.0f);
                    GameObject newEnemy = Instantiate(allData.dungeonDatas[dungeonDataIndex].Enemies[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].Enemies.Length)], randomPos, Quaternion.identity);
                }

                Vector3Int positions = new Vector3Int(StartX, StartY, 0);
                for (int j = 0; j < DiffY; j++)
                {
                    for (int i = 0; i < DiffX; i++)
                    {
                        tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x++;
                    }
                    positions.x = StartX;
                    positions.y++;
                }

                positions = new Vector3Int(StartX - 1, StartY - 1, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, EndY, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }
                positions = new Vector3Int(EndX, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }

                positions = new Vector3Int(NorthEntrance.x - 1, NorthEntrance.y - 1, 0);
                for (int i = 0; i < 3; i++)
                {
                    collidingTilemap.SetTile(positions, null);
                    positions.x++;
                }

                AnotherRoom = Random.Range(0, 2);
                if (AnotherRoom == 1)
                {
                    positions = new Vector3Int(Random.Range(StartX, EndX), EndY, 0);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    collidingTilemap.SetTile(positions, null);
                    positions.x++;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    positions.x -= 2;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);

                    //Make Corridor
                    positions.y++;
                    int LengthOfNorthCorridor = Random.Range(10, 21);
                    for (int k = 0; k < LengthOfNorthCorridor; k++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                            positions.x++;
                        }
                        positions.x -= 3;
                        positions.y++;
                    }
                    positions.x--;
                    positions.y -= LengthOfNorthCorridor;
                    for (int k = 0; k < LengthOfNorthCorridor; k++)
                    {
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.x += 4;
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.x -= 4;
                        positions.y++;
                    }
                    positions.x += 2;
                    NorthEntrance = positions;
                }
            }
        }

        private void CreateSouthernRooms(int dungeonDataIndex)
        {
            int AnotherRoom = 1;
            while (AnotherRoom == 1)
            {
                int StartX = SouthEntrance.x - Random.Range(2, 30);
                int StartY = SouthEntrance.y - Random.Range(20, 50);
                int EndX = Random.Range(30, 60) + StartX;
                int EndY = SouthEntrance.y + 1;
                int DiffX = EndX - StartX;
                int DiffY = EndY - StartY;

                int NumberOfEnemies = Random.Range(3, 9);
                for (int i = 0; i < NumberOfEnemies; i++)
                {
                    Vector3 randomPos = new Vector3(Random.Range(StartX, EndX), Random.Range(StartY, EndY), 0.0f);
                    GameObject newEnemy = Instantiate(allData.dungeonDatas[dungeonDataIndex].Enemies[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].Enemies.Length)], randomPos, Quaternion.identity);
                }

                Vector3Int positions = new Vector3Int(StartX, StartY, 0);
                for (int j = 0; j < DiffY; j++)
                {
                    for (int i = 0; i < DiffX; i++)
                    {
                        tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x++;
                    }
                    positions.x = StartX;
                    positions.y++;
                }

                positions = new Vector3Int(StartX - 1, StartY - 1, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, EndY, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }
                positions = new Vector3Int(EndX, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }

                positions = new Vector3Int(SouthEntrance.x - 1, SouthEntrance.y + 1, 0);
                for (int i = 0; i < 3; i++)
                {
                    collidingTilemap.SetTile(positions, null);
                    positions.x++;
                }

                AnotherRoom = Random.Range(0, 2);
                if (AnotherRoom == 1)
                {
                    positions = new Vector3Int(Random.Range(StartX, EndX), StartY - 1, 0);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    collidingTilemap.SetTile(positions, null);
                    positions.x++;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    positions.x -= 2;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);

                    //Make Corridor
                    positions.y--;
                    int LengthOfSouthCorridor = Random.Range(10, 21);
                    for (int k = 0; k < LengthOfSouthCorridor; k++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                            positions.x++;
                        }
                        positions.x -= 3;
                        positions.y--;
                    }
                    positions.x--;
                    positions.y += LengthOfSouthCorridor;
                    for (int k = 0; k < LengthOfSouthCorridor; k++)
                    {
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.x += 4;
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.x -= 4;
                        positions.y--;
                    }
                    positions.x += 2;
                    SouthEntrance = positions;
                }
            }
        }

        private void CreateEasternRooms(int dungeonDataIndex)
        {
            int AnotherRoom = 1;
            while (AnotherRoom == 1)
            {
                int StartX = EastEntrance.x;
                int StartY = EastEntrance.y - Random.Range(20, 30);
                int EndX = Random.Range(30, 60) + StartX;
                int EndY = StartY + Random.Range(30, 50);
                int DiffX = EndX - StartX;
                int DiffY = EndY - StartY;

                int NumberOfEnemies = Random.Range(3, 9);
                for (int i = 0; i < NumberOfEnemies; i++)
                {
                    Vector3 randomPos = new Vector3(Random.Range(StartX, EndX), Random.Range(StartY, EndY), 0.0f);
                    GameObject newEnemy = Instantiate(allData.dungeonDatas[dungeonDataIndex].Enemies[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].Enemies.Length)], randomPos, Quaternion.identity);
                }

                Vector3Int positions = new Vector3Int(StartX, StartY, 0);
                for (int j = 0; j < DiffY; j++)
                {
                    for (int i = 0; i < DiffX; i++)
                    {
                        tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x++;
                    }
                    positions.x = StartX;
                    positions.y++;
                }

                positions = new Vector3Int(StartX - 1, StartY - 1, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, EndY, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }
                positions = new Vector3Int(EndX, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }

                positions = new Vector3Int(EastEntrance.x - 1, EastEntrance.y - 1, 0);
                for (int i = 0; i < 3; i++)
                {
                    collidingTilemap.SetTile(positions, null);
                    positions.y++;
                }

                AnotherRoom = Random.Range(0, 2);
                if (AnotherRoom == 1)
                {
                    positions = new Vector3Int(EndX, Random.Range(StartY, EndY), 0);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    collidingTilemap.SetTile(positions, null);
                    positions.y++;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    positions.y -= 2;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);

                    //Make Corridor
                    positions.x++;
                    int LengthOfEastCorridor = Random.Range(10, 21);
                    for (int k = 0; k < LengthOfEastCorridor; k++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                            positions.y++;
                        }
                        positions.y -= 3;
                        positions.x++;
                    }
                    positions.y--;
                    positions.x -= LengthOfEastCorridor;
                    for (int k = 0; k < LengthOfEastCorridor; k++)
                    {
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.y += 4;
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.y -= 4;
                        positions.x++;
                    }
                    positions.y += 2;
                    EastEntrance = positions;
                }
            }
        }

        private void CreateWesternRooms(int dungeonDataIndex)
        {
            int AnotherRoom = 1;
            while (AnotherRoom == 1)
            {
                int StartX = WestEntrance.x - Random.Range(20, 50);
                int StartY = WestEntrance.y - Random.Range(20, 30);
                int EndX = WestEntrance.x + 1;
                int EndY = StartY + Random.Range(30, 50);
                int DiffX = EndX - StartX;
                int DiffY = EndY - StartY;

                int NumberOfEnemies = Random.Range(3, 9);
                for (int i = 0; i < NumberOfEnemies; i++)
                {
                    Vector3 randomPos = new Vector3(Random.Range(StartX, EndX), Random.Range(StartY, EndY), 0.0f);
                    GameObject newEnemy = Instantiate(allData.dungeonDatas[dungeonDataIndex].Enemies[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].Enemies.Length)], randomPos, Quaternion.identity);
                }

                Vector3Int positions = new Vector3Int(StartX, StartY, 0);
                for (int j = 0; j < DiffY; j++)
                {
                    for (int i = 0; i < DiffX; i++)
                    {
                        tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                        positions.x++;
                    }
                    positions.x = StartX;
                    positions.y++;
                }

                positions = new Vector3Int(StartX - 1, StartY - 1, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, EndY, 0);
                for (int i = 0; i < DiffX + 2; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.x++;
                }
                positions = new Vector3Int(StartX - 1, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }
                positions = new Vector3Int(EndX, StartY, 0);
                for (int i = 0; i < DiffY; i++)
                {
                    collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                    positions.y++;
                }

                positions = new Vector3Int(WestEntrance.x + 1, WestEntrance.y - 1, 0);
                for (int i = 0; i < 3; i++)
                {
                    collidingTilemap.SetTile(positions, null);
                    positions.y++;
                }

                AnotherRoom = Random.Range(0, 2);
                if (AnotherRoom == 1)
                {
                    positions = new Vector3Int(StartX - 1, Random.Range(StartY, EndY), 0);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    collidingTilemap.SetTile(positions, null);
                    positions.y++;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                    positions.y -= 2;
                    collidingTilemap.SetTile(positions, null);
                    tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);

                    //Make Corridor
                    positions.x--;
                    int LengthOfWestCorridor = Random.Range(10, 21);
                    for (int k = 0; k < LengthOfWestCorridor; k++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            tilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomGroundTiles.Length)]);
                            positions.y++;
                        }
                        positions.y -= 3;
                        positions.x--;
                    }
                    positions.y--;
                    positions.x += LengthOfWestCorridor;
                    for (int k = 0; k < LengthOfWestCorridor; k++)
                    {
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.y += 4;
                        collidingTilemap.SetTile(positions, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles[Random.Range(0, allData.dungeonDatas[dungeonDataIndex].RoomWallTiles.Length)]);
                        positions.y -= 4;
                        positions.x--;
                    }
                    positions.y += 2;
                    WestEntrance = positions;
                }
            }
        }
    }
}