using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private Tower selectedTower;

    public static UIManager instance;

    public GameObject towerMenu;
    public GameObject upgradeMenu;

    public TMP_Text coinsText;
    public TMP_Text upgradeMenuCoinsText;
    public TMP_Text livesText;
    public TMP_Text roundsText;
    public TMP_Text towerNameTxt;
    public TMP_Text upgradeDamageCostText;
    public TMP_Text upgradeRangeCostText;
    public TMP_Text upgradeFireRateCostText;
    public TMP_Text sellTowerCostText;
    public TMP_Text archerCostText;

    private void Update()
    {
        updateUIText();
    }
    private void Awake()
    {
        instance = this;
    }

    public void selectTower(Tower t)
    {
        selectedTower = t;
        towerMenu.SetActive(false);
        upgradeMenu.SetActive(true);
    }

    public void deselectTower()
    {
        towerMenu.SetActive(true);
        upgradeMenu.SetActive(false);
    }

    public void sellSelectedTower()
    {
        GameManager.instance.coins += selectedTower.cost / 2;
        Destroy(selectedTower.gameObject);
        deselectTower();
    }

    public void updateUIText()
    {
        livesText.text = "Lives: " + GameManager.instance.lives;
        coinsText.text = "Coins: " + GameManager.instance.coins;
        roundsText.text = "Round: " + GameManager.instance.currentRound;
        if (upgradeMenu.activeInHierarchy)
        {
            towerNameTxt.text = selectedTower.name;
            upgradeMenuCoinsText.text = "Coins: " + GameManager.instance.coins;
            upgradeDamageCostText.text = "Cost: " + selectedTower.upgradeCostDamage;
            upgradeRangeCostText.text = "Cost: " + selectedTower.upgradeCostRange;
            upgradeFireRateCostText.text = "Cost: " + selectedTower.upgradeCostFireRate;
            sellTowerCostText.text = "Sell: " + selectedTower.sellCost;
        }
    }

    public void upgradeSelectedTowerDamage()
    {
        if(selectedTower != null)
        {
            selectedTower.UpgradeDamage();
        }
    }

    public void upgradeSelectedTowerRange()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeRange();
        }
    }

    public void upgradeSelectedTowerFireRate()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeFireRate();
        }
    }
}
