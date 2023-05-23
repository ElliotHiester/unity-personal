using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Color greyedHeart;
    [SerializeField] private Color redHeart;

    [SerializeField] private GameObject reloadBarBG;
    [SerializeField] private GameObject reloadBar;

    [SerializeField] private GameObject comboBarBG;
    [SerializeField] private GameObject comboBar;

    public TextMeshProUGUI maxAmmoDisplay;
    public Slider clipAmmoSlider;

    List<GameObject> heartObjects = new List<GameObject>();
    private RectTransform heartTransform;
    private PlayerController playerController = null;

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    public GameObject gameOverBG;
    public GameObject generatingScreen;

    private void Start()
    {
        generatingScreen.SetActive(true);
    }
    void Update()
    {
        playerController = playerController != null ? playerController : GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();

        if (playerController is not null && heartObjects.Count == 0)
        {
            UpdateHearts();
        }
    }

    public void Reload(float time)
    {
        reloadBarBG.SetActive(true);
        LeanTween.scaleX(reloadBar, 0, time).setOnComplete(ReloadCallBack);
    }

    private void ReloadCallBack()
    {
        reloadBarBG.SetActive(false);
        LeanTween.scaleX(reloadBar, 1, 0);
    }

    public void StopReload()
    {
        LeanTween.cancel(reloadBar);
        reloadBarBG.SetActive(false);
        LeanTween.scaleX(reloadBar, 1, 0);
    }

    public void Combo(float time)
    {
        LeanTween.cancel(comboBar);
        LeanTween.scaleX(comboBar, 1, 0);
        comboBarBG.SetActive(true);
        LeanTween.scaleX(comboBar, 0, time).setOnComplete(ComboCallBack);
    }

    private void ComboCallBack()
    {
        comboBarBG.SetActive(false);
        LeanTween.scaleX(comboBar, 1, 0);
    }

    public void UpdateHearts()
    {
        GameObject heart;

        if (playerController is null) return;

        currentHealth = playerController.health;
        maxHealth = playerController.maxHealth;

        if (heartObjects.Count == 0) //Generates hearts if there arent any yet
        {
            for (int i = 0; i < maxHealth; i++)
            {
                heart = Instantiate(heartPrefab, canvas.transform);
                heartObjects.Add(heart);
                heartTransform = heart.GetComponent<RectTransform>();

                float heartX = ((heartObjects.Count - 1) * heartTransform.sizeDelta.x) + 35; //linear function to calculate the position of the heart

                heartTransform.anchoredPosition = new Vector3(heartX, -25, 0);
            }
        }

        if (currentHealth < 0)
            return;

        for (int i = 0; i < maxHealth; i++)
        {
            heart = heartObjects[i];
            heart.GetComponent<Image>().color = i < currentHealth ? redHeart : greyedHeart;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
