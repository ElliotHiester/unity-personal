using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Color greyedHeart;
    [SerializeField] private Color redHeart;

    List<GameObject> heartObjects = new List<GameObject>();
    private GameObject heart;
    private RectTransform heartTransform;
    private PlayerController playerController = null;

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    void Update()
    {
        playerController ??= GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();

        if(playerController is not null && heartObjects.Count == 0)
        {
            UpdateHearts();
        }
    }

    public void UpdateHearts()
    {
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

                float heartX = ((heartObjects.Count - 1) * heartTransform.sizeDelta.x) + 65; //linear function to calculate the position of the heart

                heartTransform.anchoredPosition = new Vector3(heartX, -55, 0);
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
}
