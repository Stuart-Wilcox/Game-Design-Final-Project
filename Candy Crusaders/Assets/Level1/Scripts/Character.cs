using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Character Stats")]
public class Character : ScriptableObject
{
    [SerializeField]
    public bool isActive;

    [SerializeField]
    public int maxHp;

    [SerializeField]
    public int hp;

    [SerializeField]
    public int atk;

    [SerializeField]
    public int def;

    [SerializeField]
    public int spd;

    [SerializeField]
    public int res;

    [SerializeField]
    public Vector2 location; // X MUST BE 0-14 (inclusive) and Y MUST BE 0-9 (inlcusive)!!
}
