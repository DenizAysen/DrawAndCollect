using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("------TOP VE TEKNÝK OBJELER")]
    [SerializeField] private TopAtar _TopAtar;
    [SerializeField] private CizgiCiz _CizgiCiz;
    [Header("------GENEL OBJELER")]
    [SerializeField] private ParticleSystem KovaGirme;
    [SerializeField] private ParticleSystem BestScoreGecis;
    [SerializeField] private AudioSource[] Sesler;
    [Header("------UI OBJELER")]
    [SerializeField] private GameObject[] Paneller;
    [SerializeField] private TextMeshProUGUI[] ScoreTextleri;
    bool oyunBittiMi;
    bool sesCaldimi;
    int girenTopSayisi;
    void Start()
    {
        girenTopSayisi = 0;
        oyunBittiMi = false;
        sesCaldimi = false;
        //Eðer "BestScore" anahtarý varsa, anahtardaki deðer yazdýrýlýr
        girenTopSayisi = 0;
        if (PlayerPrefs.HasKey("BestScore"))
        {
            ScoreTextleri[0].text = PlayerPrefs.GetInt("BestScore").ToString();
            ScoreTextleri[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        }
        else
        {//Yoksa varsayýlan olarak 0 deðeri yazdýrýlýr
            PlayerPrefs.SetInt("BestScore", 0);
            ScoreTextleri[0].text = "0";
            ScoreTextleri[1].text = "0";
        }
    }
    public void DevamEt(Vector2 topPos)
    {//Top Kovaya girdiðinde çalýþýr
        if (!oyunBittiMi)
        {
            KovaGirme.transform.position = topPos;
            KovaGirme.gameObject.SetActive(true);
            KovaGirme.Play();
            girenTopSayisi++;
            Sesler[0].Play();
            _TopAtar.DevamEt();
            _CizgiCiz.DevamEt();
        }       
    }
    public void OyunBitti()
    {
        oyunBittiMi = true;
        Paneller[2].SetActive(false);
        if (!sesCaldimi)
        {
            Sesler[1].Play();
            sesCaldimi = true;
        }      
        Paneller[1].SetActive(true);       
        if (girenTopSayisi > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", girenTopSayisi);
            BestScoreGecis.gameObject.SetActive(true);
            BestScoreGecis.Play();
        }
        ScoreTextleri[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        ScoreTextleri[2].text = girenTopSayisi.ToString();
        _TopAtar.TopAtmaDurdur();
        _CizgiCiz.CizmeyiDurdur();
        //Time.timeScale = 0;
    }
    public void OyunuBaslat()
    {
        Paneller[0].SetActive(false);
        _TopAtar.OyunuBaslat();
        _CizgiCiz.CizmeyiBaslat();
        Paneller[2].SetActive(true);
    }
    public void TekrarOyna()
    {//Sahneyi Tekrar Yükler
       // Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void OyundanCik()
    {
        Application.Quit();
    }
}
