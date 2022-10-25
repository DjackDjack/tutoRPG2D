using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSO> inventory;
    public int inventoryLenght = 63;
    public GameObject inventoryPanel, holderSlot;
    private GameObject slot;
    public GameObject prefabs;

    public TextMeshProUGUI title, descriptionObject;
    public Image iconDescription;

    [Header("Description")]
    public GameObject holderDescription;
    private int amountToUse;
    [SerializeField] private TextMeshProUGUI valueToUse;
    [SerializeField] private Button plusButton, lessButton;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject removeButton;
    [SerializeField] private GameObject amountToRemove;



    public static InventoryManager instance;

    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && !inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(true);
            RefreshInventory();
            
        }
        else if(Input.GetKeyDown(KeyCode.I) && inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(false);
        }

    }

    public void ChargeItem(int i)
    {
        if(i < inventory.Count)
        {
            int amountToUse = 0;
            valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;

            if(inventory[i].type == ItemSO.Type.Consommable)
            {
                useButton.SetActive(true);
                removeButton.SetActive(true);
                amountToRemove.SetActive(true);
            }
            else if(inventory[i].type == ItemSO.Type.Quest)
            {
                useButton.SetActive(false);
                removeButton.SetActive(false);
                amountToRemove.SetActive(false);
            }
            else if(inventory[i].type == ItemSO.Type.Commun)
            {
                useButton.SetActive(false);
                removeButton.SetActive(true);
                amountToRemove.SetActive(true);
            }

            holderDescription.SetActive(true);
            title.text = inventory[i].title;
            descriptionObject.text = inventory[i].description;
            iconDescription.sprite = inventory[i].icon;

            plusButton.GetComponent<Button>().onClick.RemoveAllListeners();
            plusButton.GetComponent<Button>().onClick.AddListener(delegate { PlusButton(i); });
            
            lessButton.GetComponent<Button>().onClick.RemoveAllListeners();
            lessButton.GetComponent<Button>().onClick.AddListener(delegate { LessButton(i); });

            useButton.GetComponent<Button>().onClick.RemoveAllListeners();
            useButton.GetComponent<Button>().onClick.AddListener(delegate { UseItem(i); });

            removeButton.GetComponent<Button>().onClick.RemoveAllListeners();
            removeButton.GetComponent<Button>().onClick.AddListener(delegate { RemoveItem(i); });
        }
        else
        {
            Debug.Log("emplacement vide");
        }

    }

    public void UseItem(int i)
    {
        for(int y = 0; y < amountToUse; y++)
        {
            PlayerController.instance.currentHealth += inventory[i].amountToHeal;

            //supprime l'item si l'on supprime le dernier (retour à une case vide dans l'inventaire)
            if(inventory[i].amount == 1)
            {
                inventory.Remove(inventory[i]);
                holderDescription.SetActive(false);
                amountToUse = 0;
                break;
            }
            else
            {
                inventory[i].amount --;
            }
            amountToUse = 0;
        }
        RefreshInventory();
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;   
    }

    public void RemoveItem(int i)
    {
        for(int y = 0; y < amountToUse; y++)
        {
            //supprime l'item si l'on supprime le dernier (retour à une case vide dans l'inventaire)
            if(inventory[i].amount <= 1)
            {
                inventory.Remove(inventory[i]);
                holderDescription.SetActive(false);
                break;
            }
            else
            {
                inventory[i].amount --;
            }
            amountToUse = 0;
        }
        RefreshInventory();
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;

    }

    public void PlusButton(int i)
    {
        if(amountToUse <= inventory[i].amount - 1)
            amountToUse ++;
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;
    }

    public void LessButton(int i)
    {
        if(amountToUse > 0)
            amountToUse --;
        valueToUse.text = amountToUse + "/" + inventory[i].maxAmount;
    }

    private void RefreshInventory()
    {
        if(holderSlot.transform.childCount > 0)
        {
            foreach (Transform item in holderSlot.transform)
            {
                Destroy(item.gameObject);
            }
        }

        for (int i = 0; i < inventoryLenght; i++)
        {

            if(i <= inventory.Count -1)
            {
                slot = Instantiate(prefabs, transform.position, transform.rotation);
                slot.transform.SetParent(holderSlot.transform);
                TextMeshProUGUI amount = slot.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                Image img = slot.transform.Find("Image").GetComponent<Image>();
                slot.GetComponent<SlotItem>().itemSlot = i;
                amount.text = inventory[i].amount.ToString();
                img.sprite = inventory[i].icon;
            }
            else if( i > inventory.Count - 1)
            {
                slot = Instantiate(prefabs, transform.position, transform.rotation);
                slot.transform.SetParent(holderSlot.transform);
                slot.GetComponent<SlotItem>().itemSlot = i;
                TextMeshProUGUI amount = slot.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                Image img = slot.transform.Find("Image").GetComponent<Image>();
                amount.gameObject.SetActive(false);
            }
        }
    }

}


