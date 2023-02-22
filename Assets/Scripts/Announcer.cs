using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Announcer : MonoBehaviour
{
    private float elapsedTime;
    public float stayTime;
    private bool displaying = false;
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (displaying)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime < 0f)
            {
                displaying = false;
                text.enabled = false;
            }
        }
    }

    public void announce(string msg)
    {
        text.enabled = true;
        text.SetText(msg);
        elapsedTime = stayTime;
        displaying = true;
    }
}
