using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public Text scoreText;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetInstance();
        Debug.Log(gameManager.score1);

        TextMeshProUGUI textComponent = this.transform.Find("TextScore").GetComponent<TextMeshProUGUI>();

        textComponent.text = "aaaa: " + gameManager.score1.ToString();
    } 

    void Update()
    {
        // Vous pouvez laisser Update() vide si vous n'avez rien à mettre dedans
    }
}