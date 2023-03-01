using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(LoadSaveSystem))]
public class GameManager : MonoBehaviour
{
    [Header("Profile Name")]
    public static string profileName = "Default";

    [Header("Tick Rate")]
    public static int ticksPerSecond = 30;

    [SerializeField] private List<IridiumGenerator> ownedStructures = new List<IridiumGenerator>();

    [Header("Balancing")]
    [SerializeField] private float clickUpgradePriceMultiplier = 1.2f;
    [SerializeField] private float iridiumPerClickPercent = 1;
    [SerializeField] private float upgradeClick_BaseCost = 1000;
    private float upgradeClick_CurrentCost = 1000;

    [Header("Buttons")]
    [SerializeField] private Button getIridiumButton;
    [SerializeField] private Button upgradeClick_Button;
    [SerializeField] private GameObject structureButtonParent;
    [SerializeField] private GameObject structureButtonPrefab;
    private TMP_Text upgradeClick_ButtonText;

    private List<Button> structureButtons;
    private List<TMP_Text> structureNameTexts;
    private List<TMP_Text> structureCostTexts;
    private List<TMP_Text> structureOwnedTexts;

    [Space(10)]

    [Header("Texts")]
    [SerializeField] private TMP_Text totalIridiumText;
    [SerializeField] private TMP_Text iridiumPerSecondText;
    [SerializeField] private TMP_Text iridiumPerClickText;

    private bool initializedUI = false;
    private bool iridiumClicked = false;
    private float totalIridium = 0;
    private float iridiumPerSecond = 0;
    private string firstLaunchPlayerPref = "FirstLaunch";

    private Coroutine tickCoroutine;
    private WaitForSeconds tickWait;

    private LoadSaveSystem loadSaveSystem;

    #region Utility Functions

    private void Awake()
    {
        loadSaveSystem = GetComponent<LoadSaveSystem>();
    }

    private void Start()
    {
        StartGame();
    }

    private void OnDestroy()
    {
        SaveGame();
    }

    private void StartGame()
    {
        if (PlayerPrefs.GetInt(firstLaunchPlayerPref, 0) == 0)
        {
            LoadStructureSOs(); //Load structure Scriptable Objects and put save data into them

            SaveGame(); //Save the game

            PlayerPrefs.SetInt(firstLaunchPlayerPref, 1);
        }
        else
        {
            LoadGame(); // Load the save data

            UpdateIridiumPerSecond(); //Update the iridium per second
        }

        InitializeUI(); //Initialize all UI Variables

        CalculateCosts(); //Calculate all upgrade costs

        UpdateAllUI(); //Update all UI

        SetupCoroutine(); //Setup the coroutine for the tick rate
    }

    private void SetupCoroutine()
    {
        if (tickCoroutine != null)
            StopCoroutine(tickCoroutine);

        tickWait = new WaitForSeconds(1f / ticksPerSecond);
        tickCoroutine = StartCoroutine(Tick());
    }

    private void LoadStructureSOs()
    {
        var os = Resources.LoadAll("IridiumGenerators", typeof(IridiumGeneratorSO));

        foreach (var o in os)
        {
            IridiumGenerator structure = new((IridiumGeneratorSO)o);
            ownedStructures.Add(structure);
        }

        UpdateIridiumPerSecond();
    }

    private void InitializeUI()
    {
        getIridiumButton.onClick.AddListener(GetIridiumClicked);
        upgradeClick_Button.onClick.AddListener(UpgradeClickClicked);
        upgradeClick_ButtonText = upgradeClick_Button.transform.GetChild(0).GetComponent<TMP_Text>();

        structureButtons = new List<Button>();
        structureNameTexts = new List<TMP_Text>();
        structureCostTexts = new List<TMP_Text>();
        structureOwnedTexts = new List<TMP_Text>();

        for (int i = 0; i < ownedStructures.Count; i++)
        {
            int j = i;
            GameObject obj = Instantiate(structureButtonPrefab, structureButtonParent.transform);
            obj.name = ownedStructures[i].structureName + " Button";
            structureButtons.Add(obj.GetComponent<Button>());
            structureNameTexts.Add(obj.transform.GetChild(0).GetComponent<TMP_Text>());
            structureCostTexts.Add(obj.transform.GetChild(1).GetComponent<TMP_Text>());
            structureOwnedTexts.Add(obj.transform.GetChild(2).GetComponent<TMP_Text>());

            structureButtons[i].onClick.AddListener(() => StructureBuyClicked(j));
        }

        initializedUI = true;
    }

    private void CalculateCosts()
    {
        upgradeClick_CurrentCost = (int)(upgradeClick_BaseCost * Mathf.Pow(clickUpgradePriceMultiplier, iridiumPerClickPercent - 1));

        for (int i = 0; i < ownedStructures.Count; i++)
        {
            ownedStructures[i].structureCurrentCost = (int)(ownedStructures[i].structureBaseCost * Mathf.Pow(ownedStructures[i].structureCostMultiplier, ownedStructures[i].structureOwned));
        }
    }

