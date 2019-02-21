using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int yOffset = 4;
    public int zOffset = -2;

    private GameObject playerObj;

    void Start()
    {
        this.transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    void Update()
    {
       
    }

    void LateUpdate()
    {
        // プレイヤーを追従させる
        // なぜ以下の記事がわかりやすいです
        // LateUpdateからhttps://www.shibuya24.info/entry/2016/07/04/090000。
        this.transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y + yOffset, playerObj.transform.position.z + zOffset);
    }

    /// <summary>
    /// プレイヤーのオブジェクトを設定
    /// </summary>
    public void SetPlayerObj(GameObject obj) {
        this.playerObj = obj;
    }
}
