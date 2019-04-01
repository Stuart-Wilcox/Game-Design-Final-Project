//using System.String.;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveService : MonoBehaviour
{
    // stats for both characters
    private Character[] characters;
    private Character[] enemies;
    
    // hold the tile types to display them when player clicked
    private Transform greenTile;
    private Transform redTile;

    // player clicked state
    private bool playerHighlighted;
    private int playerHighlightedIndex;

    // hold the instances of tiles to delete later
    private List<Transform> tiles;

    public MoveService(Character[] characters, Character[] enemies, Transform greenTile, Transform redTile)
    {
        this.characters = characters;
        this.enemies = enemies;
        this.greenTile = greenTile;
        this.redTile = redTile;
        this.tiles = new List<Transform>();
    }

    public bool IsMovePositionClicked(int characterIndex, Vector3 clickedPosition)
    {
        return this.IsClickedLocationInRange(characterIndex, clickedPosition) && !this.IsEnemyAtLocation(clickedPosition);
    }

    public bool IsAttackPositionClicked(int characterIndex, Vector3 clickedPosition)
    {
        return this.IsClickedLocationInRange(characterIndex, clickedPosition) && this.IsEnemyAtLocation(clickedPosition);
    }

    public void PlayerClicked(int characterIndex)
    {
        if (this.playerHighlighted)
        {
            this.playerHighlightedIndex = -1;
            UndisplayMovePositions();
        }
        else
        {
            this.playerHighlightedIndex = characterIndex;
            DisplayMovePositions(characterIndex);
        }
        this.playerHighlighted = !this.playerHighlighted;
    }

    public bool IsPlayerHighlighted()
    {
        return this.playerHighlighted;
    }

    public int GetPlayerHighlightedIndex()
    {
        return this.playerHighlightedIndex;
    }

    private void DisplayMovePositions(int characterIndex)
    {
        Vector3 location = this.characters[characterIndex].location;
        int x = (int)location.x;
        int y = (int)location.y;

        this.tiles = new List<Transform>(); // reset the list

        int[] pos = { 0, 1, 2, 1, 0 }; // for non-square pattern
        for (int i = x - 2, k = 0; i <= x + 2; i++, k++)
        {
            if (i < 0 || i > 15) continue; // dont highlight tiles off the grid

            for (int j = y - pos[k]; j <= y + pos[k]; j++)
            {
                if (j < 0 || j > 9) continue; // dont highlight tiles off the grid
                if (this.IsPlayerAtLocation(new Vector3(i, j, 0))) continue; // dont highlight the player's tile

                if (this.IsEnemyAtLocation(new Vector3(i,j,0)))
                {
                    // find if there is a legal move spot
                    int enemyIndex = -1;
                    Vector2 position = new Vector2(i, j);
                    for (int l = 0; l < this.enemies.Length; l++)
                    {
                        Character enemy = this.enemies[l];
                        if (enemy.location.x == position.x && enemy.location.y == position.y)
                        {
                            enemyIndex = l;
                            break;
                        }
                    }

                    Vector2 calculatedAttackPosition = this.CalculatePlayerAttackPosition(characterIndex, enemyIndex);
                    if (calculatedAttackPosition.x == -1 && calculatedAttackPosition.y == -1) continue;

                    Transform redTileInstance = (Transform)Instantiate(redTile, new Vector3(i, j, -0.2f), Quaternion.Euler(0, 0, 0));
                    this.tiles.Add(redTileInstance);
                }
                else
                {
                    Transform greenTileInstance = (Transform)Instantiate(greenTile, new Vector3(i, j, -0.2f), Quaternion.Euler(0, 0, 0)); // create green tile
                    this.tiles.Add(greenTileInstance);
                }
            }
        }
    }

    private void UndisplayMovePositions()
    {
        foreach (Transform greenTile in this.tiles)
        {
            Destroy(greenTile.gameObject); // destroy the tile
        }

        this.tiles = new List<Transform>(); // reset the list
    }

    public void MovePlayer(int characterIndex, Vector3 position)
    {
        this.characters[characterIndex].location = new Vector2(position.x, position.y); // set the new position on the object
        this.UndisplayMovePositions(); // take away the highlighted tiles
        this.playerHighlighted = false; // change state
    }

    public void MoveEnemy(int enemyIndex, Vector3 position)
    {
        this.enemies[enemyIndex].location = new Vector2(position.x, position.y);
    }

    public Vector2 CalculatePlayerAttackPosition(int characterIndex, int enemyIndex)
    {
        Vector2 position = this.enemies[enemyIndex].location;

        Vector2[] positions = { position + Vector2.up, position + Vector2.down, position + Vector2.left, position + Vector2.right };

        // find the legal positions
        List<Vector2> legalPositions = new List<Vector2>();
        legalPositions.Add(new Vector2(-1, -1));
        for (int i = 0; i < positions.Length; i++)
        {
            if (this.IsLegalPosition(characterIndex, (Vector3)positions[i]))
            {
                legalPositions.Add(positions[i]);
            }
        }

        // find the closest position to current
        Vector2 currentPosition = this.characters[characterIndex].location;
        Vector2 closestPosition = legalPositions[0];
        float minDistance = (closestPosition - currentPosition).magnitude;
        for (int i = 0; i < legalPositions.Count; i++)
        {
            float distance = (legalPositions[i] - currentPosition).magnitude;
            if (distance < minDistance)
            {
                closestPosition = legalPositions[i];
                minDistance = distance;
            }
        }

        return closestPosition;
    }

    public Vector2 CalculateEnemyAttackPosition(int characterIndex, int enemyIndex)
    {
        Vector2 position = this.characters[enemyIndex].location;

        Vector2[] positions = { position + Vector2.up, position + Vector2.down, position + Vector2.left, position + Vector2.right };

        // find the legal positions
        List<Vector2> legalPositions = new List<Vector2>();
        legalPositions.Add(new Vector2(-1, -1));
        for (int i = 0; i < positions.Length; i++)
        {
            if (this.IsLegalPosition(characterIndex, (Vector3)positions[i]))
            {
                legalPositions.Add(positions[i]);
            }
        }

        // find the closest position to current
        Vector2 currentPosition = this.characters[characterIndex].location;
        Vector2 closestPosition = legalPositions[0];
        float minDistance = (closestPosition - currentPosition).magnitude;
        for (int i = 0; i < legalPositions.Count; i++)
        {
            float distance = (legalPositions[i] - currentPosition).magnitude;
            if (distance < minDistance)
            {
                closestPosition = legalPositions[i];
                minDistance = distance;
            }
        }

        return closestPosition;
    }

    public bool IsClickedLocationInRange(int characterIndex, Vector3 clickedPosition)
    {
        Vector3 location = this.characters[characterIndex].location;
        int x = (int)location.x;
        int y = (int)location.y;

        if (clickedPosition.x == x && clickedPosition.y == y) return true; // clicked on player

        int[] pos = { 0, 1, 2, 1, 0 };
        for (int i = x - 2, k = 0; i <= x + 2; i++, k++)
        {
            if (i < 0 || i > 15) continue;
            for (int j = y - pos[k]; j <= y + pos[k]; j++)
            {

                if (j < 0 || j > 9) continue;
                if (this.IsPlayerAtLocation(new Vector3(i, j, 0))) continue;

                // clicked position is in range
                if (i == clickedPosition.x && j == clickedPosition.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsClickedLocationInEnemyRange(int enemyIndex, Vector3 clickedPosition)
    {
        Vector3 location = this.enemies[enemyIndex].location;
        int x = (int)location.x;
        int y = (int)location.y;

        if (clickedPosition.x == x && clickedPosition.y == y) return true; // clicked on player

        int[] pos = { 0, 1, 2, 1, 0 };
        for (int i = x - 2, k = 0; i <= x + 2; i++, k++)
        {
            if (i < 0 || i > 15) continue;
            for (int j = y - pos[k]; j <= y + pos[k]; j++)
            {

                if (j < 0 || j > 9) continue;
                if (this.IsPlayerAtLocation(new Vector3(i, j, 0))) continue;

                // clicked position is in range
                if (i == clickedPosition.x && j == clickedPosition.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsPlayerAtLocation(Vector3 location)
    {
        // check if the clicked position has an enemy in it (must)
        foreach (Character character in this.characters)
        {
            if (character.location.x == location.x && character.location.y == location.y && character.hp > 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsEnemyAtLocation(Vector3 location)
    {
        // check if the clicked position has an enemy in it (must)
        foreach (Character enemy in this.enemies)
        {
            if (enemy.isActive && enemy.location.x == location.x && enemy.location.y == location.y && enemy.hp > 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsLegalPosition(int characterIndex, Vector3 position)
    {
        bool isClickedLocationInRange = this.IsClickedLocationInRange(characterIndex, position);
        bool isPlayerAtLocation = this.IsPlayerAtLocation(position);
        bool isEnemyAtLocation = this.IsEnemyAtLocation(position);

        if (isPlayerAtLocation)
        { 
            // is allowed to be player
            isPlayerAtLocation =  !(this.characters[characterIndex].location.x == position.x && this.characters[characterIndex].location.y == position.y);
        }

        return isClickedLocationInRange && !isEnemyAtLocation && !isPlayerAtLocation;
    }

    public bool IsLegalEnemyLocation(int enemyIndex, Vector3 position)
    {
        bool isClickedLocationInRange = this.IsClickedLocationInEnemyRange(enemyIndex, position);
        bool isPlayerAtLocation = this.IsPlayerAtLocation(position);
        bool isEnemyAtLocation = this.IsEnemyAtLocation(position);

        if (isEnemyAtLocation)
        {
            // allowed to be enemy
            isEnemyAtLocation = !(this.enemies[enemyIndex].location.x == position.x && this.enemies[enemyIndex].location.y == position.y);
        }

        return isClickedLocationInRange && !isEnemyAtLocation && !isPlayerAtLocation;
    }
}
