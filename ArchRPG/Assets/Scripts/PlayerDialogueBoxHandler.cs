﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogueBoxHandler : MonoBehaviour
{
    public float scroll_speed;

    private bool active;
    private bool writing;
    private List<string> write_queue;
    private List<EffectContainer> effect_queue;
    private GameObject dialogue_box;
    private TMP_Text text;
    private PlayerMovement movement;

    public void OpenTextBox()
    {
        active = true;
        movement.interaction_protection = true;
        GetComponent<PauseMenuHandler>().pause_menu_protection = true;
        dialogue_box.SetActive(true);
    }

    public void CloseTextBox()
    {
        active = false;
        StartCoroutine(ResetInteraction());
        dialogue_box.SetActive(false);
    }

    //public function for clearing the text of the textbox
    public void Clear()
    {
        text.text = "";
    }

    public void SetWriteQueue(List<string> new_queue)
    {
        write_queue = new List<string>();
        for(int i=0; i<new_queue.Count; i++)
        {
            write_queue.Add(new_queue[i]);
        }
    }

    public void SetEffectQueue(List<EffectContainer> new_effects)
    {
        effect_queue = new List<EffectContainer>();
        for(int i=0; i<new_effects.Count; i++)
        {
            effect_queue.Add(new_effects[i]);
        }
    }

    private IEnumerator ResetInteraction()
    {
        yield return new WaitForEndOfFrame();
        movement.interaction_protection = false;
        GetComponent<PauseMenuHandler>().pause_menu_protection = false;
    }

    //function that handles scrolling text
    private IEnumerator Write()
    {
        Clear();
        writing = true;
        for(int i=0; i<write_queue[0].Length; i++)
        {
            yield return new WaitForSeconds(1f/scroll_speed);
            text.text += write_queue[0][i];
        }
        writing = false;
        write_queue.RemoveAt(0);
    }

    public int GetWriteCount() { return write_queue.Count; }
    public bool GetActive() { return active; }

    //public version of writing function
    public void WriteDriver()
    {
        StartCoroutine(Write());
    }

    public void HandleEffects()
    {
        //handle effects
        text.ForceMeshUpdate();
        Mesh mesh = text.mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] vertex_colors = mesh.colors;

        //loop through all effects 
        for (int i = 0; i < effect_queue[0].effects.Count; i++)
        {
            //find effects with particular names to do that effect
            //--WAVE--
            if (effect_queue[0].effects[i].name == "Wave")
            {
                for (int j = 0; j < vertices.Length; j++)
                {
                    int character = j / 4;
                    if (character >= effect_queue[0].effects[i].lower && character <= effect_queue[0].effects[i].upper)
                    {
                        Vector3 offset = Wave(Time.time + (float)character / 2f);
                        vertices[j] = vertices[j] + offset;
                    }
                }
            }
            //--COLOR--
            if (effect_queue[0].effects[i].name == "Color")
            {
                for (int j = 0; j < vertex_colors.Length; j++)
                {
                    int character = j / 4;
                    if (character >= effect_queue[0].effects[i].lower && character <= effect_queue[0].effects[i].upper)
                    {
                        vertex_colors[j] = effect_queue[0].effects[i].color;
                    }
                }
            }
            //--QUAKE--
            if (effect_queue[0].effects[i].name == "Quake")
            {
                for (int j = 0; j < vertices.Length; j += 4)
                {
                    int character = j / 4;
                    if (character >= effect_queue[0].effects[i].lower && character <= effect_queue[0].effects[i].upper)
                    {
                        Vector3 offset = Quake(2f);
                        vertices[j] = vertices[j] + offset;
                        vertices[j + 1] = vertices[j + 1] + offset;
                        vertices[j + 2] = vertices[j + 2] + offset;
                        vertices[j + 3] = vertices[j + 3] + offset;
                    }
                }
            }
        }

        //update the vertices
        mesh.vertices = vertices;
        mesh.colors = vertex_colors;
        text.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wave(float time)
    {
        return new Vector2(0.0f, Mathf.Sin(time * 2.2f) * 5);
    }

    Vector2 Quake(float intensity)
    {
        return new Vector2(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogue_box = transform.GetChild(1).GetChild(0).gameObject;
        text = dialogue_box.transform.GetChild(0).GetComponent<TMP_Text>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (active)
        {
            //handle cancelling of dialogue
            if (Input.GetButtonDown("Interact") && writing)
            {
                StopAllCoroutines();
                Clear();
                text.text = write_queue[0];
                write_queue.RemoveAt(0);
                writing = false;
            }
            else
            {
                if (write_queue.Count > 0 && Input.GetButtonDown("Interact"))
                {
                    Clear();
                    effect_queue.RemoveAt(0);
                    WriteDriver();
                }
                else if (write_queue.Count == 0 && Input.GetButtonDown("Interact"))
                {
                    Clear();
                    effect_queue.Clear();
                    CloseTextBox();
                }
            }

            if(effect_queue.Count > 0 && text.text.Length > 0)
            HandleEffects();
        }
    }
}
