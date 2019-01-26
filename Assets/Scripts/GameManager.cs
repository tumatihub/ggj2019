using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider barraFelicidade;
    public Slider barraFome;
    public Slider barraSono;

    public GameObject Pet;

    public GameObject Dono;
    
    private Animator Anim;

    private Dono DonoScript;

    private float MaxFelicidade = 100f;
    private float MaxFome = 100f;
    private float MaxSono = 100f;

    private float Felicidade;
    private float Fome;
    private float Sono;

    private float fomeRate = -1f;
    private float sonoRate = -1f;
    private float felicidadeRate = 1f;

    private float bonusComida = 5;

    // Start is called before the first frame update
    void Start()
    {
        Anim = Pet.GetComponent<Animator>();

        DonoScript = Dono.GetComponent<Dono>();

        Felicidade = MaxFelicidade;
        Fome = 0;
        Sono = 0;

        barraFelicidade.value = 1f;
        barraFome.value = 0f;
        barraSono.value = 0f;


    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        // Update Felicidade
        Felicidade -= Time.deltaTime * felicidadeRate;
        barraFelicidade.value = Felicidade / MaxFelicidade;

        // Update Fome
        Fome -= Time.deltaTime * fomeRate;
        barraFome.value = Fome / MaxFome;

        // Update Sono
        Sono -= Time.deltaTime * sonoRate;
        barraSono.value = Sono / MaxSono;
    }

    public void Comer()
    {
        if (DonoScript.TemComidaNoPote())
        {
            StartCoroutine(ComerAnim());
        }
        else
        {
            Debug.Log("Não tem mais comida no pote...");
        }
    }

    IEnumerator ComerAnim()
    {
        Anim.SetBool("Comendo", true);
        yield return new WaitForSeconds(2f);
        Anim.SetBool("Comendo", false);
        DonoScript.RetiraComidaDoPote();
        Fome = Mathf.Clamp(Fome - bonusComida, 0, MaxFome);
    }

    public void Dormir()
    {

    }

    public void Brincar()
    {
        StartCoroutine(BrincarAnim());
    }

    IEnumerator BrincarAnim()
    {
        Anim.SetBool("Brincando", true);
        yield return new WaitForSeconds(2f);
        Anim.SetBool("Brincando", false);
        DonoScript.RecebeBrincadeira(this);
    }

    public void Carinho(float carinhoBonus)
    {
        Debug.Log("Dono fez carinho");
        Felicidade = Mathf.Clamp(Felicidade + carinhoBonus, 0, MaxFelicidade);
    }
}
