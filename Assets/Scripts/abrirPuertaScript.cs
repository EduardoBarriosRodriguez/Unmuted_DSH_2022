using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class abrirPuertaScript : MonoBehaviour
{
    public Animator puerta;
    private bool enZona;
    private bool activa;
    public JuegoNumeros JuegoNumerosClass;

    void Awake()
    {

        JuegoNumerosClass = GameObject.Find("AudioManager").GetComponent<JuegoNumeros>();

    }

    void Update()
    {
        if(enZona == true)
        {
            if(Input.GetKeyDown(KeyCode.E) && JuegoNumerosClass.numLLaves > 0 && !puerta.GetBool("open"))
            {
                JuegoNumerosClass.AbrirPuertaSonido(); 
                puerta.SetBool("open", true);
                JuegoNumerosClass.numLLaves--;
                JuegoNumerosClass.interfaz_llaves.text = JuegoNumerosClass.numLLaves.ToString();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            enZona = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enZona = false;
        }
    }
}
