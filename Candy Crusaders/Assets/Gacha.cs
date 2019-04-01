using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public GameObject unit;
    public GameObject specificUnit;
    private GameObject[] allunits;

    private void Start()
    {
        allunits = new GameObject[4];
        allunits[0] = GameObject.Find("King Werther");
        allunits[1] = GameObject.Find("Lolipop Knight");
        allunits[2] = GameObject.Find("Cupcake Witch");
        allunits[3] = GameObject.Find("Toffee Knight");
        for(int i=0; i<4; i++)
        {
            allunits[i].SetActive(false);
        }
    }


    public void Clicktest()
    {
        allunits[0].SetActive(true);
    }
}