using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAInfo : MonoBehaviour
{
    public OverlayTile activeTileIA;
    public int rangoMov = 0;
    public int rangoAtaque = 0;
    public int ataque = 0;
    public int vida = 0;
    public int totalHealth = 0;
    public GameObject cuadroDeTextoPrefab;
    private GameObject cuadroDeTexto;
    private bool cuadroActivo = false;

    private float changeTime = 0.1f;
    private int parpadeos = 0;
    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        totalHealth = vida;
    }
    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(1) && cuadroActivo == false) // 1 representa el boton derecho del raton
        {
            // Acciones que deseas que ocurran al hacer clic derecho sobre el personaje
            Vector2 posicionClic = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            posicionClic += new Vector2(0f, 0.3f);

            // Crear el cuadro de texto en la posicion del clic
            cuadroDeTexto = Instantiate(cuadroDeTextoPrefab, posicionClic, Quaternion.identity);//genero desde el pstart y lo destruyo
            MovDialogo scriptCuadro = cuadroDeTexto.GetComponent<MovDialogo>();

            scriptCuadro.SetValoresText(ataque, vida);
            cuadroActivo = true;
        }
        else if (Input.GetMouseButtonDown(1) && cuadroActivo == true)
        {
            Destroy(cuadroDeTexto);
            cuadroActivo = false;
        }
    }

    public void BorrarCuadroTextoIA()
    {
        if (cuadroActivo == true)
        {
            Destroy(cuadroDeTexto);
            cuadroActivo = false;
        }
    }
    public int GetAtaque()
    {
        return ataque;
    }
    public void Pupa(int da�o)
    {


        InvokeRepeating("ChangeVisibility", 0f, changeTime);
        vida -= da�o;
        //scriptCuadro.SetValoresText(ataque,vida);
        if (vida <= 0)
        {
            Destroy(this.gameObject);
            Destroy(cuadroDeTexto);
        }

    }
    void ChangeVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        parpadeos++;
        if (parpadeos >= 6)
        {

            parpadeos = 0;
            CancelInvoke("ChangeVisibility");
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("PowerVida"))
        {
            vida += 1;
            publicVariables.healthInMap = false;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PowerAtaque"))
        {
            ataque += 1;
            publicVariables.swordInMap = false;
            Destroy(other.gameObject);
        }
    }

    public void EmpezarAtaque()
    {
        anim.SetBool("Ataque", true);
        foreach (Transform child in transform)
        {
            Animator childAnimator = child.GetComponent<Animator>();
            if (childAnimator != null)
            {
                childAnimator.SetBool("Ataque", true);
            }
        }

        Invoke("FinAtaque", 1.5f);
    }

    public void FinAtaque()
    {
        anim.SetBool("Ataque", false);
        foreach (Transform child in transform)
        {
            Animator childAnimator = child.GetComponent<Animator>();
            if (childAnimator != null)
            {
                childAnimator.SetBool("Ataque", false);
            }
        }
    }
}
