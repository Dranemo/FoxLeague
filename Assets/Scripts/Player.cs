using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static Player;

public class Player : MonoBehaviour
{
    public enum PlayerEnum { player1, player2, AI };
    [SerializeField] private PlayerEnum playerEnum;

    private Material redMat;
    private Material blueMat;

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


        blueMat = gameManager.blueMat;
        redMat = gameManager.redMat;


        Transform chapo = go.transform.Find("animal_people_beanie_wolf_1_onWear_WithAccessoryLogic").Find("animal_people_beanie_wolf_1_onWear_WithAccessoryLogic");
        Transform camera = go.transform.Find("PlayerCamera");
        Transform arrow = camera.Find("3D RightArrow");
        camera.tag = "MainCamera";
 
        int playerLayer = 0;
        int arrowLayerAdv = 0;
 
 
        if (gameManager.playerLoaded == 0)
        {
 
            playerLayer = 6;
            arrowLayerAdv = 9;



            go.name = "Player1";
            go.tag = "Player";
 
            chapo.GetComponent<SkinnedMeshRenderer>().material = blueMat; // Mettre la couleur du chapo
            arrow.GetComponent<SkinnedMeshRenderer>().material = blueMat; // MEttre la couleur de la fleche


            if (numberPlayer == 1)
            {
                Rect tempRect = camera.GetComponent<Camera>().rect; // Deplacer le render
                tempRect.width = 1f;
 
                camera.GetComponent<Camera>().rect = tempRect;
            }
        }
 
        else if (gameManager.playerLoaded == 1)
        {
            playerLayer = 7;
            arrowLayerAdv = 8;



            chapo.GetComponent<SkinnedMeshRenderer>().material = redMat; // Mettre la couleur du chapo
            arrow.GetComponent<SkinnedMeshRenderer>().material = redMat; // MEttre la couleur de la fleche


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


        camera.GetComponent<Camera>().cullingMask &= ~(1 << playerLayer | 1 << arrowLayerAdv); // Enlever la layer de la camera

        foreach (Transform child in transform) // Mettre la layer
        {
            child.gameObject.layer = playerLayer;
            foreach (Transform child2 in child)
            {
                if(child2 != arrow)
                {
                    child2.gameObject.layer = playerLayer;
                }
                else
                {
                    child2.gameObject.layer = playerLayer + 2;
                }
                
            }
        }



        gameManager.playerLoaded++;
    }

} 
