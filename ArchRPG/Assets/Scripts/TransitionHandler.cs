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
        //set all values to 0
        t_canvas.SetActive(true);
        Image b_transition = t_canvas.transform.GetChild(0).GetComponent<Image>();
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
    }

    public void BattleTransitionDriver()
    {
        StartCoroutine(BattleTransition());
    }

    // Start is called before the first frame update
    void Start()
    {
        transition_completed = false;
        t_canvas = transform.GetChild(2).gameObject;
    }
}
