using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class JuegoNumeros : MonoBehaviour
{
    public AudioSource audioSource, audioSource_door, audioSource_key, audioSource_NO, audioSource_Fondo;

    public AudioClip audioClip, zumbidoMovil, get_key, open_door, NO_1, NO_2, NO_3, audioFondo;

    public vozNumeros vozNumeros_class;
    public TextMeshProUGUI interfaz_llaves;
    public GameObject lienzoVideoScreamer;
    public VideoPlayer videoPlayer;

    // Esta variable sera 0, 1, 2 (Facil, medio, dificil).
    public int DIFICULTAD;

    public int VidasJugador, RespuestaCorrecta, numLLaves, seconds, TiempoMargen,
                TiempoMinEsperaNiña, TiempoMaxEsperaNiña;

    public int[] orden;

    public string PathVoces;
    public bool hasMuerto;

    
    // Inicializa todas las variables necesarias para empezar la partida.
    private void Awake()
    {
        DIFICULTAD = cambiarScene.dificultad;
        
        hasMuerto = false;

        VidasJugador = 3 + (Random.Range(0,4)%4);

        numLLaves = 0;
        interfaz_llaves.text = numLLaves.ToString();

        PathVoces = "file://" + Application.streamingAssetsPath + "/";
        
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource_door = gameObject.AddComponent<AudioSource>();
        audioSource_door.clip = open_door;
        audioSource_door.volume = 0.5f;

        audioSource_Fondo = gameObject.AddComponent<AudioSource>();
        audioSource_Fondo.clip = audioFondo;
        audioSource_Fondo.volume = 0.1f;
        audioSource_Fondo.loop = true;
        audioSource_Fondo.Play();

        audioSource_key = gameObject.AddComponent<AudioSource>();
        audioSource_key.clip = get_key;

        audioSource_NO = gameObject.AddComponent<AudioSource>();

        // Variables que dependen de la dificultad
        orden = new int[4 + DIFICULTAD];
        TiempoMargen = 9 + (DIFICULTAD *2);
        TiempoMinEsperaNiña = 14 + (DIFICULTAD *2);
        TiempoMaxEsperaNiña = 20 + (DIFICULTAD *2);
        // El juego se llama continuamente hasta que ganes o pierdas.
        InvokeRepeating("LlamadaBucleJuego", 5f, Random.Range(TiempoMinEsperaNiña, TiempoMaxEsperaNiña + 1));
        
    }

    public void LlamadaBucleJuego() { StartCoroutine(BucleJuego()); }

    public IEnumerator BucleJuego() {

        if(hasMuerto == false)
        {
            seconds = 0;
            NuevaPartida();
            StartCoroutine(ReproducirPartida());

            vozNumeros_class.respuestaJugador = 0;
            
            // Mientras el jugador no ha respondido y no ha pasado el tiempo de margen
            // se espera.
            while(vozNumeros_class.respuestaJugador == 0 && seconds < TiempoMargen)
            {
                yield return new WaitForSeconds(1);
                seconds++;
            }

            if(ComprobarNumero(vozNumeros_class.respuestaJugador))
            {
                // Reinicia las variables necesarias
                vozNumeros_class.respuestaJugador = 0;
                seconds = 0;
                // Si acierta te da una llave
                audioSource_key.Play();
                numLLaves++;
                interfaz_llaves.text = numLLaves.ToString();

            } else
            {
                // Reinicia las variables necesarias
                vozNumeros_class.respuestaJugador = 0;
                seconds = 0;
                
                // Si se equivoca, pierde una vida.
                VidasJugador--;

                // Reproduce un audio de equivocación aleatorio.
                int randomNO = Random.Range(1,4);

                switch(randomNO)
                {
                    case 1:
                        audioSource_NO.clip = NO_1;
                        break;
                    case 2:
                        audioSource_NO.clip = NO_2;
                        break;
                    case 3:
                        audioSource_NO.clip = NO_3;
                        break;
                }

                audioSource_NO.Play();


                // Si pierde, reproduce el screamer y carga pantalla perdido.
                if(VidasJugador <= 0)
                {
                    hasMuerto = true;
                    yield return new WaitForSeconds(4);
                    audioSource_Fondo.Stop();
                    lienzoVideoScreamer.SetActive(true);
                    videoPlayer.Play();
                    
                    yield return new WaitForSeconds(9);
                    SceneManager.LoadScene("perdido");
                }
            }

        }
    }

    public void NuevaPartida()
    {
        // Elige la respuesta correcta de esta partida.
        RespuestaCorrecta = Random.Range(1,6 + DIFICULTAD);
        
        // Rellena el vector con los demás números.
        for(int i=1, indice = 0; i<= 5 + DIFICULTAD; i++)
        {
            if (i != RespuestaCorrecta) {
                orden[indice] = i;
                indice++;
            }
        }

        // Desordena el vector.
        for (int t = 0; t < orden.Length; t++ )
        {
            int tmp = orden[t];
            int r = Random.Range(t, orden.Length);
            orden[t] = orden[r];
            orden[r] = tmp;
        }
    }

    public IEnumerator ReproducirPartida() {

        // Reproduce un zumbido antes de empezar la partida.
        audioSource.clip = zumbidoMovil;
        audioSource.Play();
        yield return new WaitForSeconds(1);

        // Recorre el vector para ir reproduciendo los distintos audios.
        for(int i = 0; i < orden.Length; i++)
        {
            // Elige un audio aleatorio del fichero.
            int randomFile = Random.Range(1,4);

            // Espera a recibir el audio seleccionado.
            WWW request = GetAudioFromFile(PathVoces + orden[i].ToString() + "_Voces/", randomFile.ToString() + ".wav");
            yield return request;

            // Reproduce el audio seleccionado.
            audioClip = request.GetAudioClip();
            audioSource.clip = audioClip;
            audioSource.Play();

            // Espera a que el audio seleccionado termine de ser reproducido.
            yield return new WaitWhile (()=> audioSource.isPlaying);
        }
        
    }

    // Devuelve el audio pedido de la carpeta StreamingAssets
    private WWW GetAudioFromFile(string path, string filename)
    {
        string audioToLoad = string.Format(path + "{0}", filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    // Comprueba que el numero es correcto o no.
    public bool ComprobarNumero(int NumeroPropuesto) { return NumeroPropuesto == RespuestaCorrecta; }

    public void AbrirPuertaSonido() { audioSource_door.Play(); }

}