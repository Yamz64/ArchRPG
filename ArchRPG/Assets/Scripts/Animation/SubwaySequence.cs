using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwaySequence : MonoBehaviour
{
    IEnumerator TestEase()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(CutsceneHelper.EaseOutTranslateCharacter(gameObject, (Vector2)transform.position + Vector2.up * 2, .1f));
            yield return new WaitForSeconds(2f);
            transform.position = new Vector2(transform.position.x, transform.position.y - 2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestEase());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
