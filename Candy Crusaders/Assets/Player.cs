using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Player : ScriptableObject
{
    [SerializeField]
    public Character[] characters;

    [SerializeField]
    public Character[] activeCharacters;

    [SerializeField]
    public int activeLevel;

    [SerializeField]
    public bool[] unlocked;
}
