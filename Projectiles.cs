using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private AudioClip projectileSoundClip;
    private int pierce; //number of enemies the projectile can pass through

    public Tower tower;
    public float lifeTime = 5f;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
        pierce = tower.pierce;
    }

    private void Update()
    {
        //projectile speed based on speed attribute
        transform.position += transform.up *tower.speed * Time.deltaTime;
    }

    //this function applies the damage to the enemy and removes the projectile from the game
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemies e = collision.GetComponent<Enemies>();
        if (e != null)
        {
            if (pierce <= 0)
            {
                Destroy(gameObject);
                return;
            }
            pierce--;
            e.takeDamage(tower.damage, tower.weaponTypes);

            //play sound when towers shoot
            AudioSource.PlayClipAtPoint(projectileSoundClip, transform.position, 0.8f);

            if (tower.explosive)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
                foreach (Collider2D col in colliders)
                {
                    //apllies damage to each enemy within the explosion radius
                    Enemies enemy = col.gameObject.GetComponent<Enemies>();
                    if (enemy != null)
                    {
                        enemy.takeDamage(tower.damage, tower.weaponTypes);
                    }
                }
            }
            if (pierce <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

}

//add more types if there is time
//TODO: Fire, Ice, Poison
public enum weaponType
{
    Standard,
    Explosive,
    Fire,
    Ice,
    Poison
}