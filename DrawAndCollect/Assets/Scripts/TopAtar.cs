using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopAtar : MonoBehaviour
{
    [SerializeField] private GameObject[] Toplar;
    [SerializeField] private GameObject TopAtarMerkezi;
    [SerializeField] private GameObject Kova;
    [SerializeField] private GameObject[] Kova_Noktalari;
    int aktifTopIndex;
    int RandomKovaPointIndex;
    public static int atilanTopSayisi;
    public static int TopAtisSayisi;
    bool Kilit;
    private void Start()
    {
        TopAtisSayisi = 0;
        atilanTopSayisi = 0;
    }
    IEnumerator TopAtisSistemi()
    {
        while (true)
        {
            if (!Kilit)
            {//Kilit kapalýysa bu kod bloðu çalýþýr
                yield return new WaitForSeconds(.5f);
                //TopAtisSayisi 5 veya 5 in katýysa 2 top atýlýr
                if (TopAtisSayisi % 5 == 0 && TopAtisSayisi != 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        TopAtisVeAyarlama();
                    }                    
                    atilanTopSayisi = 2;
                    TopAtisSayisi++;
                }
                else
                {
                    TopAtisVeAyarlama();
                    atilanTopSayisi = 1;
                    TopAtisSayisi++;
                }             
                yield return new WaitForSeconds(.7f);
                //Sahnedeki kova koordinatlarýndan biri seçilir
                //Kova sahnede aktif olduktan 5 saniye sonra TopuKontrolEt metodu çaðýrýlýr
                RandomKovaPointIndex = Random.Range(0, Kova_Noktalari.Length - 1);
                Kova.transform.position = Kova_Noktalari[RandomKovaPointIndex].transform.position;
                Kova.SetActive(true);
                Kilit = true;
                Invoke("TopuKontrolEt", 5f);
            }
            else
            {
                yield return null;
            }
        }
    }
    float Aciver(float deger1, float deger2)
    {
        return Random.Range(deger1, deger2);
    }
    Vector3 PozisyonVer(float gelenAci)
    {
        return Quaternion.AngleAxis(gelenAci, Vector3.forward) * Vector3.right;
    }
    public void DevamEt()
    {
        if(atilanTopSayisi == 1)
        {//Eðer top kovaya atýlmýþsa TopuKontrolEt metodu çaðýrýlmaz
            Kilit = false;
            Kova.SetActive(false);
            CancelInvoke("TopuKontrolEt");
            atilanTopSayisi--;
        }
        else
        {
            atilanTopSayisi--;
        }
    }
    public void OyunuBaslat()
    {
        StartCoroutine(TopAtisSistemi());
    }
    public void TopAtmaDurdur()
    {
        StopAllCoroutines();
        CancelInvoke("TopuKontrolEt");
    }
    void TopuKontrolEt()
    {
        if (Kilit)
        {//Eðer sahnede aktif top varsa oyun biter
            GetComponent<GameManager>().OyunBitti();
        }
    }
    void TopAtisVeAyarlama()
    {
        Toplar[aktifTopIndex].transform.position = TopAtarMerkezi.transform.position;
        Toplar[aktifTopIndex].SetActive(true);
        Toplar[aktifTopIndex].GetComponent<Rigidbody2D>().AddForce(PozisyonVer(Aciver(70f, 110f)) * 700);
        if (aktifTopIndex != Toplar.Length - 1)
        {
            aktifTopIndex++;
        }
        else
        {
            aktifTopIndex = 0;
        }
    }
}
