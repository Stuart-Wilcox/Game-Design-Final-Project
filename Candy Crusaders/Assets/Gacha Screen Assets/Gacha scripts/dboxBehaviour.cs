using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dboxBehaviour : MonoBehaviour
{
    private GameObject unit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        unit = GameObject.FindGameObjectWithTag("0");
        unit.SetActive(true);
    }
}
