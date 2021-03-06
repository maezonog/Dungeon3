﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapState {
    EMPTY,
    WALL,
    PLAYER,
//	STAIRS,
	ITEM,
	ENEMY,

}

public class MapInfo {
    public MapState state = MapState.EMPTY;
    public bool isRoom = false;
	public bool isStairs= false;
	public bool isItem= false;
	public bool isEnemy = false;

    public void setRoom(bool isRoom) {
        this.isRoom = isRoom;
    }

	public void setStairs(bool isStairs){
		this.isStairs = isStairs;
	}

	public void setItem(bool isItem){
		this.isItem = isItem;
	}
	public void setEnemy(bool isEnemy){
		this.isEnemy = isEnemy;
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
	public GameObject stairsPrefab;
	public GameObject enemyPrefab;
	public GameObject itemPrefab;

    public MapInfo[,] mapInfo;
    public int mapWidth = 100;
    public int mapHeight = 100;
    public int maxRoom = 10;

    public int PlayerXpos;
    public int playerZpos;

	public int stairsXpos;
	public int stairsZpos;

	public int itemXpos;
	public int itemZpos;

	public int enemyXpos;
	public int enemyZpos;

    private GameObject playerObject;
	private GameObject stairs;
	private GameObject item;
	private GameObject enemy;
    private PlayerController playerCtrl;

	private List<GameObject> objects = new List<GameObject> ();

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

	//
	//void Update(){
	//}
	//



    /// <summary>
    /// マップデータ作成
    /// </summary>
    void GenerateMapData()
    {
        // int型の2次元配列を作っている。0の場合は壁、1の場合は通路
        //int[,] map = generator.Generate();

        MapGenerator mapGen = new MapGenerator();
		//0が壁、1が通路、2が部屋のint型二次元配列
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
			// 横のランダムなindexを取得
            int xPos = Random.Range(0, mapWidth - 1);

			// 縦のランダムなindexを取得
            int zPos = Random.Range(0, mapHeight - 1);

			// そこが空だったら
            if (mapInfo[xPos, zPos].state == MapState.EMPTY)
            {
				// 該当する座標のマップ情報をプレイヤーに書き換える
                mapInfo[xPos, zPos].SetState(MapState.PLAYER);

				// プレイヤーの座標を設定
                PlayerXpos = xPos;
                playerZpos = zPos;
			
                break;
            }
        }

		while (true)
		{
			// 横のランダムなindexを取得
			int xPos = Random.Range(0, mapWidth - 1);

			// 縦のランダムなindexを取得
			int zPos = Random.Range(0, mapHeight - 1);

			// そこが空だったら
			if (mapInfo[xPos, zPos].state == MapState.EMPTY && mapInfo[xPos, zPos].isRoom)
			{
				// 該当する座標のマップ情報を階段にに書き換える
				mapInfo[xPos, zPos].setStairs(true);

				stairsXpos = xPos;
				stairsZpos = zPos;
				break;
			}
		}

		while (true)
		{
			// 横のランダムなindexを取得
			int xPos = Random.Range(0, mapWidth - 1);

			// 縦のランダムなindexを取得
			int zPos = Random.Range(0, mapHeight - 1);

			// そこが空だったら
			if (mapInfo[xPos, zPos].state == MapState.EMPTY && mapInfo[xPos, zPos].isRoom)
			{
				// 該当する座標のマップ情報をアイテムにに書き換える
				mapInfo[xPos, zPos].setItem(true);

				itemXpos = xPos;
				itemZpos = zPos;
				break;
			}
		}

		while (true)
		{
			// 横のランダムなindexを取得
			int xPos = Random.Range(0, mapWidth - 1);

			// 縦のランダムなindexを取得
			int zPos = Random.Range(0, mapHeight - 1);

			// そこが空だったら
			if (mapInfo[xPos, zPos].state == MapState.EMPTY && mapInfo[xPos, zPos].isRoom)
			{
				// 該当する座標のマップ情報を敵にに書き換える
				mapInfo[xPos, zPos].setEnemy(true);

				enemyXpos = xPos;
				enemyZpos = zPos;
				break;
			}
		}

    }
		
	/// <summary>
	/// オブジェクトを生成している
	/// </summary>

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
					this.objects.Add (wall);
                }

                else if (mapInfo[x, z].state == MapState.PLAYER)
                {
                    // 該当する座標のマップ情報を通路にする
                    playerObject = Instantiate(playerPrefab);
                    playerObject.transform.localPosition = new Vector3(x, 0, z);
                    playerCtrl = playerObject.GetComponent<PlayerController>();
                }

				   //階段生成
				else if (mapInfo[x, z].isStairs) {
					
					stairs = Instantiate(stairsPrefab);
					stairs.transform.localPosition = new Vector3(x, 0, z);
				}

				//アイテム生成
				else if (mapInfo[x, z].isItem) {

					item = Instantiate(itemPrefab);
					item.transform.localPosition = new Vector3(x, 0, z);
				}

				//敵生成
				else if (mapInfo[x, z].isEnemy) {

					enemy = Instantiate(enemyPrefab);
					enemy.transform.localPosition = new Vector3(x, 0, z);
				}

                // 床を生成
                var tile = Instantiate((mapInfo[x,z].isRoom) ? floorPrefab : floorPrefab2);
                tile.transform.localPosition = new Vector3(x, -1, z);
				this.objects.Add (tile);
            }
        }
    }

	public void RegenerateMap() {
		// まップデータの再生成
		// マップ情報の２次元配列を作成。
		// 壁、プレイヤー、敵、階段など該当する座標がどのような情報かを格納しておく為の変数。
		mapInfo = new MapInfo[mapWidth, mapHeight];

		// マップの情報を生成する
		GenerateMapData();

		// 床と壁を全て削除
		for (int i = 0; i < this.objects.Count; i++) {
			Destroy(this.objects[i]);
				}

		this.objects.Clear ();
		
		// マップをもう一度生成、階段、プレイヤーは座標だけ移動
		// 座標の変更
		for(var x= 0; x<mapWidth; x++)
		{
			for (var z = 0; z < mapHeight; z++)
			{

				if (mapInfo [x, z].state == MapState.WALL) {
					// 壁の場合は壁のキューブを生成
					GameObject wall = Instantiate (wallPrefab);
					wall.transform.localPosition = new Vector3 (x, 0, z);
					this.objects.Add (wall);

				} else if (mapInfo [x, z].state == MapState.PLAYER) {
					// 該当する座標のマップ情報を通路にする
					playerObject.transform.localPosition = new Vector3 (x, 0, z);
				}

				//階段生成
				else if (mapInfo [x, z].isStairs) {
					stairs.transform.localPosition = new Vector3 (x, 0, z);
				}

				//アイテム生成
				else if (mapInfo [x, z].isItem) {
					item.transform.localPosition = new Vector3 (x, 0, z);
				}

				//敵生成
				else if (mapInfo[x,z].isEnemy){
					enemy.transform.localPosition = new Vector3 (x, 0, z);
				}

				// 床を生成
				var tile= Instantiate((mapInfo[x,z].isRoom)?floorPrefab:floorPrefab2);
				tile.transform.localPosition = new Vector3 (x, -1, z);
				this.objects.Add (tile);
			}
		}
	}

	public void DeleteItem(){
		Debug.Log ("GET");
		Destroy (itemPrefab);
	}
		
		
}
