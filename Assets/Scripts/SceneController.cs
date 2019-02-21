using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapState {
    EMPTY,
    WALL,
    PLAYER,
}

public class MapInfo {
    public MapState state = MapState.EMPTY;
    public bool isRoom = false;

    public void setRoom(bool isRoom) {
        this.isRoom = isRoom;
    }

    public void SetState(MapState state) {
        this.state = state;
    }
}

public class SceneController : MonoBehaviour
{
    public CameraController cameraCtrl;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject floorPrefab2;
    public GameObject playerPrefab;

    public MapInfo[,] mapInfo;
    public int mapWidth = 100;
    public int mapHeight = 100;
    public int maxRoom = 10;
    public int PlayerXpos;
    public int playerZpos;

    private GameObject playerObject;
    private PlayerController playerCtrl;

    void Start()
    {
        // マップ情報の２次元配列を作成。
        // 壁、プレイヤー、敵、階段など該当する座標がどのような情報かを格納しておく為の変数。
        mapInfo = new MapInfo[mapWidth, mapHeight];

        // マップの情報を生成する
        GenerateMapData();

        // マップの情報を元に壁とプレイヤーを生成する
        GenerateObjects();

        // カメラにプレイヤーのGameObjectを認識させる
        cameraCtrl.SetPlayerObj(playerObject);
    }

    /// <summary>
    /// マップデータ作成
    /// </summary>
    void GenerateMapData()
    {
        // int型の2次元配列を作っている。0の場合は壁、1の場合は通路
        //int[,] map = generator.Generate();

        MapGenerator mapGen = new MapGenerator();
        int[,] map = mapGen.GenerateMap(mapWidth, mapHeight, maxRoom);

        for (var x = 0; x < mapWidth; x++)
        {
            for (var z = 0; z < mapHeight; z++)
            {
                if (map[x, z] == 0)
                {
                    // 該当する座標のマップ情報を壁にする
                    MapInfo info = new MapInfo();
                    info.SetState(MapState.WALL);
                    info.setRoom(false);
                    mapInfo[x, z] = info;
                }
                else
                {
                    // 該当する座標のマップ情報を通路にする
                    MapInfo info = new MapInfo();
                    info.SetState(MapState.EMPTY);
                    info.setRoom(map[x, z] == 2);
                    mapInfo[x, z] = info;
                }
            }
        }

        while (true)
        {
            int xPos = Random.Range(0, mapWidth - 1);
            int zPos = Random.Range(0, mapHeight - 1);

            if (mapInfo[xPos, zPos].state == MapState.EMPTY)
            {
                // 該当する座標のマップ情報をプレイヤーにする
                mapInfo[xPos, zPos].SetState(MapState.PLAYER);
                PlayerXpos = xPos;
                playerZpos = zPos;
                break;
            }
        }
    }

    void GenerateObjects()
    {
        for (var x = 0; x < mapWidth; x++)
        {
            for (var z = 0; z < mapHeight; z++)
            {
                if (mapInfo[x, z].state == MapState.WALL)
                {
                    // 壁の場合は壁のキューブを生成
                    GameObject wall = Instantiate(wallPrefab);
                    wall.transform.localPosition = new Vector3(x, 0, z);
                }
                else if (mapInfo[x, z].state == MapState.PLAYER)
                {
                    // 該当する座標のマップ情報を通路にする
                    playerObject = Instantiate(playerPrefab);
                    playerObject.transform.localPosition = new Vector3(x, 0, z);
                    playerCtrl = playerObject.GetComponent<PlayerController>();
                }

                // 床を生成
                var tile = Instantiate((mapInfo[x,z].isRoom) ? floorPrefab : floorPrefab2);
                tile.transform.localPosition = new Vector3(x, -1, z);
            }
        }
    }
}
