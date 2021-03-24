using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloorColorCycler : MonoBehaviour
{
    public Texture2D cycler;
    public float change_delay;

    [SerializeField]
    public List<Color> ramp;

    private float max_delay;
    private Color[] color_data;
    private SpriteRenderer rend;

    //function will cycle colors in the color ramp and then apply them to the color_data's texture
    private void CycleColors()
    {
        //cycle the ramp
        if (ramp != null && ramp.Count != 0) {
            Color first = new Color(ramp[0].r, ramp[0].g, ramp[0].b, ramp[0].a);

            for (int i = 0; i < ramp.Count; i++)
            {
                if (i != ramp.Count - 1) ramp[i] = ramp[i + 1];
                else ramp[i] = first;
            }
        }

        //determine where on the ramp the color data of a pixel would place
        Color[] new_color_data = new Color[color_data.Length];
        for(int i=0; i<color_data.Length; i++)
        {
            //average RGB and find where on the ramp the value is
            float average = (color_data[i].r + color_data[i].g + color_data[i].b) / 3f;
            int position = (int)(average * (ramp.Count - 1));

            new_color_data[i] = ramp[position];
        }

        //set the sprite's color data to the new color data
        rend.sprite.texture.SetPixels(new_color_data);
        rend.sprite.texture.Apply(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        max_delay = change_delay;
        change_delay = 0;

        rend = GetComponent<SpriteRenderer>();
        color_data = cycler.GetPixels();
    }

    // Update is called once per frame
    void Update()
    {
        if(change_delay < max_delay)
        {
            change_delay += Time.deltaTime;
        }
        else
        {
            CycleColors();
            change_delay = 0;
        }
    }
}
