using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyShotgunAdult : NPCDialogue
{
    private AudioSource shotgun;

    IEnumerator PlayFunnyNoise()
    {
        //wait until the dialogue box is finished or closed before playing the shotgun noise
        yield return new WaitUntil(() => !player.GetActive() || (player.GetWriteCount() == 0 && player.GetWriting() == false));
        shotgun.Play();
    }

    // Start is called before the first frame update
    public new void Start()
    {
        shotgun = GetComponent<AudioSource>();
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(PlayFunnyNoise());
    }
}
