using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gacha : MonoBehaviour
{
    public GameObject unit;
    public GameObject specificUnit;
    private GameObject[] allunits;

    public PlayerData playerData;
    public Text currencyText;

    private void Start()
    {
        allunits = new GameObject[4];
        allunits[0] = GameObject.Find("Lolipop Knight");
        allunits[1] = GameObject.Find("Cupcake Witch");
        allunits[2] = GameObject.Find("Sweetheart Mage");
        allunits[3] = GameObject.Find("King Werther");
        for (int i=0; i<4; i++)
        {
            allunits[i].SetActive(false);
        }

    }

    private void Update()
    {
        //Go back to main menu
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Assets/InteractiveMobileMenu/Scenes/Menu.unity");
        }
    }

    public void Clicktest()
    {
        int n = Random.Range(0, 10);
        if (n < 3)
        {
            allunits[0].SetActive(true);
        }
        else if (n < 6)
        {
            allunits[1].SetActive(true);
        }
        else if (n < 9)
        {
            allunits[2].SetActive(true);
        }
        else
        {
            allunits[3].SetActive(true);
        }

        playerData.currency -= playerData.summonCost;
        currencyText.text = $"Currency: {playerData.currency}";
    }
}