using UnityEngine;

public class ShipBarsManager : MonoBehaviour
{
    public static ShipBarsManager Instance; // Singleton instance of ShipBarsManager
    public ProgressBar shipHealthBar; // Reference to the ship health bar progress bar
    public ProgressBar shipShieldBar; // Reference to the ship shield bar progress bar

    private GameObject shieldInstance; // Reference to the spawned shield instance
    private ShieldBehavior shieldBehavior; // Reference to the ShieldBehavior component of the shield instance

    private bool hasShield = false; // Flag to track if the ship has a shield

    private void Awake()
    {
        // Ensure only one instance of ShipBarsManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeBars();
        Debug.Log("ShipBarsManager Initialized");
    }

    // Method to initialize bars to default values
    private void InitializeBars()
    {
        UpdateHealthBar(1f); // Set health bar to 100%
        if (!hasShield) // Only reset the shield bar if no shield is present
        {
            UpdateShieldBar(0f); // Set shield bar to 0% initially
            Debug.Log("Initial Shield Bar set to 0%");
        }
    }


    // Method to set the shield instance reference
    public void SetShieldInstance(GameObject shield)
    {
        shieldInstance = shield;
        shieldBehavior = shieldInstance.GetComponent<ShieldBehavior>();
        if (shieldBehavior != null)
        {
            shieldBehavior.ShieldHealthChanged += UpdateShieldBar;
            hasShield = true; // Set this flag here to ensure readiness for updates
            shieldBehavior.PickupShield(); // Explicitly call to ensure event is triggered after subscription
        }
        else
        {
            Debug.LogError("ShieldBehavior component not found on the shield instance.");
        }
    }


    // Method to update the shield health bar when the shield power-up is picked up
    public void PickupShield()
    {
        hasShield = true;
        UpdateShieldBar(1f); // This sets the shield bar to 100% initially.
        Debug.Log("Shield picked up, bar set to 100%");
    }


    // Method to update the ship health bar with the given health percentage
    public void UpdateHealthBar(float healthPercentage)
    {
        shipHealthBar.BarValue = healthPercentage * 100f; // Convert percentage to value between 0 and 100
    }

    // Method to update the ship shield bar with the given shield percentage
    public void UpdateShieldBar(float shieldPercentage)
    {
        Debug.Log("Updating Shield Bar with: " + shieldPercentage);
        if (hasShield)
        {
            shipShieldBar.BarValue = shieldPercentage * 100f; // Ensure this multiplication is correct
        }
        else
        {
            Debug.Log("Attempted to update shield bar without shield being active.");
        }
    }

}
