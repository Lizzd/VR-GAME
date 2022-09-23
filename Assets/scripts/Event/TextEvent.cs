using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvent : Event
{
    public string text = "";
    public int space_interval = 4;
    public int text_limit = 4;
    int text_current = 0;
    public string target_text = "1    2    3    4";
    public GameObject textObject;
    public List<Event> events;
    public override void Execute(int parameter)
    {
        if(parameter < 0)
        {
            text = "";
            text_current = 0;
        } else if(parameter == 10)
        {
            if (text == target_text)
            {
                TriggerEvents();
            }
        } else
        {
            if(text_current < text_limit)
            {
                add(parameter);
                text_current += 1;
            }
            
        }
        if(textObject.GetComponent<TextMesh>() != null)
        {
            textObject.GetComponent<TextMesh>().text = text;
        } else
        {
            Debug.LogWarningFormat(" Text Mesh Error!");
        }
        

    }
    private void add(int num)
    {
        if(text.Length == 0)
        {
            text += num.ToString();
        } else
        {
           text += new string(' ', space_interval) + num.ToString();
        }
    }
    private void TriggerEvents()
    {
        foreach(Event e in events)
        {
            e.Execute(0);
        }
    }
}
