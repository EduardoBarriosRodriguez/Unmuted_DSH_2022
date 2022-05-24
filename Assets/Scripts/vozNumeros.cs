using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Windows.Speech;

public class vozNumeros : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public int respuestaJugador;
    // Start is called before the first frame update
    void Start()
    {
        respuestaJugador = 0;
        actions.Add("uno", fUno);
        actions.Add("dos", fDos);
        actions.Add("tres", fTres);
        actions.Add("cuatro", fCuatro);
        actions.Add("cinco", fCinco);
        actions.Add("seis", fSeis);
        actions.Add("siete", fSiete);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }
    
    private void fUno(){respuestaJugador = 1;}

    private void fDos(){respuestaJugador = 2;}
    
    private void fTres(){respuestaJugador = 3;}

    private void fCuatro(){respuestaJugador = 4;}
    
    private void fCinco(){respuestaJugador = 5;}

    private void fSeis(){respuestaJugador = 6;}

    private void fSiete(){respuestaJugador = 7;}

}
