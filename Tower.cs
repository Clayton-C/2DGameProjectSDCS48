using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private AudioClip upgradeSoundClip;
    private float nextAttack;

    public GameObject radiusDisplay;
    public Projectiles projectile;
    public GameObject mesh;

    public string name;
    public int cost = 1;
    public int sellCost = 1;
    public int upgradeCostDamage = 100;
    public int upgradeCostRange = 100;
    public int upgradeCostFireRate = 100;
    public float radius = 1;
    public float fireRate = 1;
    public int damage = 1;
    public float speed = 11f; //affects speed of projectile
    public int pierce = 1; //Number of enemies projectile can pass through
    public bool explosive = false;
    public weaponType[] weaponTypes;

    private void Update()
    {
        if (Time.time > nextAttack)
        {
            Enemies enemy = getEndEnemy();
            if (enemy != null)
            {
                nextAttack = Time.time + fireRate;
                attack(enemy);
            }
        }
    }

    private void attack(Enemies currentEnemy)
    {
        float distance = Vector2.Distance(transform.position, currentEnemy.transform.position);
        Vector2 projectilePrediction = (Vector2)currentEnemy.transform.position - ((currentEnemy.velocity * distance / speed) * 2);
        transform.up = projectilePrediction - (Vector2)transform.position;
        Projectiles proj = Instantiate(projectile, transform.position, transform.rotation);
        proj.tower = this;
    }
    private Enemies[] getAllEnemiesInRange()
    {
        Collider2D[] colider = Physics2D.OverlapCircleAll(transform.position, radius);
        Enemies[] enemy = new Enemies[colider.Length];
        if (colider.Length == 0)
        {
            return enemy;
        }
        int i = 0;
        foreach (var a in colider)
        {
            Enemies e = a.gameObject.GetComponent<Enemies>();
            if (e != null)
            {
                enemy[i] = e;
            }
            i++;
        }
        return enemy;
    }
    private Enemies getEndEnemy()
    {
        Enemies[] towers = getAllEnemiesInRange();
        Enemies endEnemy = null;
        float distance = float.MaxValue;
        foreach(Enemies e in towers)
        {
            if (e == null)
            {
                continue;
            }

            float dist = e.getDistanceRemaining();
            if (dist < distance)
            {
                distance = dist;
                endEnemy = e;
            }
        }
        return endEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void selected(bool s)
    {
        radiusDisplay.SetActive(s);
        radiusDisplay.transform.localScale = new Vector2(radius * 2, radius * 2);
    }

    public void UpgradeDamage()
    {
        if (GameManager.instance.coins >= upgradeCostDamage && damage <= 4)
        {
            GameManager.instance.coins -= upgradeCostDamage;
            sellCost += (upgradeCostDamage / 2);
            damage++;
            upgradeCostDamage = Mathf.RoundToInt(upgradeCostDamage * 2.5f);
            AudioSource.PlayClipAtPoint(upgradeSoundClip, transform.position, 1f);
        }
    }

    public void UpgradeRange()
    {
        if(GameManager.instance.coins >= upgradeCostRange)
        {
            GameManager.instance.coins -= upgradeCostRange;
            sellCost += (upgradeCostRange / 2);
            radius+= 0.25f;
            upgradeCostRange = Mathf.RoundToInt(upgradeCostRange * 1.5f);
            AudioSource.PlayClipAtPoint(upgradeSoundClip, transform.position, 1f);
        }
    }

    public void UpgradeFireRate()
    {
        if (GameManager.instance.coins >= upgradeCostFireRate && fireRate > 0)
        {
            GameManager.instance.coins -= upgradeCostFireRate;
            sellCost += (upgradeCostFireRate / 2);
            fireRate -= 0.1f;
            upgradeCostFireRate = Mathf.RoundToInt(upgradeCostFireRate * 1.5f);
            AudioSource.PlayClipAtPoint(upgradeSoundClip, transform.position, 1f);
        }
    }
}
