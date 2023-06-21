using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public GameObject CutsceneHolder;
    CompanionBehaviour _companionBehaviour;
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
}
