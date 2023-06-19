using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockFlash : MonoBehaviour
{
    public GameObject ClockTekst;
    bool flagOnce;

    private void OnEnable()
    {
        if (!flagOnce)
        {
            flagOnce = true;
            StartCoroutine(Flicker());
        }
    }

    IEnumerator Flicker()
    {
        ClockTekst.SetActive(true);
        yield return new WaitForSeconds(1);
        ClockTekst.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        ClockTekst.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        ClockTekst.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        ClockTekst.SetActive(true);
        yield return new WaitForSeconds(1);
    }
}
