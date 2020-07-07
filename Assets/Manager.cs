using System.Collections;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Sprite[] spriteList;
    public Button[] skinButtons;

    bool control = false; //false olarak başlatalım ki butona tıklamadan önce karta tıklanırsa açılmasın.
    int index = 0; // bu değer geçerli buton indeksidir.

    Color firstColor; //Bu renk hareketli frame rengi.
    Color secondColor; 

    public float speed;

    void Start()
    {
        firstColor = Color.green; //dolaşan frame rengi
        secondColor = Color.grey; //sabit frame rengi
    }

    void Update()
    {
        if(control)
        {
            buttonsInteractable();
            control = false;
        }
    }
    // Bu fonksiyon Unlock Button' a tıklandığında çalışır. Diğer fonksiyonları harekete geçirir. 
    public void UnlockButton()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            skinButtons[i].interactable = false; // Başlangıçta bütün butonları işlevsiz hale getirmeliyiz.
            skinButtons[i].GetComponent<Image>().color = secondColor; // hepsinin rengi gri olarak ayarlanır.
            skinButtons[i].GetComponent<Image>().sprite = null; //aynı şekilde hepsinin sprite'ı başlangıçta null'dur.
        }
        UnityEventTools.RemovePersistentListener(skinButtons[index].onClick, ActivateButton);
        skinButtons[index].transform.GetChild(0).gameObject.SetActive(true);
        skinButtons[index].transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(enumerator(speed)); // Random değer üretip yapılan işlemleri burada başlatırız.
    }
    //random olarak seçilen butonun kullanımına izin vermek ve o butonun frame'ini görünür yapmak için
    void buttonsInteractable()
    {
        skinButtons[index].interactable = true; // butonun kullanımına izin verir.
        skinButtons[index].GetComponent<Image>().color = secondColor;
        skinButtons[index].GetComponent<Image>().sprite = spriteList[index]; // butonun sprite'ı listeden aynı indekse sahip sprite olarak ayarlanır.
        skinButtons[index].onClick.AddListener(() => ActivateButton());
    }

    //Butonu görmek için açmamızı sağlayan fonksiyon
    void ActivateButton()
    {
        skinButtons[index].transform.GetChild(0).gameObject.SetActive(false);
        skinButtons[index].transform.GetChild(1).gameObject.SetActive(false);
    }

    //0-9 arası random değer oluşturarak bu index değerine sahip butonlar yeşile boyanarak gezilir.
    IEnumerator enumerator(float speed)
    {
        for (int i = 0; i < 20; i++)
        {
            skinButtons[index].GetComponent<Image>().color = secondColor; // Her yeni renklendirmede önceki renk eski rengine boyanır.
            index = Random.Range(0, 9);
            skinButtons[index].GetComponent<Image>().color = firstColor;
            yield return new WaitForSeconds(speed); 
            speed += 0.02f; // Sona doğru işlemin yavaşlaması efekti için beklenen süre arttırılır.
        }
        control = true;
    }
}
