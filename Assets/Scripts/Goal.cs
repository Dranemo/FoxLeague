using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum PlayerGoal { Player_1,Player_2, endManche }
    [SerializeField] private PlayerGoal playerGoal;

    Material redMat;
    Material blueMat;

    GameManager gameManager;

    public void SetGoal(PlayerGoal _playerGoal)
    {
        playerGoal = _playerGoal;
    }

    public PlayerGoal GetGoal()
    {
        return playerGoal;
    }

    private void Awake()
    {
        gameManager = GameManager.GetInstance();


        blueMat = gameManager.blueMat;
        redMat = gameManager.redMat;
    }

    private void Start()
    {
        GameObject barres = transform.Find("GOAL").Find("Cube").gameObject;

        if(playerGoal == PlayerGoal.Player_2)
        {
            barres.GetComponent<MeshRenderer>().material = redMat;
        }
        else
        {
            barres.GetComponent<MeshRenderer>().material = blueMat;
        }
    }
}
