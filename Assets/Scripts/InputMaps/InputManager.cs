using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMap inputMap;

    private void Awake()
    {
        inputMap = new InputMap();
    }
    private void OnEnable()
    {
        inputMap.Enable();
    }
    private void OnDisable()
    {
        inputMap.Disable();
    }
    public Vector2 GetMousePosition()
    {
        return inputMap.Land.MousePosition.ReadValue<Vector2>();
    }
    public Vector2 GetMovementInput()
    {
        Vector2 movement;
        if (inputMap.Land.Move.ReadValue<Vector2>() == null)
        {
            movement = new Vector2(0, 0);
        }
        else
        {
            movement = inputMap.Land.Move.ReadValue<Vector2>();
        }
        return movement;
    }
    public bool GetRunningInput()
    {
        if (inputMap.Land.Run.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetShootingWaterGunInput()
    {
        if (inputMap.Land.ShootWaterGun.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetShootingNormalGunInput()
    {
        if (inputMap.Land.ShootNormalGun.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetWateringInput()
    {
        if (inputMap.Land.WaterPlants.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetRefillInput()
    {
        if (inputMap.Land.RefillWaterGun.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetGlobalPlantInput()
    {
        if (inputMap.Land.PlantAllSeeds.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetWaterGunActiveInput()
    {
        if (inputMap.Land.ActivateWaterGun.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetNormalGunActiveInput()
    {
        if (inputMap.Land.ActivateNormalGun.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetPasuseScreenInput()
    {
        if (inputMap.GameManager.PauseScreen.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetInventoryScreenToggle()
    {
        if (inputMap.Land.ToggleInventory.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SavePlayerInventory()
    {
        if (inputMap.InventorySaveLoad.SaveInventory.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool LoadPlayerInventory()
    {
        if (inputMap.InventorySaveLoad.LoadInventory.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetConversationContinueInput()
    {
        if (inputMap.NPCInteractions.ContinueDialogue.triggered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DissableAllLandActions()
    {
        inputMap.Land.Disable();
    }
    public void EnableAllLandActions()
    {
        inputMap.Land.Enable();
    }
}