using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ikot : MonoBehaviour
{
    public float rotationSpd = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpd * Time.deltaTime, 0);

    }
}
