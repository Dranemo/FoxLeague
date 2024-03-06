using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class SliderJetpack : MonoBehaviour
{
    public Slider mainSlider1;
    public Slider mainSlider2;

    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        mainSlider1 = this.transform.Find("SliderP1").GetComponent<Slider>();
        mainSlider2 = this.transform.Find("SliderP2").GetComponent<Slider>();

        mainSlider1.minValue = 0;
        mainSlider1.maxValue = 100;
        mainSlider2.minValue = 0;
        mainSlider2.maxValue = 100;
    }

    void Update()
    {
        if (playerMovement != null) 
        {
            float normalizedFlyBoost = Mathf.Clamp(playerMovement.GetBoost(), 0f, 100f);
            mainSlider1.value = normalizedFlyBoost;
        }
    }
}