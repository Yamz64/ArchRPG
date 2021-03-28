using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class statIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void Update()
    {
        if (on)
        {
            OnMouseEnter();
        }
        else
        {
            OnMouseExit();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        on = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        on = false;
    }

    public void OnMouseEnter()
    {
        blurbBack.gameObject.SetActive(true);
        blurbBack2.gameObject.SetActive(true);
        statBlurb.gameObject.SetActive(true);
        statBlurb.text = desc + "\nTurns: " + count;
        Debug.Log("Pointer entered the image");
        
    }

    public void OnMouseExit()
    {
        blurbBack.gameObject.SetActive(false);
        blurbBack2.gameObject.SetActive(false);
        statBlurb.gameObject.SetActive(false);
        statBlurb.text = "";
        Debug.Log("Pointer escaped the image");
    }

    public bool on = false;
    public Image blurbBack;
    public Image blurbBack2;
    public Text statBlurb;
    public string desc;
    public int count;
}
