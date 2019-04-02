using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    public int currency;

    [SerializeField]
    public int summonCost = 20;
}
