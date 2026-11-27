using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeMachine : MonoBehaviour
{
    [Header("Zaman Katmanları")]
    public GameObject guncelDonemUcak;
    public GameObject gecmisDonemUcak;

    [Header("Atmosfer (Işık ve Gökyüzü)")]
    public GameObject gunesGuncel;
    public GameObject gunesGecmis;
    public Material skyboxGuncel;
    public Material skyboxGecmis;

    [Header("İşitsel Derinlik")]
    public AudioSource glitchSesi;

    [Header("Görsel Efektler")] // YENİ EKLENDİ
    public ParticleSystem gecisEfekti; // YENİ EKLENDİ

    [Header("VR Konforu (Fade)")]
    public CanvasGroup siyahEkranGecisi;

    [Header("VR Kontrol Tuşu")]
    public InputActionReference zamanTusu;

    private bool gecmisteMi = false;
    private bool gecisYapiliyor = false;

    void Start()
    {
        BaslangicDurumunuAyarla();
    }

    void OnEnable()
    {
        if (zamanTusu != null) zamanTusu.action.started += VRTusuBasildi;
    }

    void OnDisable()
    {
        if (zamanTusu != null) zamanTusu.action.started -= VRTusuBasildi;
    }

    private void VRTusuBasildi(InputAction.CallbackContext context)
    {
        if (!gecisYapiliyor) StartCoroutine(ZamansalDalgalanmaKareografisi());
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!gecisYapiliyor) StartCoroutine(ZamansalDalgalanmaKareografisi());
        }
    }

    private void BaslangicDurumunuAyarla()
    {
        gecmisteMi = false;
        if (guncelDonemUcak != null) guncelDonemUcak.SetActive(true);
        if (gecmisDonemUcak != null) gecmisDonemUcak.SetActive(false);
        if (gunesGuncel != null) gunesGuncel.SetActive(true);
        if (gunesGecmis != null) gunesGecmis.SetActive(false);

        if (skyboxGuncel != null)
        {
            RenderSettings.skybox = skyboxGuncel;
        }
    }

    private IEnumerator ZamansalDalgalanmaKareografisi()
    {
        gecisYapiliyor = true;

        if (glitchSesi != null) glitchSesi.Play();
        if (gecisEfekti != null) gecisEfekti.Play(); // YENİ EKLENDİ - Patlama efekti burada tetikleniyor

        float fadeSuresi = 0.4f;
        float sayac = 0f;
        while (sayac < fadeSuresi)
        {
            sayac += Time.deltaTime;
            if (siyahEkranGecisi != null) siyahEkranGecisi.alpha = Mathf.Lerp(0f, 1f, sayac / fadeSuresi);
            yield return null;
        }

        gecmisteMi = !gecmisteMi;

        if (guncelDonemUcak != null) guncelDonemUcak.SetActive(!gecmisteMi);
        if (gecmisDonemUcak != null) gecmisDonemUcak.SetActive(gecmisteMi);

        if (gunesGuncel != null) gunesGuncel.SetActive(!gecmisteMi);
        if (gunesGecmis != null) gunesGecmis.SetActive(gecmisteMi);

        Material yeniGokyuzu = gecmisteMi ? skyboxGecmis : skyboxGuncel;
        if (yeniGokyuzu != null)
        {
            RenderSettings.skybox = yeniGokyuzu;
        }

        yield return new WaitForSeconds(0.2f);

        sayac = 0f;
        while (sayac < fadeSuresi)
        {
            sayac += Time.deltaTime;
            if (siyahEkranGecisi != null) siyahEkranGecisi.alpha = Mathf.Lerp(1f, 0f, sayac / fadeSuresi);
            yield return null;
        }

        gecisYapiliyor = false;
    }
}