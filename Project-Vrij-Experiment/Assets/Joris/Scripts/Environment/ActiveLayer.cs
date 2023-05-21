using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLayer : MonoBehaviour
{
    [Header("ActiveLayerCheck")]
    [SerializeField]
    bool layerActive;
    [SerializeField]
    LayerMask ActiveLayerCheck;
    [SerializeField]
    float raylength;

    [Header("ClampToSurface")]
    [SerializeField]
    GameObject LayerPH;

    PlayerMovement _Player;
    PlayerSanityManager _SanityManager;
    private void Awake()
    {
        _Player = FindObjectOfType<PlayerMovement>();
        _SanityManager = FindObjectOfType<PlayerSanityManager>();
    }

    void Update()
    {
        layerActive = Physics.Raycast(transform.position, -transform.up, raylength, ActiveLayerCheck);

        if(_SanityManager.Sanity == 0)
        {
            StartCoroutine(LayerStick());
        }
    }

    IEnumerator LayerStick()
    {
        _Player.transform.parent = LayerPH.transform;

        yield return new WaitForSeconds(1);

        _Player.transform.parent = null;
    }
}
