using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaClicked : MonoBehaviour
{
    private GameObject confirmSummonImg;
    private GameObject notEnoughCurrencyImg;
    private List<Button> buttons;
    public PlayerData playerData;
    public Text currencyText;

    void Start()
    {
        confirmSummonImg = transform.Find("Dialogue Box").gameObject;
        notEnoughCurrencyImg = transform.Find("NotEnoughCurrency").gameObject;

        buttons = new List<Button>();

        // First 5 buttons are Gacha Buttons
        Button[] canvasButtons = GetComponentsInChildren<Button>();
        for(int i = 0; i < 5; ++i)
        {
            buttons.Add(canvasButtons[i]);
            buttons[i].onClick.AddListener(ShowGachaConfirmOrDenied);
        }

        currencyText.text = $"Currency: {playerData.currency}";
    }

    // If the user has enough currency, the summoning confirmation will be
    // shown, otherwise the not enough currency window will be shown
    void ShowGachaConfirmOrDenied()
    {
        if(playerData.currency >= playerData.summonCost)
        {
            confirmSummonImg.SetActive(true);
        }
        else
        {
            var text = notEnoughCurrencyImg.GetComponentInChildren<Text>();
            text.text = $"Not enough currency. {playerData.summonCost} is needed";
            notEnoughCurrencyImg.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
