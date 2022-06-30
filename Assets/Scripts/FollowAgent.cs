using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAgent : MonoBehaviour
{

    private float offSetZ; private float offSetY;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        offSetZ = transform.position.z - player.transform.position.z;
        offSetY = transform.position.y - player.transform.position.y;






    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, offSetY, offSetZ);


    }
}
