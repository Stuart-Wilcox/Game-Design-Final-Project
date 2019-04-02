using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character character;

    public void Start()
    {
        this.SetPosition();
    }

    public void Update()
    {
        // update position
        this.SetPosition();
    }

    void SetPosition()
    {
        this.transform.position = (Vector3)this.character.location + new Vector3(0, 0, -0.1f);
    }
}
