using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Player player;

    private Character[] players;
    public Character character;


    private MoveService moveService;

    public void Start()
    {
        this.players = this.player.activeCharacters;

        this.SetPosition();
    }

    public void Update()
    {
        // update position
        this.SetPosition();
    }

    private void SetPosition()
    {
        this.transform.position = (Vector3)this.character.location + new Vector3(0, 0, -0.1f);
    }

    public void SetMoveService(MoveService moveService)
    {
        this.moveService = moveService;
    }

    public Turn TakeTurn()
    {
        // build the turn, which is moving every time
        Turn turn = new Turn();

        // find the distances of players
        int closestPlayerIndex = 0;
        float minDistance = 100;
        for(int i = 0; i < this.players.Length; i++)
        {
            if (this.players[i].hp < 1) continue; // ignore dead players

            float distance = (this.players[i].location - this.character.location).magnitude;
            if(distance < minDistance)
            {
                closestPlayerIndex = i;
                minDistance = distance;
            }
        }

        turn.isMove = !this.IsPlayerInRange(closestPlayerIndex); // move if not in range
        turn.isAttack = !turn.isMove; // attack if in range

        if (turn.isMove)
        {
            // find the closest spot to move to the player
            turn.movePosition = this.CalcMoveLocation(closestPlayerIndex);
        }
        else
        {
            // attack the closest player
            turn.attackPosition = this.CalcAttackLocation(closestPlayerIndex);
        }

        return turn;
    }

    private bool IsPlayerInRange(int playerIndex)
    {
        Vector2 characterLocation = this.character.location;
        Vector2 playerLocation = this.players[playerIndex].location;
        int x = (int)characterLocation.x;
        int y = (int)characterLocation.y;

        int[] pos = { 0, 1, 2, 1, 0 };
        for (int i = x - 2, k = 0; i <= x + 2; i++, k++)
        {
            if (i < 0 || i > 15) continue;
            for (int j = y - pos[k]; j <= y + pos[k]; j++)
            {

                if (j < 0 || j > 9) continue;

               if(i == playerLocation.x && j == playerLocation.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private Vector2 CalcMoveLocation(int playerIndex)
    {
        Vector2 characterLocation = this.character.location;
        Vector2 playerLocation = this.players[playerIndex].location;

        Vector2 moveLocation = new Vector2(characterLocation.x, characterLocation.y);
        float minDistance = (moveLocation - playerLocation).magnitude;

        int x = (int)characterLocation.x;
        int y = (int)characterLocation.y;

        int[] pos = { 0, 1, 2, 1, 0 };
        for (int i = x - 2, k = 0; i <= x + 2; i++, k++)
        {
            if (i < 0 || i > 15) continue;
            for (int j = y - pos[k]; j <= y + pos[k]; j++)
            {

                if (j < 0 || j > 9) continue;

                // spot occupied, cant move there
                if (this.moveService.IsPlayerAtLocation(new Vector2(i, j)) || this.moveService.IsEnemyAtLocation(new Vector2(i, j))) continue;

                // find best spot
                float distance = (playerLocation - new Vector2(i, j)).magnitude;
                if(distance < minDistance)
                {
                    moveLocation = new Vector2(i, j);
                    minDistance = distance;
                }
            }
        }

        return moveLocation;
    }

    private Vector2 CalcAttackLocation(int playerIndex)
    {
        return this.players[playerIndex].location;
    }
}
