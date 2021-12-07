using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Global UI Parent")]
    [SerializeField] GameObject setupUI;
    [SerializeField] GameObject preparationUI;
    [SerializeField] GameObject fightUI;

    [Header("Specific UI Element")]
    [SerializeField] Text floorText;
    [SerializeField] GameObject doorMissingText;
    [SerializeField] GameObject heroMissingText;
    [SerializeField] Text VirtualCountText;
    [SerializeField] Button VirtualAddButton;

    public void UpdateUI()
    {
        switch(GameManager.GetState())
        {
            case GameManager.GameState.Setup:
                setupUI.SetActive(true);
                preparationUI.SetActive(false);
                fightUI.SetActive(false);
                break;

            case GameManager.GameState.Preparation:
                setupUI.SetActive(false);
                preparationUI.SetActive(true);
                fightUI.SetActive(false);
                UpdateVirtualUI();
                break;

            case GameManager.GameState.Fight:
                setupUI.SetActive(false);
                preparationUI.SetActive(false);
                fightUI.SetActive(true);
                break;
        }
        UpdateFloorText();
    }
    private void UpdateFloorText()
    {
        int mF = GameManager.GetMaxFloor();
        floorText.text = GameManager.GetCurrentFloor() + "/" + (mF > 0 ? mF.ToString() : "-");
    }




    public void StartGame()
    {
        doorMissingText.SetActive(!GameManager.TryEndSetUpPhase());
    }
    public void AddFloor()
    {
        GameManager.IncreaseMaxFloor();
        UpdateFloorText();
    }
    public void RemoveFloor()
    {
        GameManager.DecreaseMaxFloor();
        UpdateFloorText();
    }



    public void StartFight()
    {
        int result = GameManager.StartFightPhase();
        doorMissingText.SetActive(result == 1);
        heroMissingText.SetActive(result == 2);
    }
    public void SpawnVirtualHuman()
    {
        GameManager.SpawnVirtualHero();
        UpdateVirtualUI();
    }
    private void UpdateVirtualUI()
    {
        int count = GameManager.GetVirtualReserveCount();
        VirtualCountText.text = count.ToString();
        VirtualAddButton.interactable = count != 0;
    }
}
