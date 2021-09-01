using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int totalHealth = 3;

    private int _health;

    private SpriteRenderer _spriteRenderer;

    private bool _coolDown;

    public RectTransform _rectHeart;

    public float heartSize = 99;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _health = totalHealth;
        _rectHeart.sizeDelta = new Vector2(heartSize * _health, 64);
    }

    public void AddDamage(int amount)
    {
        if (_coolDown)
        {
            return;
        }

        _coolDown = true;
        
        _health = _health - amount;

        StartCoroutine("VisualFeedback");

        if (_health <= 0)
        {
            _health = 0;
            Destroy(gameObject);
        }

        _rectHeart.sizeDelta = new Vector2(heartSize * _health, 64);

        Debug.Log("Player got damage. His current health is: " + _health);
    }

    public void AddHealth(int amount)
    {
        _health = _health + amount;

        if (_health >= totalHealth)
        {
            _health = totalHealth;
        }
        
        _rectHeart.sizeDelta = new Vector2(heartSize * _health, 64);
        
        Debug.Log("Player got some life. His current health is: " + _health);
    }

    private IEnumerator VisualFeedback()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0);

        yield return new WaitForSeconds(.05f);
        
        _spriteRenderer.color = Color.white;
        
        yield return new WaitForSeconds(1f);
        
        _coolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
