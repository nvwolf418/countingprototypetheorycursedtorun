using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] meteors;
    private GameObject player;
    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ShieldWarrior");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InvokeRepeating("CreateMeteor", 8, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateMeteor()
    {
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null && gameManager.gameStarted)
        {
            pooledProjectile.SetActive(true); // activate it
            pooledProjectile.transform.position = player.transform.position + 
                new Vector3(Random.Range(-12, 12), Random.Range(10, 15), Random.Range(20, 30)); // position it at player
        }
    }
}
