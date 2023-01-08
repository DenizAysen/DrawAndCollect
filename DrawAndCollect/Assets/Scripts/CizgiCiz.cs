using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CizgiCiz : MonoBehaviour
{
    public GameObject LinePrefab;
    public GameObject Cizgi;

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider2D;
    public List<Vector2> ParmakPozisyonListesi;
    bool CizmekMumkunmu;
    public List<GameObject> Cizgiler;
    int cizmeHakki;
    [SerializeField] private TextMeshProUGUI cizmeHakkiText;
    private void Start()
    {
        CizmekMumkunmu = false;
        cizmeHakki = 3;
        cizmeHakkiText.text = cizmeHakki.ToString();
    }
    void Update()
    {
        if(CizmekMumkunmu && cizmeHakki != 0)
        {
            if (Input.GetMouseButtonDown(0))//Fareye Basýlmýþsa
            {
                CizgiOlustur();
            }
            if (Input.GetMouseButton(0))//Fareye basýlý tutuluyorsa
            {
                Vector2 ParmakPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(ParmakPos, ParmakPozisyonListesi[ParmakPozisyonListesi.Count - 1]) > .1f)
                {
                    CizgiyiGuncelle(ParmakPos);
                }
            }
        }
        //Eger oyuncu ekrana cizgi cizmisse ve cizme hakký 0 degilse
        //cizmeHakki 1 azalýr
        if (Cizgiler.Count != 0 && cizmeHakki != 0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                cizmeHakki--;
                cizmeHakkiText.text = cizmeHakki.ToString();
            }
        }
    }
    void CizgiOlustur()
    {
        //(0,0) pozisyonunda çizgi oluþturur
        Cizgi = Instantiate(LinePrefab, Vector2.zero, Quaternion.identity);
        Cizgiler.Add(Cizgi);
        lineRenderer = Cizgi.GetComponent<LineRenderer>();
        edgeCollider2D = Cizgi.GetComponent<EdgeCollider2D>();
        //Her çizgide bu liste kullanýlacaðý için listeyi her çalýþmasýnda temizler.
        ParmakPozisyonListesi.Clear();
        ParmakPozisyonListesi.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        ParmakPozisyonListesi.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, ParmakPozisyonListesi[0]);//Çizginin baþlangýcý
        lineRenderer.SetPosition(1, ParmakPozisyonListesi[1]);//Çizginin bitiþi
        /*En sonda edgecollidera koordinat eklenir. 
         * Böylece edgecollider gönderilen koordinata kadar uzar*/
        edgeCollider2D.points = ParmakPozisyonListesi.ToArray();        
    }
    void CizgiyiGuncelle(Vector2 GelenParmakPos)
    {//Pos = Position
        ParmakPozisyonListesi.Add(GelenParmakPos);
        lineRenderer.positionCount++;
        //lineRendererdaki pozisyon arrayinin son indisine GelenParmakPosu ekler.
        lineRenderer.SetPosition(lineRenderer.positionCount-1,GelenParmakPos);
        edgeCollider2D.points = ParmakPozisyonListesi.ToArray();
    }
    public void DevamEt()
    { //Metod Sahnedeki Cizgileri siler ve listeyi temizler
        if (TopAtar.atilanTopSayisi == 0)
        {          
            foreach (var item in Cizgiler)
            {
                Destroy(item.gameObject);
            }
            Cizgiler.Clear();
            cizmeHakki = 3;
            cizmeHakkiText.text = cizmeHakki.ToString();
        }
        
    }
    public void CizmeyiDurdur()
    {
        CizmekMumkunmu = false;
    }
    public void CizmeyiBaslat()
    {
        CizmekMumkunmu = true;
        cizmeHakki = 3;
    }
}
