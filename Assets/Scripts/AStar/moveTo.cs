using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveTo : MonoBehaviour
{
    [Header("Zooop")]
    public Transform player;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Respec");
            agent.destination = player.position;
          
            gameObject.GetComponent<Animator>().Play("Zoop.Run");
            
        }
        // gameObject.GetComponent<Animator>().Play("Zoop.Idle");
        if(agent.transform.position == agent.destination)
        {
            Debug.Log("Arrived");
            gameObject.GetComponent<Animator>().Play("Zoop.Idle");
        }
    }
}