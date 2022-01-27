using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Siren : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer sirenLeftSMR;
    [SerializeField] SkinnedMeshRenderer sirenRightSMR;
    Material sirenLeftMaterial;
    Material sirenRightMaterial;
    Vector4 colorEmissionLeft;
    Vector4 colorEmissionRight;
    float emissionFactor = 0;
    [SerializeField] float speed = 0.2f;

    bool isWorking;

    private void Start()
    {
        sirenLeftMaterial = sirenLeftSMR.material;
        sirenRightMaterial = sirenRightSMR.material;
        colorEmissionLeft = new Vector4(sirenLeftMaterial.color.r, sirenLeftMaterial.color.g, sirenLeftMaterial.color.b);
        colorEmissionRight = new Vector4(sirenRightMaterial.color.r, sirenRightMaterial.color.g, sirenRightMaterial.color.b);
        sirenLeftSMR.material.EnableKeyword("_EMISSION");
        sirenRightSMR.material.EnableKeyword("_EMISSION");
        emissionFactor = 0;

        StartFlipFlop();
    }

    void StartFlipFlop()
    {
        Flip();
        isWorking = true;
    }

    void StopFlipFlop()
    {
        DOTween.KillAll();
        isWorking = false;
        sirenLeftSMR.material.SetVector("_EmissionColor", colorEmissionLeft * 0);
        sirenRightSMR.material.SetVector("_EmissionColor", colorEmissionRight * 0);
    }

    void Flip() =>
        DOTween.To(() => emissionFactor, x => emissionFactor = x, 1.0f, speed).OnUpdate(() =>
            {
                sirenLeftSMR.material.SetVector("_EmissionColor", colorEmissionLeft * emissionFactor);
                sirenRightSMR.material.SetVector("_EmissionColor", colorEmissionRight * (1 - emissionFactor));
            }).OnComplete(() => Flop());

    void Flop() =>
        DOTween.To(() => emissionFactor, x => emissionFactor = x, 0.0f, speed).OnUpdate(() =>
           {
               sirenLeftSMR.material.SetVector("_EmissionColor", colorEmissionLeft * emissionFactor);
               sirenRightSMR.material.SetVector("_EmissionColor", colorEmissionRight * (1 - emissionFactor));
           }).OnComplete(() => Flip());

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWorking == true) StopFlipFlop();
            else StartFlipFlop();
        }
    }
}