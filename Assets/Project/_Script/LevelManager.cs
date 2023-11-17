using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// [Serializable]
// public class DicSO
// {
//     [SerializeField] public GameConfig.SO_TYPE Type;
//     [SerializeField] public List<ScriptableObject> Stats;
// }

public enum GameMode
{
    Sweep,
    Survival,
    SearchAndDestroy
}

public class LevelManager : MonoBehaviour
{
    #region Fields & Properties
    public GameMode currentGameMode;
    public GameMode[] availableGameMode = { GameMode.Sweep, GameMode.Survival };
    public GameObject characterSpawner;

    //[SerializeField]
    //public DataBank DataBank;

    private static LevelManager instance;
    public static LevelManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    //public CharacterSO characterInfo;
    [SerializeField] protected Character character;

    [SerializeField] protected List<Enemy> enemies;

    [SerializeField] CameraController myCamera;

    protected float possibleEnemyCount, enemiesLeft;
    #endregion

    #region Methods
    private void OnDrawGizmos()
    {
        if (characterSpawner == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(characterSpawner.transform.position, 0.5f);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DataBank = GetComponent<DataBank>();
    }

    void Start()
    {
        character = GameObject.FindObjectOfType<Character>();
        if (character == null)
            switch (GameManager.Instance.SelectedCharacter)
            {
                case GameConfig.CHARACTER.CHARACTER_1:
                    character = Character1.Create(null, characterSpawner.transform.position);
                    break;

                case GameConfig.CHARACTER.CHARACTER_2:
                    character = Character2.Create(null, characterSpawner.transform.position);
                    break;

                case GameConfig.CHARACTER.CHARACTER_3:
                    character = Character3.Create(null, characterSpawner.transform.position);
                    break;

                case GameConfig.CHARACTER.CHARACTER_4:
                    character = Character4.Create(null, characterSpawner.transform.position);
                    break;
            }
        //CharacterSO c = GameManager.Instance.selectedCharacter;
        //GameObject charactergameobject = Instantiate(c.characterPrefab, characterSpawner.transform.position, characterSpawner.transform.rotation);
        //character = charactergameobject.GetComponent<Character>();

        enemies.AddRange(GameObject.FindObjectsOfType<Enemy>());
        myCamera = Camera.main.gameObject.GetComponent<CameraController>();

        character.Initialize();
        myCamera.Initialize(character.gameObject);
        foreach (var enemy in enemies)
        {
            enemy.Initialize();
        }

        possibleEnemyCount = enemies.Count;
        foreach (EnemySpawner es in GameObject.FindObjectsOfType<EnemySpawner>())
        {
            possibleEnemyCount += es.enemySpawnLimit;
        }
        enemiesLeft = possibleEnemyCount;
    }

    private void FixedUpdate()
    {
        RemoveDeathEnemy();

        if (!character.IsDeath)
            character.UpdateCharacter(enemies);
        myCamera.UpdateCamera();
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDead)
                enemy.UpdateEnemy(character);
        }
        if (WinCondition())
        {
            character.SetScreenText("You Win!");
        } else if (LoseCondition())
        {
            character.SetScreenText("You Lose!");
        }
    }

    private void LateUpdate()
    {

    }

    //public SO_EnemyDefault GetStats(Enemy aEnemy)
    //{
    //    //Debug.Log($"Type: {type}, Index {index}");
    //    return DataBank.EnemyStats.Find(element => element.enemy.GetType() == aEnemy.GetType()).Stats;
    //}

    private void RemoveDeathEnemy()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].IsDead)
            {
                if (enemies[i].deleteUponDeath)
                {
                    enemies[i].StopAllCoroutines();
                    Destroy(enemies[i].gameObject);
                }
                enemies.RemoveAt(i);
                enemiesLeft -= 1;
            }
        }
    }

    public void AddEnemy(Enemy e)
    {
        enemies.Add(e);
    }

    virtual public bool WinCondition()
    {
        switch (currentGameMode)
        {
            default:
                {
                    if (enemies.Count == 0)
                        return true; else
                        return false;
                }
        }
    }

    virtual public bool LoseCondition()
    {
        switch (currentGameMode)
        {
            default:
                {
                    return character.IsDeath;
                }
        }
    }
    #endregion
}
