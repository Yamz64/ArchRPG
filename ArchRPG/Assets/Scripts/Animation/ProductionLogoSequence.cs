using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProductionLogoSequence : MonoBehaviour
{
    private bool faded;
    private VideoPlayer player;
    private RawImage image;

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.isPlaying);
        yield return new WaitUntil(() => !player.isPlaying);
        StartCoroutine(Fade());
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => faded);
        SceneManager.LoadScene("TitleScreen");
    }

    IEnumerator Fade(float duration = 1)
    {
        //tween it's alpha value
        float progress = 0;
        while (progress < duration)
        {
            progress += 1 / 24f;
            yield return new WaitForSeconds(1 / 24f);
            image.color = Color.Lerp(new Color(image.color.r, image.color.g, image.color.b, 1.0f), Color.black, progress / duration);
        }
        faded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        image = GetComponent<RawImage>();
        StartCoroutine(LateStart());
    }
}
