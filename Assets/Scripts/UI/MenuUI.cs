using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private InputField levelInput;
    [SerializeField] private InputField dmgInput;
    [SerializeField] private InputField defInput;
    [SerializeField] private InputField agiInput;
    [SerializeField] private InputField luckInput;
    [SerializeField] private InputField HPInput;
    [SerializeField] private Unit unit;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Toggle customToggle;
    [SerializeField] private GameObject transition;

    [SerializeField] private int pointer;

    private Scene currentScene;
    private string sceneName;

    private void Start()
    {
        FindObjectOfType<AudioManager>().StopMusic("battle");
        UpdateMenuSelection();
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }
    private void Update()
    {
        if (sceneName == "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                pointer--;
                UpdateMenuSelection();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                pointer++;
                UpdateMenuSelection();
            }

            if (pointer < 0)
            {
                pointer = 2;
                UpdateMenuSelection();
            }

            if (pointer > 2)
            {
                pointer = 0;
                UpdateMenuSelection();
            }

            // set the other inputs as not interactable if toggle is on
            if (!customToggle.isOn)
            {
                levelInput.interactable = false;
                dmgInput.interactable = false;
                defInput.interactable = false;
                agiInput.interactable = false;
                luckInput.interactable = false;
                HPInput.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                levelInput.interactable = true;
                dmgInput.interactable = true;
                defInput.interactable = true;
                agiInput.interactable = true;
                luckInput.interactable = true;
                HPInput.interactable = true;
                nameInput.interactable = true;
            }
        }
    }

    private void SaveValuesToFile()
    {
        // set values in inputfields to unit
        unit.level = Int32.Parse(levelInput.text);
        unit.baseDamage = Int32.Parse(dmgInput.text);
        unit.baseDefence = Int32.Parse(defInput.text);
        unit.baseAgility = Int32.Parse(agiInput.text);
        unit.luck = Int32.Parse(luckInput.text);
        unit.maxHP = Int32.Parse(HPInput.text);
        unit.currentHP = Int32.Parse(HPInput.text);

        // save unit data to file
        SaveSystem.SavePlayer(unit);
    }

    public void ButtonPressed()
    {
        SaveValuesToFile();

        StartCoroutine(InitiateBattle());
    }

    public void GameOverButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateMenuSelection()
    {
        switch (pointer)
        {
            case 0:
                nameInput.text = "Glass Cannon";
                levelInput.text = "1";
                dmgInput.text = "30";
                defInput.text = "5";
                agiInput.text = "25";
                luckInput.text = "3";
                HPInput.text = "180";
                break;
            case 1:
                nameInput.text = "Tank";
                levelInput.text = "1";
                dmgInput.text = "10";
                defInput.text = "25";
                agiInput.text = "5";
                luckInput.text = "2";
                HPInput.text = "620";
                break;
            case 2:
                nameInput.text = "Luckster";
                levelInput.text = "1";
                dmgInput.text = "12";
                defInput.text = "10";
                agiInput.text = "8";
                luckInput.text = "22";
                HPInput.text = "230";
                break;

        }
    }

    private IEnumerator InitiateBattle()
    {
        transition.SetActive(true);

        FindObjectOfType<AudioManager>().Play("encounter");

        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene("BattleScene");
    }
}
