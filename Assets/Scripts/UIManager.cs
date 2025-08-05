using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
        
    [Header("HUD")]
    public Slider xpSlider;
    public TMP_Text levelText;
    public TMP_Text killText;
    public TMP_Text goldCountText;
    public GameObject levelMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text pauseMenuText;
    public List<Image> upgradePictures;
    [SerializeField] private GameObject playerSpecPanel;
    public List<TMP_Text> playerSpecTexts=new List<TMP_Text>();

    
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private List<Button> upgradeButtons = new List<Button>();
    private void Start()
    {
        ContinueGame();
        
        restartButton.onClick.AddListener(Restart);
        mainMenuButton.onClick.AddListener(GoBackToMenu);
        closeButton.onClick.AddListener(ClosePauseMenu);

        for (var buttonId = 0; buttonId < upgradeButtons.Count; buttonId++)
        {
            var id = buttonId;
            upgradeButtons[buttonId].onClick.AddListener(delegate { GameManager.Instance.Upgrade(id); });
        }
    }
    
    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseMenu.activeSelf)
            {
                ClosePauseMenu();
            }
            else if (!levelMenu.activeSelf)
            {
                OpenPauseMenu();
            }
        }
    }
    
    private void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        levelMenu.SetActive(false);
        ContinueGame();
    }
    
    public void OpenPauseMenu(bool isDead=false)
    {
        if (isDead)
        {
            pauseMenuText.text = "You're Dead";
            closeButton.gameObject.SetActive(false);
        }

        pauseMenu.SetActive(true);
        PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        CharacterSpecs();
    }


    private void ContinueGame()
    {
        Time.timeScale = 1;
        playerSpecPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void CharacterSpecs()
    {
        playerSpecTexts[0].text = GameManager.Instance.player.maxHealth + "";
        playerSpecTexts[1].text = GameManager.Instance.player.armor + "";
        playerSpecTexts[2].text = GameManager.Instance.player.speed + "";
        playerSpecTexts[3].text = GameManager.Instance.player.regenerate + "";
        playerSpecTexts[4].text = GameManager.Instance.player.magnet + "";
        playerSpecPanel.SetActive(true);
    }
}