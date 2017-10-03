using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  Vincent Creveld GDV2 3019866
//
//  Deze class is een script wat je op bijna elk gameobject kan zetten.
//  De functie van deze class is de material van het gameobject met emission te highlighten volgens een Sinusgolf
//  In dit geval van de class heb ik gebruik gemaakt van een animationcurve om makkelijker de frequentie van de golf aan te kunnen passen in de editor
//
//  Een nadeel van de animationcurve is dat het op elk object weer opnieuw ingesteld moet worden.
//  
//  In een vervolg zou ik niet meer een animationcurve gebruiken maar gewoon een sinusgolf en deze manipuleren met variabele ipv dat de editor het allemaal voor mij doet
// 


public class HighlightableObj : MonoBehaviour, IHighlightable
{
    public AnimationCurve curve;
    public bool PullMatFromObj;
    public GameObject ObjToPullMat;
    public float duration;
    private Material mat;
    private float timer;

    public void Start(){
        if (PullMatFromObj == false){
            if (transform.GetComponent<MeshRenderer>().material != null){
                ObjToPullMat = gameObject;
            }
        }
        mat = ObjToPullMat.GetComponent<MeshRenderer>().material;
    }

    public Material ReturnMatGO(){
        return mat;
    }

    public void Highlight(){
        timer += Time.deltaTime;
        if (timer >= duration){
            timer -= duration;
        }
        float curveValue = curve.Evaluate(timer);
        Color tempColor = new Color(curveValue, 0, 0, 1);
        mat.SetColor("_EmissionColor", tempColor);
    }

    public void DeHighlight(){
        Color tempColor = new Color(0.0f, 0, 0, 1);
        mat.SetColor("_EmissionColor", tempColor);
        timer = 0;
    }

 }
