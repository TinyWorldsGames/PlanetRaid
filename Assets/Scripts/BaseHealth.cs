using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 100;

    [SerializeField] TMP_Text healthText;

    [SerializeField]
    GameObject gameOverPanel;

    private void Start()
    {
        healthText.text = health.ToString();
    }


    public void Die()
    {

        gameOverPanel.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        healthText.DOColor(Color.red, 0.5f).OnComplete(() => healthText.DOColor(Color.white, 0.5f));
        healthText.text = health.ToString();
    }
}
