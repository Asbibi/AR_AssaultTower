using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Null,
        Setup,
        Preparation,
        Fight
    };

    private static GameManager instance = null;

    [Header("Interface")]
    [SerializeField] private UIManager UI;
    [SerializeField] private GameObject proximityLinePrefab;

    [Header("Enemies")]
    [SerializeField] private EnemyPack[] possibleEnemies;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Virtual Heroes")]
    [SerializeField] private UnitData[] virtualUnitDatas;
    [SerializeField] private GameObject heroVirtualPrefab;
    [SerializeField] private int enemyCountToGetAVirtualHero = 5;
    [SerializeField] private LayerMask virtualLayer;

    //[Header("State (Debug)")]
    private GameState gameState = GameState.Setup;
    private bool fighting = false;
    private int maxFloor = 0;   // <=0 -> infinity
    private int currentFloor = 0;
    private int virtualHumanInReserve = 0;
    private int enemyCount = 0;

    private List<Hero> heroes       = new List<Hero>();
    private List<Hero> tempHeroes   = new List<Hero>();  // temporary list for when the user wants to add a hero marker outside of preparation phase (fight or setup)
    private List<Enemy> enemies     = new List<Enemy>();
    private List<Door> doors        = new List<Door>();
    private List<Door> tempDoors    = new List<Door>();   // temporary list for when the user wants to add a door marker while fighting


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("GameManagerInstance Already Exists !");
            Destroy(gameObject);
            return;
        }

        instance = this;
        UI.UpdateUI();
    }
    void Update()
    {
        switch (gameState)
        {
            case GameState.Setup:
                {
                    break;
                }
            case GameState.Preparation:
                {
                    _UpdatePreparationPhase();
                    break;
                }
            case GameState.Fight:
                {
                    if (fighting)
                        break;

                    _UpdateFightPhase();
                    break;
                }
        }
    }
    private void _GameOver()
    {
        SceneManager.LoadScene(2);
    }

    static public GameState GetState()
    {
        if (instance == null)
            return GameState.Null;

        return instance.gameState;
    }
    static public GameObject GetEnemyPrefab()
    {
        if (instance == null)
            return null;
        else
            return instance.enemyPrefab;
    }
    static public GameObject GetProximityLinePrefab()
    {
        if (instance == null)
            return null;
        else
            return instance.proximityLinePrefab;
    }
    static public void IncreaseMaxFloor()
    {
        if (instance != null)
            instance.maxFloor++;
    }
    static public void DecreaseMaxFloor()
    {
        if (instance != null && instance.maxFloor > 0)
        {
            instance.maxFloor--;
        }
    }
    static public int GetCurrentFloor()
    {
        if (instance == null)
            return -1;

        return instance.currentFloor;
    }
    static public int GetMaxFloor()
    {
        if (instance == null)
            return -1;

        return instance.maxFloor;
    }
    static public int GetVirtualReserveCount()
    {
        if (instance == null)
            return 0;

        return instance.virtualHumanInReserve;
    }




    public static void RegisterDoor(Door door)
    {
        if (instance != null)
            instance._RegisterDoor(door);
    }
    private void _RegisterDoor(Door door)
    {
        if (gameState == GameState.Fight)
        {
            tempDoors.Add(door);
            door.gameObject.SetActive(false);
        }
        else
            doors.Add(door);
    }
    public static void UnRegisterDoor(Door door)
    {
        if (instance != null)
            instance._UnRegisterDoor(door);
    }
    private void _UnRegisterDoor(Door door)
    {
        doors.Remove(door);
        tempDoors.Remove(door);
    }

    public static void RegisterHero(Hero hero)
    {
        if (instance != null)
            instance._RegisterHero(hero);
    }
    private void _RegisterHero(Hero hero)
    {
        if (gameState == GameState.Preparation)
            heroes.Add(hero);
        else
        {
            tempHeroes.Add(hero);
            hero.gameObject.SetActive(false);
        }
    }
    public static void UnRegisterHero(Hero hero)
    {
        if (instance != null)
            instance._UnRegisterHero(hero);
    }
    private void _UnRegisterHero(Hero hero)
    {
        heroes.Remove(hero);
        tempHeroes.Remove(hero);
    }

    public static void RegisterEnemy(Enemy enemy)
    {
        if (instance != null)
            instance._RegisterEnemy(enemy);
    }
    private void _RegisterEnemy(Enemy enemy)
    {
        if (gameState != GameState.Preparation)
            return;

        enemies.Add(enemy);
    }







    // ======================= SETUP PHASE PROCESSUS ==============================

    static public bool TryEndSetUpPhase()
    {
        if (instance != null)
            return instance._FinalizeSetUp();
        return false;
    }
    private bool _FinalizeSetUp()
    {
        if (doors.Count == 0)
        {
            Debug.Log("No door marker detected");
            return false;
        }
        _StartPreparationPhase();
        return true;
    }



    // ======================= PREPA PHASE PROCESSUS ==============================

    private void _StartPreparationPhase()
    {
        foreach (var door in tempDoors)
        {
            door.gameObject.SetActive(true);
            doors.Add(door);
        }
        tempDoors.Clear();

        foreach (var hero in tempHeroes)
        {
            hero.Active();
            heroes.Add(hero);
        }
        tempHeroes.Clear();

        gameState = GameState.Preparation;
        UI.UpdateUI();
    }
    private void _UpdatePreparationPhase()
    {
        foreach (var hero in heroes)
        {
            hero.ComputeProximityScore(heroes);
        }
    }
    public static void SpawnVirtualHero()      // return if it wont be possible to add another virtual hero after
    {
        if (instance == null)
            return;
        instance._SpawnVirtualHero();
    }
    private void _SpawnVirtualHero()      // return if it wont be possible to add another virtual hero after
    {
        if (virtualHumanInReserve <= 0 || gameState != GameState.Preparation)
            return;

        Vector3 spawnPosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
        float enter = 0.0f;
        if ((new Plane(Vector3.up, Vector3.zero)).Raycast(ray, out enter))
            spawnPosition = ray.GetPoint(enter);        
        GameObject heroVirtual = Instantiate(heroVirtualPrefab, spawnPosition, Quaternion.identity);
        heroVirtual.GetComponent<Hero>().Setup(virtualUnitDatas[Random.Range(0, virtualUnitDatas.Length)]);

        virtualHumanInReserve--;
    }




    // ======================= FIGHT PHASE PROCESSUS ==============================

    public static int StartFightPhase()
    {
        if (instance == null)
            return -1;

        return instance._StartFightPhase();
    }
    private int _StartFightPhase()
    {
        if (doors.Count == 0)
            return 1;
        else if (heroes.Count == 0)
            return 2;

        List<int> possiblePacks = new List<int>();
        for(int i = 0; i < possibleEnemies.Length; i++)
        {
            if (!possibleEnemies[i].OkForThisFloor(currentFloor))
                continue;

            for (int j = 0; j < possibleEnemies[i].probabilityWeight; j++)
                possiblePacks.Add(i);
        }

        if (possiblePacks.Count > 0)
        {
            foreach (var door in doors)
            {
                possibleEnemies[possiblePacks[Random.Range(0, possiblePacks.Count)]].SpawnEnemies(door.transform.position);
            }
        }

        gameState = GameState.Fight;
        UI.UpdateUI();
        return 0;
    }
    private void _UpdateFightPhase()
    {
        StartCoroutine(_UpdateFightPhase_Coroutine());
    }
    private IEnumerator _UpdateFightPhase_Coroutine()
    {
        fighting = true;
        _TryEndFightPhase();


        // ===========      Heroes attack      ==============

        foreach (var hero in heroes)
        {
            // Hero still alive now ?
            if (hero.GetLife() < 0 || enemies.Count == 0)
                continue;

            // Find closest enemy -> is him in range ?
            Enemy foundEnemy = enemies[hero.GetClosestUnit(enemies.ToArray())];
            if (!hero.IsInRange(foundEnemy))
                continue;

            // Attack this enemy
            float damage = hero.Attack();
            Vector3 toTarget = (foundEnemy.transform.position - hero.transform.position).normalized;
            hero.transform.forward = toTarget;
            foundEnemy.transform.forward = -toTarget;

            yield return new WaitForSeconds(0.5f);

            bool died = foundEnemy.Hurt(damage);
                // deal side damage to other heroes/enemies if range angle isn't forward
            yield return new WaitForSeconds(1.2f);

            if (died)
            {
                enemies.Remove(foundEnemy);
                Destroy(foundEnemy.gameObject);
                enemyCount++;
            }
        }


        // ===========      Enemies attack      ==============

        foreach (var enemy in enemies)
        {
            if (heroes.Count == 0)  // no left hero to fight => player has loose => skip
                break;



            // Find closest hero
            Hero foundHero = heroes[enemy.GetClosestUnit(heroes.ToArray())];

            // turn toward the hero
            Vector3 toTarget = (foundHero.transform.position - enemy.transform.position).normalized;
            enemy.transform.forward = toTarget;

            // Is he in range ? if not, teleport to him
            if (!enemy.IsInRange(foundHero))
            {                
                Vector3 direction = toTarget * 0.005f;
                // Here the 0.1f is a relatively good step considering the Near/Close/far values but might need to be changed

                enemy.Move();
                yield return new WaitForSeconds(0.5f);
                while (!enemy.IsInRange(foundHero))
                {
                    enemy.transform.position += direction;
                }
                yield return new WaitForSeconds(0.4f);
            }

            // ---- Attack Hero -----
            float damage = enemy.Attack();
            foundHero.transform.forward = -toTarget;

            yield return new WaitForSeconds(0.8f);

            bool died = false;

            if (enemy.IsInRange(foundHero)) // if the player moved the hero out of range -> no damages
                died = foundHero.Hurt(damage);

                // deal side damage to other heroes/enemies if range angle isn't forward
            yield return new WaitForSeconds(0.8f);

            if (died)
            {
                heroes.Remove(foundHero);
                ProximityLine.Disconnect(foundHero, heroes);
                if (virtualLayer == (virtualLayer | (1 << foundHero.gameObject.layer)))
                {
                    Debug.Log("Virtual Detected");
                    Destroy(foundHero.gameObject);
                }
            }
            // ----------------------
        }



        fighting = false;
    }
    private void _TryEndFightPhase()
    {
        if (heroes.Count == 0)
        {
            // Game is over : all heroes on the terrain died => player loosed
            _GameOver();
            return;
        }
        else if (enemies.Count == 0)
        {
            // All enemies died : if it was the last floor, game is over, player won | else start next floor
            if (maxFloor > 0 && currentFloor > maxFloor-2)
                // last floor reached
                _GameOver();
            
            else
            {
                // start next floor
                currentFloor++;
                if (enemyCount >= enemyCountToGetAVirtualHero)
                {
                    enemyCount = 0;
                    virtualHumanInReserve++;
                }
                foreach (var hero in heroes)
                    hero.Celebrate();

                _StartPreparationPhase();
            }
        }
    }
}
