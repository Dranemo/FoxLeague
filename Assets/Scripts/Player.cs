using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
        Transform chapo = go.transform.Find("Chapo").Find("ChapoModel");
        Transform camera = go.transform.Find("PlayerCamera");
        camera.tag = "MainCamera";

        int playerLayer = 0;


        if (playerEnum == PlayerEnum.player1)
        {
            playerLayer = 6;

            go.name = "Player1";

            chapo.GetComponent<SkinnedMeshRenderer>().material = blueMat; // Mettre la couleur du chapo
        }

        else if (playerEnum == PlayerEnum.player2 || playerEnum == PlayerEnum.AI)
        {
            playerLayer = 7;

            Rect tempRect = camera.GetComponent<Camera>().rect; // Deplacer le render
            tempRect.x = 0.5f;

            camera.GetComponent<Camera>().rect = tempRect;

            chapo.GetComponent<SkinnedMeshRenderer>().material = redMat; // Mettre la couleur du chapo

            if (playerEnum == PlayerEnum.player2)
            {
                go.name = "Player2";
            }
            else if (playerEnum == PlayerEnum.AI)
            {
                go.name = "AI";
            }
        }




        go.layer = playerLayer;

        camera.GetComponent<Camera>().cullingMask &= ~(1 << playerLayer); // Enlever la layer de la camera

        foreach (Transform child in transform) // Mettre la layer
        {
            child.gameObject.layer = playerLayer;
            foreach (Transform child2 in child)
            {
                child2.gameObject.layer = playerLayer;
            }
        }

    }
}
