using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Singleton
    private static EnemyManager instance = null;
    public static EnemyManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance EnemyManager found!");
            DestroyImmediate(this);
            return;
        }

        instance = this;
    }
    #endregion

    PlayerManager playerManager;

    public List<Enemy> enemys = new List<Enemy>();

    void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    void Update()
    {
        TargetPlayer();
    }

    private void TargetPlayer()
    {
        if (playerManager.player == null)
            return;

        foreach (Enemy enemy in enemys)
        {
            enemy.TargetPlayer(playerManager.player.transform);
        }
    }

    public void AddEnemy(Enemy newEnemy)
    {
        enemys.Add(newEnemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemys.Remove(enemy);
    }
}
