using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private GameObject player;
    [SerializeField] float speed = 10.0f;
    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ShieldWarrior");    
        gameManager = GameObject.Find("GameManager");    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((player.transform.position - transform.position + new Vector3(0, 0.5f,0)).normalized * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log(other.tag);
            gameObject.SetActive(false);
            gameManager.GetComponent<GameManager>().UpdatePlayerLives(-1);
        }
        //else if(other.CompareTag("Sword"))
        //{
        //    Debug.Log(other.tag);
        //    gameObject.SetActive(false);
        //    gameManager.GetComponent<GameManager>().UpdatePlayerScore(5);
        //}
        else if(!other.CompareTag("Untagged")  || other.CompareTag("Sword"))
        {
            Debug.Log(other.tag);
            gameObject.SetActive(false);
            gameManager.GetComponent<GameManager>().UpdatePlayerScore(1);
        }
    }
}
