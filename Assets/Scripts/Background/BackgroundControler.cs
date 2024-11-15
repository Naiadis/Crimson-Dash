using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private float startPosition, lenght;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        startPosition = transform.position.x;
        lenght=GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x*parallaxEffect;
        float movement =cam.transform.position.x*(1-parallaxEffect);
        transform.position=new Vector3(startPosition +distance,transform.position.y,transform.position.z);

        if(movement>startPosition+lenght){
            startPosition+=lenght;
        }
        else if(movement<startPosition-lenght){
            startPosition-=lenght;
        }
    }
}
