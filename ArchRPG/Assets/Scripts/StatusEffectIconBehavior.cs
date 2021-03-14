using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIconBehavior : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites;
    private Image rend;

    public void SetStatus(int status)
    {
        if (status >= sprites.Count) return;
        if (sprites[status] == null) return;
        rend.sprite = sprites[status];
    }

    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<Image>();
    }
}
