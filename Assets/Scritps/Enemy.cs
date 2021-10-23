using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", fileName = "Standart Enemy")]
public class Enemy : ScriptableObject
{
    [Tooltip("Тип врага")]
    public EnemyEnum.Enemy TypeEnemy;
    [Tooltip("Название врага")]
    public string EnemyName;
    [Tooltip("Жизни врага")]
    public int hp;
    [Tooltip("Урон")]
    public int damage;
    [Tooltip("Основной спрайт")]
    public Sprite EnemySprite;
}
