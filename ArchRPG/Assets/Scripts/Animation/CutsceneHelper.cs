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
}
