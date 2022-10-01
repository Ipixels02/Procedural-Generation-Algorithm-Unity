using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//let camera follow target
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    private Camera mainCamera;
    public float lerpSpeed = 2.5f;

    private Vector3 offset;

    private Vector3 targetPos;

    public void Follow()
    {
        Awake();
        Camera.main.orthographic = true;
    }

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        if (playerTransform == null) return;
        //if (this.playerTransform == null)
        //{
        //    playerTransform = GameObject.FindWithTag("Player").transform;
        //}
        //offset = transform.position - playerTransform.position;


        this.transform.position = new Vector3()
        {
            x = this.playerTransform.position.x,
            y = this.playerTransform.position.y,
            z = this.playerTransform.position.z - 10,
        };
    }

    private void Update()
    {
        if (playerTransform == null) return;
  
            Vector3 target = new Vector3()
            {
                x = this.playerTransform.position.x,
                y = this.playerTransform.position.y,
                z = this.playerTransform.position.z - 10,
            };
        Vector3 pos = Vector3.Lerp(this.transform.position, target, this.lerpSpeed * Time.deltaTime);

        this.transform.position = pos;
        //targetPos = playerTransform.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}

