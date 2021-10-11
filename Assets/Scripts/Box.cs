using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Box : MonoBehaviour
{
    public int number;
    private Rigidbody rb;
    [SerializeField] private int maxNumber;
    private TextMeshPro[] numberTexts;
    [SerializeField] Material material;
    [SerializeField] Material purpleMaterial;
    private Renderer rend;
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        numberTexts = GetComponentsInChildren<TextMeshPro>();
        number=Random.Range(1,maxNumber);
        RefreshNumbers();
    }

    public void ChangeMaterial()
    {
        rend.material = material;
    }
    public void ChangeMaterialToPurple()
    {
        rend.material = purpleMaterial;
    }
    public void RefreshNumbers()
    {
        for (int i = 0; i < numberTexts.Length; i++)
        {
            numberTexts[i].text = "" + number;
        }
    }
    public void DisablePhysics(bool togle)
    {
        rb.isKinematic = togle;
    }
}
