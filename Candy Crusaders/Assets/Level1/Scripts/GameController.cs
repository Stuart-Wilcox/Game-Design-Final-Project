using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    // starting positions of both players
    public Vector2[] playerStartingPositions;
    public Vector2[] enemyStartingPositions;

    // stats for each of the players
    public Character[] userPlayers;
    public Character[] enemyPlayers;

    // transforms of the players
    public Transform[] users;
    public Transform[] enemies;

    // misc needed transforms
    public Grid grid;
    public Transform green;
    public Transform red;
    public Transform grey;

    private AudioSource attackSource;
    private AudioSource deathSoundSource;

    // value to increment player currency by each time an enemy dies
    public int currencyIncrement;

    public PlayerData playerData;

    // text to display on win
    public Text winText;

    // the scene number of this scene
    public int sceneBuildIndex;

    // enemy controller to get the enemy's moves
    private List<EnemyController> enemyControllers;

    // service classes to perform work
    private MoveService moveService;
    private AttackService attackService;

    // watch for gameOver
    private bool isGameOver;

    // watch for who's turn it is
    private bool isPlayersTurn;
    private List<bool> userPlayerHasMoved;
    private List<Transform> greyTiles; // hold refs to the grey tiles that get put after moving

    public void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;

        // game is on
        isGameOver = false;

        // player goes first
        isPlayersTurn = true;
        greyTiles = new List<Transform>();
        userPlayerHasMoved = new List<bool>();
        for(int i = 0; i < userPlayers.Length; i++)
        {
            userPlayerHasMoved.Add(false);
        }

        for(int i = 0; i < userPlayers.Length; i++)
        {
            // reset hp at start of game
            userPlayers[i].hp = userPlayers[i].maxHp;
            // move enemy to chosen position
            userPlayers[i].location = playerStartingPositions[i];
            // set the player to active
            userPlayers[i].isActive = true;
        }

        for (int i = 0; i < enemyPlayers.Length; i++){
            // reset hp at start of game
            enemyPlayers[i].hp = enemyPlayers[i].maxHp;
            // move enemy to chosen position
            enemyPlayers[i].location = enemyStartingPositions[i];
            // set the enemy to active
            enemyPlayers[i].isActive = true;
        }

        var audioComponents = GetComponents<AudioSource>();
        attackSource = audioComponents[0];
        deathSoundSource = audioComponents[1];

        // instantiate services
        moveService = new MoveService(userPlayers, enemyPlayers, green, red);
        attackService = new AttackService(userPlayers, enemyPlayers, attackSource);

        // get reference to controller
        enemyControllers = new List<EnemyController>();
        foreach(Transform enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetMoveService(moveService);
            enemyControllers.Add(enemyController);
        }
    }

    public void Update()
    {
        // dont do anything if the game is over
        if (isGameOver)
        {
            return;
        }

        // check touch input, if there is any
        if (/* Input.touchCount > 0 */ Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition /* Input.GetTouch(0).position */);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3 position = grid.WorldToCell(worldPoint) + new Vector3(8, 5, 0);

            if (isPlayersTurn)
            {
                TakePlayerTurn(position);

                CheckGameHasEnded();

                // check if the player turn is over
                isPlayersTurn = !CheckUserTurnOver();

                UpdateUserPlayersHaveMoved(); // grey out tiles of people who have moved

                RemovePlayersKilled();
            }

            if (!isPlayersTurn)
            {
                // enemy takes turn directly after user
                for (int i = 0; i < enemyControllers.Count; i++)
                {
                    TakeEnemyTurn(i);
                }

                isPlayersTurn = true; // enemy turn is now over
                for (int i = 0; i < userPlayers.Length; i++) {
                    userPlayerHasMoved[i] = false; // reset the list of pieces that moved
                }
                UpdateUserPlayersHaveMoved(); // ungrey tiles of people who havent moved

                RemovePlayersKilled();

                CheckGameHasEnded();
            }
        }
    }

    public void EndTurn()
    {
        if (isGameOver) return;

        // player clicked end turn button
        for (int i = 0; i < enemyControllers.Count; i++)
        {
            TakeEnemyTurn(i);
        }
        isPlayersTurn = true; // enemy turn is now over
        for (int i = 0; i < userPlayers.Length; i++)
        {
            userPlayerHasMoved[i] = false; // reset the list of pieces that moved
        }
        UpdateUserPlayersHaveMoved(); // ungrey tiles of people who havent moved
    }

    public void RestartLevel()
    {
        if (isGameOver) return;

        SceneManager.LoadScene(sceneBuildIndex);
    }

    private bool TakePlayerTurn(Vector3 position)
    {
        if (moveService.IsPlayerAtLocation(position))
        {
            // click position was on player, so show/unshow the tiles
            int characterIndex = FindPlayerByPosition(position);

            if (userPlayerHasMoved[characterIndex] || userPlayers[characterIndex].hp < 1) return false; // dont do anything if the player has already gone this turn, or is dead

            moveService.PlayerClicked(characterIndex);
        }
        else if (moveService.IsPlayerHighlighted() && moveService.IsMovePositionClicked(moveService.GetPlayerHighlightedIndex(), position))
        {
            // move the player to the selected location
            int characterIndex = moveService.GetPlayerHighlightedIndex();

            if (userPlayerHasMoved[characterIndex]) return false; // dont do anything if the player has already gone this turn

            userPlayerHasMoved[characterIndex] = true;
            moveService.MovePlayer(characterIndex, position);
        }
        else if (moveService.IsPlayerHighlighted() && moveService.IsAttackPositionClicked(moveService.GetPlayerHighlightedIndex(), position))
        {
            int characterIndex = moveService.GetPlayerHighlightedIndex();

            if (userPlayerHasMoved[characterIndex]) return false; // dont do anything if the player has already gone this turn

            int enemyIndex = FindEnemyByPosition(position);

            if(enemyIndex < 0)
            {
                Debug.Log("Could not find enemy in location...");
                return false;
            }

            Vector2 currentPosition = userPlayers[characterIndex].location;
            Vector2 attackPosition = enemyPlayers[enemyIndex].location;
            Vector2 beside = moveService.CalculatePlayerAttackPosition(characterIndex, enemyIndex);

            if(beside.x == -1 && beside.y == -1)
            {
                Debug.Log("Cannot find legal spot to move");
                return false;
            }

            userPlayerHasMoved[characterIndex] = true;
            moveService.MovePlayer(characterIndex, beside); // move to beside the enemy
            attackService.PlayerAttack(characterIndex, enemyIndex); // perform the attack
        }

        return false;
    }

    private void TakeEnemyTurn(int enemyIndex)
    {
        // let the enemy decide what they want to do this turn, then perform the actions on their behalf
        Turn turn = enemyControllers[enemyIndex].TakeTurn();
        if (turn.isMove)
        {
            // move if needed
            moveService.MoveEnemy(enemyIndex, turn.movePosition);
        }
        else if (turn.isAttack)
        {
            int characterIndex = FindPlayerByPosition(turn.attackPosition); // figure out which player to attack
            // just to be safe...
            if (characterIndex < 0)
            {
                Debug.Log("Player not found! Unable to attack...");
                return;
            }
            
            Vector2 beside = moveService.CalculateEnemyAttackPosition(characterIndex, enemyIndex);

            if(beside.x == -1 && beside.y == -1)
            {
                Debug.Log("Cannot find legal spot to move");
                return;
            }

            moveService.MoveEnemy(enemyIndex, beside); // move the enemy beside the player
            attackService.EnemyAttack(characterIndex, enemyIndex); // perform the attack
        }
    }

    private void UpdateUserPlayersHaveMoved()
    {
        // get rid of all the grey tiles
        foreach(Transform greyTile in greyTiles)
        {
            Destroy(greyTile.gameObject);
        }

        greyTiles = new List<Transform>(); // reset list

        // add grey tiles as needed
        for(int i = 0; i < userPlayerHasMoved.Count; i++)
        {
            if (userPlayerHasMoved[i])
            {
                Transform greyTile = (Transform)Instantiate(grey, (Vector3)userPlayers[i].location, Quaternion.Euler(0, 0, 0));
                greyTiles.Add(greyTile);
            }
        }
    }

    private void RemovePlayersKilled()
    {
        for(int i = 0; i < userPlayers.Length; i++)
        {
            if(userPlayers[i].hp < 1 && users[i] != null)
            {
                // remove the dead from the scene
                Destroy(users[i].gameObject);
                userPlayers[i].isActive = false;
                deathSoundSource.Play();
            }
        }

        for(int i = 0; i < enemyPlayers.Length; i++)
        {
            if(enemyPlayers[i].hp < 1 && enemies[i] != null)
            {
                // remove the dead from the scene
                Destroy(enemies[i].gameObject);
                enemyPlayers[i].isActive = false;
                deathSoundSource.Play();
                playerData.currency += currencyIncrement;
            }
        }
    }

    private int FindEnemyByPosition(Vector3 position)
    {
        for(int i = 0;  i < enemyPlayers.Length; i++)
        {
            Character enemy = enemyPlayers[i];
            if(enemy.location.x == position.x && enemy.location.y == position.y)
            {
                return i;
            }
        }

        return -1;
    }

    private int FindPlayerByPosition(Vector3 position)
    {
        for (int i = 0; i < userPlayers.Length; i++)
        {
            Character player = userPlayers[i];
            if (player.location.x == position.x && player.location.y == position.y)
            {
                return i;
            }
        }

        return -1;
    }

    private bool CheckUserTurnOver()
    {
        for(int i = 0; i < userPlayerHasMoved.Count; i++)
        {
            bool moved = userPlayerHasMoved[i];
            bool isDead = userPlayers[i].hp < 1;
            if (!isDead && !moved)
            {
                return false;
            }
        }

        return true;
    }

    private void CheckGameHasEnded()
    {
        string winner = ""; // use for win text
        if (CheckEnemyHasWon()) {
            winner = "Enemy";
        }

        if(CheckUserHasWon())
        {
            winner = "User";
        }

        if(!winner.Equals(""))
        {
            // pause the game and display win text
            isGameOver = true;
            this.winText.text = winner + " Wins!";

            // the level is now over, so handle returning to overworld here and saving level state
            StartCoroutine(this.LoadLevelSelectorScene());
        }
    }

    private bool CheckUserHasWon()
    {
        foreach(Character enemy in enemyPlayers)
        {
            if(enemy.hp > 1)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckEnemyHasWon()
    {
        foreach (Character player in userPlayers)
        {
            if (player.hp > 1)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator LoadLevelSelectorScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
