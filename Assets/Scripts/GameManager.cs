using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider barraFelicidade;
    public Slider barraFome;
    public Slider barraSono;

    public Color TextFelicidadeColor;
    public Color TextFomeColor;
    public Color TextSonoColor;

    public Transform SpawnerMsgBonus;
    public Text BonusMsgPrefab;
    public Canvas Canvas;

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

    [SerializeField] private float bonusComida = 10;
    [SerializeField] private float bonusDormir = 10;
    [SerializeField] private float bonusCarinho = 10;

    [SerializeField] private float tempoComer = 2f;
    [SerializeField] private float tempoDormir = 6f;
    [SerializeField] private float tempoBrincar = 2f;

    [SerializeField] private List<Button> botoes;

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
        Felicidade = Mathf.Clamp(Felicidade, 0, MaxFelicidade);
        barraFelicidade.value = Felicidade / MaxFelicidade;

        // Update Fome
        Fome -= Time.deltaTime * fomeRate;
        Fome = Mathf.Clamp(Fome, 0, MaxFome);
        barraFome.value = Fome / MaxFome;

        // Update Sono
        Sono -= Time.deltaTime * sonoRate;
        Sono = Mathf.Clamp(Sono, 0, MaxSono);
        barraSono.value = Sono / MaxSono;
    }

    private void DesativarBotoes()
    {
        foreach(Button botao in botoes)
        {
            botao.interactable = false;
        }
    }

    private void AtivarBotoes()
    {
        foreach (Button botao in botoes)
        {
            botao.interactable = true;
        }
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
        DesativarBotoes();
        Anim.SetBool("Comendo", true);
        yield return new WaitForSeconds(tempoComer);
        Anim.SetBool("Comendo", false);
        //DonoScript.RetiraComidaDoPote();
        AtivarBotoes();
        GanhaPontoComida();
    }

    void GanhaPontoComida()
    {
        string msg = "-" + bonusComida + " de Fome";
        StartCoroutine(GeraMsgBonus(msg, TextFomeColor));
        Fome = Mathf.Clamp(Fome - bonusComida, 0, MaxFome);
    }

    public void Dormir()
    {
        StartCoroutine(DormirAnim());
    }

    IEnumerator DormirAnim()
    {
        DesativarBotoes();
        Anim.SetBool("Dormindo", true);
        yield return new WaitForSeconds(tempoDormir);
        Anim.SetBool("Dormindo", false);
        string msg = "-" + bonusDormir + " de Sono";
        StartCoroutine(GeraMsgBonus(msg, TextSonoColor));
        Sono = Mathf.Clamp(Sono - bonusDormir, 0, MaxSono);
        AtivarBotoes();
    }

    public void Brincar()
    {
        StartCoroutine(BrincarAnim());
    }

    IEnumerator BrincarAnim()
    {
        DesativarBotoes();
        Anim.SetBool("Brincando", true);
        yield return new WaitForSeconds(tempoBrincar);
        Anim.SetBool("Brincando", false);
        DonoScript.RecebeBrincadeira(this);
        AtivarBotoes();
    }

    public void Carinho()
    {
        Debug.Log("Dono fez carinho");
        string msg = "+" + bonusCarinho + " de Felicidade";
        StartCoroutine(GeraMsgBonus(msg, TextFelicidadeColor));
        Felicidade = Mathf.Clamp(Felicidade + bonusCarinho, 0, MaxFelicidade);
    }

    IEnumerator GeraMsgBonus(string msg, Color color)
    {
        Text msgObject = Instantiate(BonusMsgPrefab, Canvas.transform);
        msgObject.text = msg;
        msgObject.color = color;
        yield return new WaitForSeconds(2f);
        Destroy(msgObject.gameObject);

    }
}
