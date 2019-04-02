using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackService : MonoBehaviour
{
    private Character[] characters;
    private Character[] enemies;
    private AudioSource audioSource;

    public AttackService(Character[] characters, Character[] enemies, AudioSource source)
    {
        this.characters = characters;
        this.enemies = enemies;
        this.audioSource = source;
    }

    public void PlayerAttack(int playerIndex, int enemyIndex)
    {
        // apply damage
        int damage = Mathf.Min(this.enemies[enemyIndex].hp, this.characters[playerIndex].atk - this.enemies[enemyIndex].def);
        damage = Mathf.Max(1, damage); // make sure at least 1 damage per hit
        this.enemies[enemyIndex].hp -= damage;
        audioSource.Play();
    }

    public void EnemyAttack(int playerIndex, int enemyIndex)
    {
        // apply damage
        int damage = Mathf.Min(this.characters[playerIndex].hp, this.enemies[enemyIndex].atk - this.characters[playerIndex].def);
        damage = Mathf.Max(1, damage); // make sure at least 1 damage per hit
        this.characters[playerIndex].hp -= damage;
        audioSource.Play();
    }
}
