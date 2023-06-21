using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public GameObject CutsceneHolder;
    CompanionBehaviour _companionBehaviour;

    public bool lastcutscene;
    public GameObject clock;
    private void Awake()
    {
       _companionBehaviour = FindObjectOfType<CompanionBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CutsceneHolder.SetActive(true);
            _companionBehaviour.cutscene = true;

            if (lastcutscene)
                StartCoroutine(Timer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CutsceneHolder.SetActive(false);
            _companionBehaviour.cutscene = false;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5f);
        clock.SetActive(true);
    }
}
