using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Tooltip("True: Light is predominantly on, False: Light is predominantly off")]
    public bool predominant_state;

    public float predominant_timer;
    public float subsequent_timer;

    private bool current_state;     //true in predominant state, false in subsequent state

    private float predominant_timer_max;
    private float subsequent_timer_max;

    private SpriteMask mask;
    private GameObject cookie;

    // Start is called before the first frame update
    void Start()
    {
        predominant_timer_max = predominant_timer;
        subsequent_timer_max = subsequent_timer;
        predominant_timer = 0;
        subsequent_timer = 0;

        current_state = Random.value > .5f;

        mask = GetComponent<SpriteMask>();
        cookie = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_state)
        {
            if (predominant_timer < predominant_timer_max)
            {
                predominant_timer += Time.deltaTime;
            }
            else
            {
                current_state = false;
                predominant_timer = Random.Range(0.0f, predominant_timer_max);
            }
        }
        else
        {
            if(subsequent_timer < subsequent_timer_max)
            {
                subsequent_timer += Time.deltaTime;
            }
            else
            {
                current_state = true;
                subsequent_timer = Random.Range(0.0f, subsequent_timer_max);
            }
        }
        //light is predominantly on
        if (predominant_state)
        {
            if (current_state)
            {
                mask.enabled = true;
                cookie.SetActive(true);
            }
            else
            {
                mask.enabled = false;
                cookie.SetActive(false);
            }
        }
        //light is predominatly off
        else
        {
            if (!current_state)
            {
                mask.enabled = true;
                cookie.SetActive(true);
            }
            else
            {
                mask.enabled = false;
                cookie.SetActive(false);
            }
        }
    }
}
