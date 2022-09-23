using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvent : Event
{
    public ParticleSystem fire;
    AudioSource m_AudioSource; 
    public override void Execute(int parameter)
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.Play();
        Vector3 happendPosition = transform.position + new Vector3 (0.0f,transform.localScale.y / 2 , 0.0f);
        Instantiate(fire, happendPosition, transform.rotation);

        Invoke("destory", 0.75f);
        Invoke("stopsound", 1.25f);
    }

    public void destory()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public void stopsound()
    {
        m_AudioSource.Stop();
    }
}
