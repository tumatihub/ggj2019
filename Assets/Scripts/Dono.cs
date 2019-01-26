using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dono : MonoBehaviour
{
    public float carinhoRate = .5f;

    private int qtdComidaPote = 5;

    private float carinhoBonus = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecebeBrincadeira(GameManager gameManager)
    {
        if (Random.value > carinhoRate)
        {
            gameManager.Carinho(carinhoBonus);
        }
    }

    public bool TemComidaNoPote()
    {
        return qtdComidaPote > 0;
    }

    public void RetiraComidaDoPote()
    {
        qtdComidaPote -= 1;
    }
}
