using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool roundActive;

    public static GameManager instance;
    public Enemy_Track track;
    public roundInfo[] rounds;

    public int lives = 100;
    public int coins = 50;
    public int currentRound = 0;
    public int enemiesRemaining = 0;
    

    //spawn enemies
    private IEnumerator spawnEnemies()
    {
        roundInfo round = rounds[currentRound];

        foreach(var data in round.enemies)
        {
            for (int i = 0; i < data.count; i++)
            {
                Enemies enemy = Instantiate(data.enemy, track.getNextPosition(0), Quaternion.identity);
                enemiesRemaining+= enemy.numberOfEnemies;
                yield return new WaitForSeconds(data.spawnRate);
            }
            yield return new WaitForSeconds(round.timeBetweenEnemies);
        }
        coins += round.endOfRoundCoins;
        roundActive = false;
    }

    //function needed for nextRound button
    public void nextRound()
    {
        if(roundActive || enemiesRemaining > 0)
        {
            return;
        }
        if (lives <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
        if (currentRound >= 10 && enemiesRemaining <= 0)
        {
            SceneManager.LoadScene("WinScene");
        }
        //Start the next round
        roundActive = true;
        currentRound++;
        StartCoroutine(spawnEnemies());
    }
    private void Awake()
    {
        instance = this;
    }
}

//data for the round to track when the enemies have finished spawning/been defeated and a new round can be started
[System.Serializable]
public class roundInfo
{
    public EnemiesInfo[] enemies;
    public float timeBetweenEnemies = 0.5f;
    public int endOfRoundCoins = 100;

}
//data for spawning specific enemies for each round
[System.Serializable]
public class EnemiesInfo
{
    public Enemies enemy;
    public int count = 1;
    public float spawnRate = 0.5f;
}