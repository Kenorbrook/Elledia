using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", fileName = "Standart Enemy")]
public class Enemy : ScriptableObject
{
    [Tooltip("��� �����")]
    public EnemyEnum.Enemy TypeEnemy;
    [Tooltip("�������� �����")]
    public string EnemyName;
    [Tooltip("����� �����")]
    public int hp;
    [Tooltip("����")]
    public int damage;
    [Tooltip("�������� ������")]
    public Sprite EnemySprite;
}
