using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CutsceneHelper
{
    public static IEnumerator TranslateCharacter(GameObject character, Vector2 destination, float time)
    {
        Vector2 start_pos = character.transform.position;
        float progress = 0;
        while (progress < time)
        {
            progress += 1f / 60f;
            character.transform.position = Vector3.Lerp(start_pos, destination, progress/time);
            yield return new WaitForSeconds(1f / 60f);
        }
        character.transform.position = destination;
    }

    public static IEnumerator SlowStartTranslateCharacter(GameObject character, Vector2 destination, float time)
    {
        Vector2 start_pos = character.transform.position;
        float progress = 0;
        while(progress < time)
        {
            progress += 1 / 60f;
            character.transform.position = Vector3.Lerp(start_pos, destination, (Mathf.Pow(progress, 2)/time) / time);
            yield return new WaitForSeconds(1f/60f);
        }
        character.transform.position = destination;
    }

    public static IEnumerator EaseOutTranslateCharacter(GameObject character, Vector2 destination, float time)
    {
        Vector2 start_pos = character.transform.position;
        float progress = 0;
        while(progress < time)
        {
            progress += 1f / 60f;
            character.transform.position = Vector3.Lerp(start_pos, destination, (time * Mathf.Sin((Mathf.PI * progress)/(2*time))) / time);
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    public static IEnumerator InterpColor(SpriteRenderer rend, Color color, float time)
    {
        Color start_color = rend.color;
        float progress = 0;
        while(progress < time)
        {
            progress += 1f / 60f;
            rend.color = Color.Lerp(start_color, color, progress / time);
            yield return new WaitForSeconds(1f / 60f);
        }
        rend.color = color;
    }
}