    private void UpdateAllUI()
    {
        if (!initializedUI) return;

        iridiumPerClickText.text = iridiumPerClickPercent.ToString("0") + " % of Iridium/Sec";
        iridiumPerSecondText.text = iridiumPerSecond.ToString("000.00") + " Iridium/s";
        totalIridiumText.text = totalIridium.ToString("0") + " Iridium";

        for (int i = 0; i < ownedStructures.Count; i++)
        {
            structureNameTexts[i].text = ownedStructures[i].structureName;
            structureCostTexts[i].text = "$" + ownedStructures[i].structureCurrentCost.ToString();
            structureOwnedTexts[i].text = ownedStructures[i].structureOwned.ToString();
        }

        upgradeClick_ButtonText.text = "Upgrade Click ($" + upgradeClick_CurrentCost.ToString() + ")";
    }

    private void UpdateIridiumPerSecond()
    {
        iridiumPerSecond = 0;

        for (int i = 0; i < ownedStructures.Count; i++)
        {
            iridiumPerSecond += ownedStructures[i].GetIridiumPerTick() * ticksPerSecond;
        }
    }

    #endregion

    private IEnumerator Tick()
    {
        while (true)
        {
            ProcessIridiumAdded();
            UpdateAllUI();
            yield return tickWait;
        }
    }

    private void ProcessIridiumAdded()
    {
        ProcessIridiumPerStructure();

        if (iridiumClicked)
        {
            ProcessClickedIridium();
            iridiumClicked = false;
        }
    }

    #region Iridium Processors

    private void ProcessClickedIridium()
    {
        float iridiumToAdd = Mathf.Max(1, iridiumPerSecond * iridiumPerClickPercent / 100f);
        totalIridium += iridiumToAdd;
        totalIridiumText.text = totalIridium.ToString() + " Iridium";
    }

    private void ProcessIridiumPerStructure()
    {
        if (ownedStructures.Count > 0)
        {
            foreach (IridiumGenerator structure in ownedStructures)
            {
                float x = structure.GetIridiumPerTick();
                totalIridium += x;
            }
        }
    }

    #endregion

    #region Button Callbacks
    private void GetIridiumClicked()
    {
        iridiumClicked = true;
    }

    private void UpgradeClickClicked()
    {
        if (totalIridium >= upgradeClick_CurrentCost)
        {
            totalIridium -= upgradeClick_CurrentCost;
            upgradeClick_CurrentCost = (int)(upgradeClick_CurrentCost * clickUpgradePriceMultiplier);
            iridiumPerClickPercent += 1;
            iridiumPerClickText.text = iridiumPerClickPercent.ToString() + " % of Iridium/Sec";
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
            upgradeClick_ButtonText.text = "Upgrade Click ($" + upgradeClick_CurrentCost.ToString() + ")";
        }
    }

    private void StructureBuyClicked(int structureIndex)
    {
        if (totalIridium >= ownedStructures[structureIndex].structureCurrentCost)
        {
            totalIridium -= ownedStructures[structureIndex].structureCurrentCost;
            ownedStructures[structureIndex].structureOwned += 1;
            ownedStructures[structureIndex].structureCurrentCost = (int)(ownedStructures[structureIndex].structureCurrentCost * ownedStructures[structureIndex].structureCostMultiplier);
            structureOwnedTexts[structureIndex].text = ownedStructures[structureIndex].structureOwned.ToString();
            structureCostTexts[structureIndex].text = ownedStructures[structureIndex].structureCurrentCost.ToString();
            totalIridiumText.text = totalIridium.ToString() + " Iridium";
        }

        UpdateIridiumPerSecond();
    }

    #endregion

    #region Save, Load and Reset

    public void SaveGame()
    {
        SaveData saveData = GetSaveData();
        loadSaveSystem.Save(saveData);
    }

    public SaveData GetSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.profileName = profileName;
        saveData.totalIridium = totalIridium;
        saveData.iridiumPerClickPercent = iridiumPerClickPercent;
        saveData.upgradeClick_BaseCost = upgradeClick_BaseCost;
        saveData.ownedStructures = ownedStructures;

        return saveData;
    }
    public void LoadGame()
    {
        SaveData saveData = loadSaveSystem.Load();

        profileName = saveData.profileName;
        totalIridium = saveData.totalIridium;
        iridiumPerClickPercent = saveData.iridiumPerClickPercent;
        upgradeClick_BaseCost = saveData.upgradeClick_BaseCost;
        ownedStructures = saveData.ownedStructures;

        SetupCoroutine();
        UpdateAllUI();
    }

    void Reset()
    {
        totalIridium = 0;
        iridiumPerClickPercent = 1;
        upgradeClick_CurrentCost = upgradeClick_BaseCost;
        ownedStructures = new List<IridiumGenerator>();
    }

    #endregion
}