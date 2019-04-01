using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackService : MonoBehaviour
{
    private Character[] characters;
    private Character[] enemies;

    public AttackService(Character[] characters, Character[] enemies)
    {
        this.characters = characters;
        this.enemies = enemies;
    }

    public void PlayerAttack(int playerIndex, int enemyIndex)
    {
        // apply damage
        int damage = Mathf.Min(this.enemies[enemyIndex].hp, this.characters[playerIndex].atk - this.enemies[enemyIndex].def);
        damage = Mathf.Max(1, damage); // make sure at least 1 damage per hit
        this.enemies[enemyIndex].hp -= damage;
    }

    public void EnemyAttack(int playerIndex, int enemyIndex)
    {
        // apply damage
        int damage = Mathf.Min(this.characters[playerIndex].hp, this.enemies[enemyIndex].atk - this.characters[playerIndex].def);
        damage = Mathf.Max(1, damage); // make sure at least 1 damage per hit
        this.characters[playerIndex].hp -= damage;
    }
}
