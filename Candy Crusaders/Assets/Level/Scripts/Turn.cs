using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : ScriptableObject
{
    public bool isMove;
    public bool isAttack;
    public Vector2 movePosition;
    public Vector2 attackPosition;
}
