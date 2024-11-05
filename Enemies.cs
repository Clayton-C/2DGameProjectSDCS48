using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] private AudioClip enemySoundClip;
    [SerializeField] private AudioClip livesLostSoundClip;
    private int currentNode; //Location of enemy

    public float speed = 1.75f; //enemy movement
    public int damage = 1; // How much life is removed from the castle
    public int health = 1; //enemy health
    public int value = 1; //coins received when destroyed
    public int numberOfEnemies = 1; 
    public Enemies[] spawnedEnemies; //Stronger enemies have more health and can spawn weaker enemies when defeated
    public Vector2 velocity; //current movement of the enemy
    public weaponType[] resitances; //This is needed to give damage resistance types to enemies (i.e. weak to fire, resistant to standard damage)


 
    //Every frame, move towards Next_Node
    private void Update()
    {
        velocity = ((Vector2)transform.position - GameManager.instance.track.getNextPosition(currentNode)).normalized * speed; // Calculate movement direction and velocity
        transform.position = getNextPosition(); // Move the enemy to the next position
        if (Vector2.Distance(transform.position, GameManager.instance.track.getNextPosition(currentNode)) < 0.03f) // Check if the enemy is close to the current target node
        {
            currentNode++;
            if (currentNode >= GameManager.instance.track.Nodes.Length)
            {
                AudioSource.PlayClipAtPoint(livesLostSoundClip, transform.position, 0.8f); //sound played when enemies reach and damage the castle
                Destroy(gameObject);
                GameManager.instance.lives -= damage; //when the enemies reach the castle, subract from lives
                GameManager.instance.enemiesRemaining--;
            }
        }
    }

    // Method to handle enemy's death and spawn weaker enemies if applicable
    private void die()
    {
        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            GameObject enemy = Instantiate(spawnedEnemies[i].gameObject, transform.position, transform.rotation);
            enemy.GetComponent<Enemies>().currentNode = this.currentNode; //when a stronger enemy dies and spawns new enemies, this keeps track of the defeated enemy and spawns them in the same location
        }
        GameManager.instance.enemiesRemaining--;
        GameManager.instance.coins += value; //when enemies die, gain coin value
        AudioSource.PlayClipAtPoint(enemySoundClip, transform.position, 0.8f);
        Destroy(gameObject);
    }

    // Method to apply damage to the enemy; triggers death if health reaches zero
    public void takeDamage(int damage, weaponType[] weaponTypes)
    {
        health -= damage;
        if (health <= 0)
        {
            die();
        }
    }

    // Calculate remaining distance to the end of the path
    public float getDistanceRemaining()
    {
        return GameManager.instance.track.getTotalDistance(currentNode, transform.position);
    }

    private Vector2 getNextPosition()
    {
        Vector2 nextPosition = GameManager.instance.track.getNextPosition(currentNode);
        Vector2 direction = nextPosition - (Vector2)transform.position;
        float distance = direction.magnitude;

        return (Vector2)transform.position + (direction.normalized * speed * Time.deltaTime);
    }
}
