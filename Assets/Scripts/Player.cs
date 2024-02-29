using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Player : MonoBehaviour
{
    public enum PlayerEnum { player1, player2, AI};
    [SerializeField] private PlayerEnum playerEnum;

    private GameObject go;

    public PlayerEnum GetPlayerEnum()
    {
        return playerEnum;
    }

    public void SetPlayerEnum(PlayerEnum playerEnum_)
    {
        playerEnum = playerEnum_;
    }

    private void Awake()
    {
        go = gameObject;
    }



    private void Start()
    {
        Debug.Log(playerEnum);

        if(playerEnum == PlayerEnum.player1)
        {
            go.name = "Player1";
            go.layer = 6;

            go.transform.Find("PlayerCamera").GetComponent<Camera>().cullingMask &= ~(1 << 6);
            go.transform.Find("PlayerCamera").tag = "MainCamera";

            go.transform.Find("SkinPlayer").gameObject.layer = 6;
            go.transform.Find("ModelPlayer").gameObject.layer = 6;
        }

        else if (playerEnum == PlayerEnum.player2 || playerEnum == PlayerEnum.AI)
        {
            go.layer = 7;

            go.transform.Find("PlayerCamera").GetComponent<Camera>().cullingMask &= ~(1 << 7);

            Rect tempRect = go.transform.Find("PlayerCamera").GetComponent<Camera>().rect;
            tempRect.x = 0.5f;

            go.transform.Find("PlayerCamera").GetComponent<Camera>().rect = tempRect;
            go.transform.Find("PlayerCamera").tag = "MainCamera";

            go.transform.Find("SkinPlayer").gameObject.layer = 7;
            go.transform.Find("ModelPlayer").gameObject.layer = 7;

            if(playerEnum == PlayerEnum.player2)
            {
                go.name = "Player2";
            }
            else if (playerEnum == PlayerEnum.AI)
            {
                go.name = "AI";
            }
        }
    }
}
