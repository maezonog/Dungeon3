using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SceneController sceneCtrl;

    private bool inMove = false;
    private Vector3 fromPos;
    private Vector3 toPos;
    private float t = 0;

    void Start()
    {
        sceneCtrl = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    void Update()
    {
        if (!inMove) {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveUp();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveDown();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
        }

        if (inMove && t < 1)
        {
            t += Time.deltaTime * (1.0f / 0.2f);
            this.transform.position = Vector3.Lerp(fromPos, toPos, t);

            if (t >= 1)
            {
                inMove = false;
                t = 0;
            }
        }
    }

    void MoveUp()
    {
        int maxZPos = sceneCtrl.mapHeight - 1;
        int nextZPos = sceneCtrl.playerZpos + 1;
        if (nextZPos <= maxZPos)
        {
            if (sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, nextZPos].state == MapState.EMPTY)
            {
                sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, sceneCtrl.playerZpos].SetState(MapState.EMPTY);
                sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, nextZPos].SetState(MapState.PLAYER);
                sceneCtrl.playerZpos = nextZPos;

                inMove = true;
                fromPos = this.transform.position;
                toPos = new Vector3(this.transform.position.x, this.transform.position.y, nextZPos);
            }
        }
    }

    void MoveRight()
    {
        int maxXPos = sceneCtrl.mapWidth - 1;
        int nextXPos = sceneCtrl.PlayerXpos + 1;
        if (nextXPos <= maxXPos)
        {
            if (sceneCtrl.mapInfo[nextXPos, sceneCtrl.playerZpos].state == MapState.EMPTY)
            {
                sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, sceneCtrl.playerZpos].SetState(MapState.EMPTY);
                sceneCtrl.mapInfo[nextXPos, sceneCtrl.playerZpos].SetState(MapState.PLAYER);
                sceneCtrl.PlayerXpos = nextXPos;

                inMove = true;
                fromPos = this.transform.position;
                toPos = new Vector3(nextXPos, this.transform.position.y, this.transform.position.z);
            }
        }
    }

    void MoveDown()
    {
        int nextZPos = sceneCtrl.playerZpos - 1;
        if (nextZPos >= 0)
        {
            if (sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, nextZPos].state == MapState.EMPTY)
            {
                sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, sceneCtrl.playerZpos].SetState(MapState.EMPTY);
                sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, nextZPos].SetState(MapState.PLAYER);
                sceneCtrl.playerZpos = nextZPos;

                inMove = true;
                fromPos = this.transform.position;
                toPos = new Vector3(this.transform.position.x, this.transform.position.y, nextZPos);
            }
        }
    }

    void MoveLeft()
    {
        int nextXPos = sceneCtrl.PlayerXpos - 1;
        if (nextXPos >= 0)
        {
            if (sceneCtrl.mapInfo[nextXPos, sceneCtrl.playerZpos].state == MapState.EMPTY)
            {
                sceneCtrl.mapInfo[sceneCtrl.PlayerXpos, sceneCtrl.playerZpos].SetState(MapState.EMPTY);
                sceneCtrl.mapInfo[nextXPos, sceneCtrl.playerZpos].SetState(MapState.PLAYER);
                sceneCtrl.PlayerXpos = nextXPos;

                inMove = true;
                fromPos = this.transform.position;
                toPos = new Vector3(nextXPos, this.transform.position.y, this.transform.position.z);
            }
        }
    }
}
