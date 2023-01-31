using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffest = 3.10f;

    const float referenceX = 1.777658f;
    const float referenceY = 1.333548f;

    [SerializeField] private Transform target;
    private GameObject background;

    private GameManager gameManager;
    void Start () {
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        this.background = GameObject.Find("Background");
    }
    
    void Update()
    {  
        if(!this.gameManager.IsPaused()) {

            //this.background.transform.localScale = new Vector2(
            //    (referenceX * this.GetComponent<Camera>().orthographicSize) / 5,
            //    (referenceY * this.GetComponent<Camera>().orthographicSize) / 5
            //);

            Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffest, -10f);
            this.transform.position = Vector3.Slerp(this.transform.position, newPos, followSpeed * Time.deltaTime);
        }
    }

}
