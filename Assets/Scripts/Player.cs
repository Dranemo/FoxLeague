using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Player : MonoBehaviour
{
    public enum PlayerEnum { player1, player2, AI};
    [SerializeField] private PlayerEnum playerEnum;

    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;


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
        Transform skinPlayer = go.transform.Find("SkinPlayer");
        Transform modelPlayer = go.transform.Find("ModelPlayer");
        Transform chapo = go.transform.Find("Chapo");

        Transform camera = go.transform.Find("PlayerCamera");
        camera.tag = "MainCamera";


        if (playerEnum == PlayerEnum.player1)
        {
            go.name = "Player1";
            go.layer = 6;

            camera.GetComponent<Camera>().cullingMask &= ~(1 << 6);

            skinPlayer.gameObject.layer = 6;
            modelPlayer.gameObject.layer = 6;
            chapo.gameObject.layer = 6;

            chapo.transform.Find("ChapoModel").GetComponent<SkinnedMeshRenderer>().material = blueMat;
        }

        else if (playerEnum == PlayerEnum.player2 || playerEnum == PlayerEnum.AI)
        {
            go.layer = 7;

            camera.GetComponent<Camera>().cullingMask &= ~(1 << 7);

            Rect tempRect = camera.GetComponent<Camera>().rect;
            tempRect.x = 0.5f;

            camera.GetComponent<Camera>().rect = tempRect;

            skinPlayer.gameObject.layer = 7;
            modelPlayer.gameObject.layer = 7;
            chapo.gameObject.layer = 7;

            chapo.transform.Find("ChapoModel").GetComponent<SkinnedMeshRenderer>().material = redMat;

            if (playerEnum == PlayerEnum.player2)
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
