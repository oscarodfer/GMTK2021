using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{

    [SerializeField] float maxRadius =1;
    [SerializeField] float minRadius = 0.25f;
    [SerializeField] float frequency = 1;
    [SerializeField] float transparency = 1;
    [SerializeField] Material material;
    [SerializeField] AudioClip achievedSound;
    AudioSource aSource;
    public float ThetaScale = 0.01f;
    public float radius = 1f;

    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;
    private bool activated;
    Animator anim;
    void Start()
    {
        
        activated = false;
        radius = maxRadius;
        LineDrawer = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
        LineDrawer.materials[0] = new Material(material);
    }

    void Update()
    {
        ChangeRadious();
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = (radius * Mathf.Cos(Theta)) + transform.position.x;
            float y = (radius * Mathf.Sin(Theta)) + transform.position.y;
            LineDrawer.SetPosition(i, new Vector3(x, y, 0));
            for (int j = 0 ; j< LineDrawer.colorGradient.alphaKeys.Length; j++)
            {

                //LineDrawer.colorGradient.alphaKeys[j].alpha = transparency;
                LineDrawer.materials[0].color = new Color(LineDrawer.materials[0].color.r, LineDrawer.materials[0].color.g, LineDrawer.materials[0].color.b, transparency);
            }
        }
    }

    private void ChangeRadious()
    {
        if (!activated)
        {
            float sinValue = Mathf.Sin(Time.time * frequency);
            sinValue = Mathf.Abs(sinValue);
            radius = Mathf.SmoothStep(minRadius, maxRadius, sinValue / 1);
        }
       
        
    }

    private void Activate()
    {
        activated = true;
        aSource.PlayOneShot(achievedSound);
        anim.SetTrigger("Activated");
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!activated) Activate();
        }
    }
}
