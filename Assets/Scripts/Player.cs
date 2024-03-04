using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static Player;

public class Player : MonoBehaviour
{
    public enum PlayerEnum { player1, player2, AI};
    [SerializeField] private PlayerEnum playerEnum;

    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;

    private GameManager gameManager;
    private int numberPlayer;


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

        gameManager = GameManager.GetInstance();
        numberPlayer = gameManager.playerNumber;


        Transform chapo = go.transform.Find("Chapo").Find("ChapoModel");
        Transform camera = go.transform.Find("PlayerCamera");
        camera.tag = "MainCamera";

        int playerLayer = 0;


        if (gameManager.playerLoaded == 0)
        {

            playerLayer = 6;

            go.name = "Player1";
            go.tag = "Player";

            chapo.GetComponent<SkinnedMeshRenderer>().material = blueMat; // Mettre la couleur du chapo

            if(numberPlayer == 1)
            {
                Rect tempRect = camera.GetComponent<Camera>().rect; // Deplacer le render
                tempRect.width = 1f;

                camera.GetComponent<Camera>().rect = tempRect;
            }
        }

        else if (gameManager.playerLoaded == 1)
        {
            playerLayer = 7;
            

            chapo.GetComponent<SkinnedMeshRenderer>().material = redMat; // Mettre la couleur du chapo

            if (numberPlayer == 2)
            {
                go.name = "Player2";
                go.tag = "Player2";
                go.GetComponent<Player>().SetPlayerEnum(Player.PlayerEnum.player2);

                Rect tempRect = camera.GetComponent<Camera>().rect; // Deplacer le render
                tempRect.x = 0.5f;

                camera.GetComponent<Camera>().rect = tempRect;
            }
            else if (numberPlayer == 1)
            {
                go.name = "AI";
                go.tag = "AI";
                go.GetComponent<Player>().SetPlayerEnum(Player.PlayerEnum.AI);

                Destroy(camera.gameObject);
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


        gameManager.playerLoaded++;
    }

}
