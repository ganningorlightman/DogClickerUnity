using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] int _num;
    [SerializeField] int _diamond;
    [SerializeField] int _control;
    [SerializeField] int _click;
    [SerializeField] int _price;
    [SerializeField] int _auto;
    [SerializeField] int _priceAuto;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _diamondScore;
    [SerializeField] TextMeshProUGUI _diamondScoreShop;
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _priceAutoText;
    [SerializeField] TextMeshProUGUI _levelClickText;
    [SerializeField] TextMeshProUGUI _levelAutoText;
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _shopPanel;

    [SerializeField] GameObject _clickTextPrefab;
    ClickObj[] clickTextPool = new ClickObj[15];
    int clickTextNum;

    void Start()
    {
        Application.runInBackground = true;

        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("num"))
        {
            _num = PlayerPrefs.GetInt("num");
            _control = PlayerPrefs.GetInt("control");
            _click = PlayerPrefs.GetInt("click");
            _auto = PlayerPrefs.GetInt("auto");
            _diamond = PlayerPrefs.GetInt("diamond");
            _price = PlayerPrefs.GetInt("price");
            _priceAuto = PlayerPrefs.GetInt("priceAuto");
        } else
        {
            _num = 0;
            PlayerPrefs.SetInt("num", _num);
            _control = 0;
            PlayerPrefs.SetInt("control", _control);
            _click = 1;
            PlayerPrefs.SetInt("click", _click);
            _auto = 0;
            PlayerPrefs.SetInt("auto", _auto);
            _diamond = 0;
            PlayerPrefs.SetInt("diamond", _diamond);
            _price = 50;
            PlayerPrefs.SetInt("price", _price);
            _priceAuto = 50;
            PlayerPrefs.SetInt("priceAuto", _priceAuto);
        }

        _score.text = _num.ToString();
        _levelClickText.text = _click.ToString();
        _levelAutoText.text = _auto.ToString();
        _diamondScore.text = _diamond.ToString();
        _priceText.text = "Улучшить клик: " + _price;
        _priceAutoText.text = "Улучшить автоклик: " + _priceAuto;

        StartCoroutine(autoIncrement());
        for (int i = 0; i < clickTextPool.Length; i++)
        {
            clickTextPool[i] = Instantiate(_clickTextPrefab, transform).GetComponent<ClickObj>();
        }
    }

    public void OnClickDown(GameObject button)
    {
        button.transform.localScale = Vector2.one * .9f;
    }
    public void ClickButton(GameObject button)
    {
        var audioClick = button.GetComponent<AudioSource>();
        audioClick.Play();
        _num += _click;
        PlayerPrefs.SetInt("num", _num);
        _control += _click;
        PlayerPrefs.SetInt("control", _control);
        //Debug.Log(_control);
        if (_control >= 10)
        {
            _diamond += _control / 10;
            PlayerPrefs.SetInt("diamond", _diamond);
            _diamondScore.text = _diamond.ToString();
            _control = 0;
            PlayerPrefs.SetInt("control", _control);
        }
        _score.text = _num.ToString();
        clickTextPool[clickTextNum].StartMotion(_click);
        clickTextNum = clickTextNum == clickTextPool.Length - 1 ? 0 : clickTextNum + 1;
    }
    public void OnClickUp(GameObject button)
    {
        button.transform.localScale = Vector2.one;
    }
    public void ToShopPanel()
    {
        _mainPanel.SetActive(false);
        _shopPanel.SetActive(true);
        _diamondScoreShop.text = _diamond.ToString();
    }
    public void ToMainPanel()
    {
        _shopPanel.SetActive(false);
        _mainPanel.SetActive(true);
        _diamondScore.text = _diamond.ToString();
    }
    public void UpButton()
    {
        if (_diamond >= _price) {
            _click *= 2;
            PlayerPrefs.SetInt("click", _click);
            _levelClickText.text = _click.ToString();
            _diamond -= _price;
            PlayerPrefs.SetInt("diamond", _diamond);
            _diamondScoreShop.text = _diamond.ToString();
            _price *= 2;
            PlayerPrefs.SetInt("price", _price);
            _priceText.text = "Улучшить клик: " + _price;
        }
    }
    public void UpAutoButton()
    {
        if (_diamond >= _priceAuto)
        {
            _auto++;
            PlayerPrefs.SetInt("auto", _auto);
            _levelAutoText.text = _auto.ToString();
            _diamond -= _priceAuto;
            PlayerPrefs.SetInt("diamond", _diamond);
            _diamondScoreShop.text = _diamond.ToString();
            _priceAuto += 150;
            PlayerPrefs.SetInt("priceAuto", _priceAuto);
            _priceAutoText.text = "Улучшить автоклик: " + _priceAuto;
        }
    }
    private IEnumerator autoIncrement()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (_auto > 0) {
            _num += _auto;
            PlayerPrefs.SetInt("num", _num);
            _control += _auto;
            PlayerPrefs.SetInt("control", _control);
            if (_control >= 10)
            {
                _diamond += _control / 10;
                PlayerPrefs.SetInt("diamond", _diamond);
                _diamondScore.text = _diamond.ToString();
                _diamondScoreShop.text = _diamond.ToString();
                _control = 0;
                PlayerPrefs.SetInt("control", _control);
            }
            _score.text = _num.ToString();
        }

        StartCoroutine(autoIncrement());
    }
}
