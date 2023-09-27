using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penny : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 17f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if(Magnet.isMagnet == true)
        {
            if(Vector3.Distance(transform.position, playerTransform.position) < 7)
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            Destroy(gameObject);
        }
    }

}

