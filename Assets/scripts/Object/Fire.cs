using System;
using System.Collections;
using UnityEngine;

public class Fire : ObjectAnchor
{
    // [Header("exploded cube")]
    public ParticleSystem smoke;

    // check if should emitt smoke
    public bool emitt_smoke = false;

    void Start()
    {
        emitt_smoke = false;
        smoke.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        // smoke.GetComponent<ParticleSystem>().enableEmission = true;
        if (emitt_smoke)
        {
            Instantiate(smoke, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
