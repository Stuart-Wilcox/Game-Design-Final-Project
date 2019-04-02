using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character character;

    public void Start()
    {
        this.SetPosition();


        SpriteRenderer renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderer.sprite = character.sprite;
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
