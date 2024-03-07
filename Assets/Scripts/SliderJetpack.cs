using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class SliderJetpack : MonoBehaviour
{
    public Slider mainSlider1;
    public Slider mainSlider2;

    private PlayerMovement playerMovement;
    private PlayerMovement playerMovement2;

    private GameManager gameManager;


    private void Awake()
    {
        gameManager = GameManager.GetInstance();
    }

    void Start()
    {
        playerMovement = gameManager.player.GetComponent<PlayerMovement>();
        playerMovement2 = gameManager.player2.GetComponent<PlayerMovement>();


        mainSlider1 = this.transform.Find("SliderP1").GetComponent<Slider>();
        mainSlider2 = this.transform.Find("SliderP2").GetComponent<Slider>();

        mainSlider1.minValue = 0;
        mainSlider1.maxValue = 100;
        mainSlider2.minValue = 0;
        mainSlider2.maxValue = 100;
    }

    void Update()
    {
        float normalizedFlyBoost = Mathf.Clamp(playerMovement.GetBoost(), 0f, 100f);
        mainSlider1.value = normalizedFlyBoost;
        normalizedFlyBoost = Mathf.Clamp(playerMovement2.GetBoost(), 0f, 100f);
        mainSlider2.value = normalizedFlyBoost;
    }
}