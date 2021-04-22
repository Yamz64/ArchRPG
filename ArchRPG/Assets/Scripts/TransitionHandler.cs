using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
    public float battle_transition_time;
    public bool transition_completed;

    private GameObject t_canvas;

    IEnumerator BattleTransition()
    {
        GetComponent<PauseMenuHandler>().pause_menu_protection = true;
        //set all values to 0
        t_canvas.SetActive(true);
        Image b_transition = t_canvas.transform.GetChild(0).GetComponent<Image>();
        b_transition.gameObject.SetActive(true);
        b_transition.transform.localScale = Vector3.zero;
        b_transition.fillAmount = 0;

        //play sound
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOverworldAudioHandler>().PlaySound("Sound/SFX/EncounterSmall");

        //the actual animation
        float progress = 0;
        while(progress < battle_transition_time)
        {
            progress += 1f / 24f;
            yield return new WaitForSeconds(1f / 24f);
            b_transition.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(2.05f, 2.05f, 2.05f), progress);
            b_transition.fillAmount = progress * (1f / battle_transition_time);
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().isPlaying == false);
        transition_completed = true;
        GetComponent<PauseMenuHandler>().pause_menu_protection = false;
    }

    IEnumerator Fade(float duration = 1)
    {
        GetComponent<PauseMenuHandler>().pause_menu_protection = true;
        //set all values to 0
        t_canvas.SetActive(true);
        Image f_transition = t_canvas.transform.GetChild(1).GetComponent<Image>();
        f_transition.gameObject.SetActive(true);

        //tween it's alpha value
        float progress = 0;
        while(progress < duration)
        {
            progress += 1 / 24f;
            yield return new WaitForSeconds(1 / 24f);
            f_transition.color = Color.Lerp(new Color(f_transition.color.r, f_transition.color.g, 
                f_transition.color.b, 0.0f), new Color(f_transition.color.r, f_transition.color.g, f_transition.color.b, 1.0f), progress/duration);
        }
        transition_completed = true;
        GetComponent<PauseMenuHandler>().pause_menu_protection = false;
    }

    IEnumerator FadeOut(float duration = 1)
    {
        GetComponent<PauseMenuHandler>().pause_menu_protection = true;
        //set all values to 0
        t_canvas.SetActive(true);
        Image f_transition = t_canvas.transform.GetChild(1).GetComponent<Image>();
        f_transition.gameObject.SetActive(true);

        //tween it's alpha value
        float progress = 0;
        while (progress < duration)
        {
            progress += 1 / 24f;
            yield return new WaitForSeconds(1 / 24f);
            f_transition.color = Color.Lerp(new Color(f_transition.color.r, f_transition.color.g,
                f_transition.color.b, 1.0f), new Color(f_transition.color.r, f_transition.color.g, f_transition.color.b, 0.0f), progress / duration);
        }
        transition_completed = true;
        GetComponent<PauseMenuHandler>().pause_menu_protection = false;
    }

    public void BattleTransitionDriver()
    {
        StopAllCoroutines();
        transition_completed = false;
        StartCoroutine(BattleTransition());
    }

    public void FadeDriver(float duration = 1)
    {
        StopAllCoroutines();
        transition_completed = false;
        StartCoroutine(Fade(duration));
    }

    public void FadeoutDriver(float duration = 1)
    {
        StopAllCoroutines();
        transition_completed = false;
        StartCoroutine(FadeOut(duration));
    }

    public void SetFadeColor(Color c)
    {
        Image f_transition = t_canvas.transform.GetChild(1).GetComponent<Image>();
        f_transition.color = new Color(c.r, c.g, c.b, f_transition.color.a);
    }

    // Start is called before the first frame update
    void Start()
    {
        transition_completed = false;
        t_canvas = transform.GetChild(2).gameObject;
    }
}
