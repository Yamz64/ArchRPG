using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class statTree : MonoBehaviour
{
    public List<statIcon> spaces;
    public string desc;
    public int count;
    public Image blurbBack;
    public Image blurbBack2;
    public Text statBlurb;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y));
        mousePosition.z = 90;

        desc = "";

        for (int i = 0; i < 8; i++)
        {
            if (spaces[i].GetComponent<BoxCollider2D>().OverlapPoint(mousePosition) && !spaces[i].desc.Equals(""))
            {
                desc = spaces[i].desc;
                count = spaces[i].count;
            }
        }

        if (!desc.Equals(""))
        {
            blurbBack.gameObject.SetActive(true);
            blurbBack2.gameObject.SetActive(true);
            statBlurb.gameObject.SetActive(true);
            statBlurb.text = desc + "\nTurns: " + count;
        }
        else
        {
            blurbBack.gameObject.SetActive(false);
            blurbBack2.gameObject.SetActive(false);
            statBlurb.gameObject.SetActive(false);
            statBlurb.text = "";
        }

    }
}
