using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner instance;
    public List<GameObject> obstPrefab;
    public List<int> spawnPoint;
    public List<int> spawnChance;
    
    public Dictionary<int, GameObject> obstPrefabs;

    public List<GameObject> obstaclesPool;

    private List<GameObject> spawnedObstacles;

    private System.Random rnd;
    float width;

    private void Awake()
    {

        obstPrefabs = new Dictionary<int, GameObject>();
        obstaclesPool = new List<GameObject>();

        obstPrefabs.Add(spawnPoint[0], obstPrefab[0]);
        obstPrefabs.Add(spawnPoint[1], obstPrefab[1]);
        obstPrefabs.Add(spawnPoint[2], obstPrefab[2]);
        obstPrefabs.Add(spawnPoint[3], obstPrefab[3]);

    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        spawnedObstacles = new List<GameObject>();
        //width = GetComponent<BoxCollider2D>().size.x;
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        //print(width + "/" + GetComponent<BoxCollider2D>().size.x);
        

        rnd = new System.Random();
    }


    public void Launch()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            var obstacleToSpawn = TakeObstacle();

            var obs = Instantiate(obstacleToSpawn, new Vector3(Random.Range(-width / 2, width / 2), transform.position.y, 0), Quaternion.identity);

            obs.GetComponent<Rigidbody2D>().AddForce(Vector2.down * PlaygroundManager.instance.speed);

            spawnedObstacles.Add(obs);
        }
    }

    private void Colorise(GameObject obstacle)
    {
        var color = Settings.obstacleColor[rnd.Next(Settings.obstacleColor.Count)];
        var srs = obstacle.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in srs)
        {
            sprite.color = color;
        }
    }

    public void Clean()
    {
        foreach (var obs in spawnedObstacles)
        {
            Destroy(obs);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }
    }

    public void FillPool(int score)
    {
        if (obstPrefabs.ContainsKey(score))
        {
            obstaclesPool.Add(obstPrefabs[score]);
        }
    }

    public GameObject TakeObstacle()
    {
        for (int i = 1; i < obstaclesPool.Count; i++)
        {
            var rand = rnd.Next(100);
            if (rand <= spawnChance[i])
            {
                return obstaclesPool[i];
            }
        }

        return obstaclesPool[0];
    }

}
