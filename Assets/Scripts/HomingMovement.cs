using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    
    // cache
    GameObject homingTo;

    // Start is called before the first frame update
    void Start()
    {
        // cache
        homingTo = GameObject.Find("Player");
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);   
    }

    // Update is called once per frame
    void Update()
    {
        HomeIn();
    }

    private void HomeIn()
    {
        if (homingTo)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                homingTo.transform.position,
                                moveSpeed * Time.deltaTime);
        }

    }
}
