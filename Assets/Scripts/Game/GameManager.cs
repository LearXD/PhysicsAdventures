using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;

    private Player player;

    public GameObject pausePanel;
    public GameObject dialogPanel;
    public GameObject respawnPanel;

    private DialogManager dialogManager;

    void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        this.pausePanel = GameObject.Find("PausePanel");
        this.pausePanel.SetActive(false);

        this.dialogPanel = GameObject.Find("DialogPanel");
        this.dialogPanel.SetActive(false);

        this.respawnPanel = GameObject.Find("RespawnPanel");
        this.respawnPanel.SetActive(false);

        this.dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        
        this.dialogManager.SetWriteSpeed(0.01f);
        this.player.init();  
    }

    
    public bool IsPaused() => this.paused;

    public Player GetPlayer() => this.player;

    public DialogManager GetDialogManager() => this.dialogManager;

    public void PauseButton () {
        this.Pause();
    }

    public void ConfigurationsButton() {
    }

    public void GoBackToMenuButton() {
        this.GoBackToMenu();
    }

    public void GoBackToMenu () {
        SceneManager.LoadScene("MenuScene");
        if(this.paused) this.Pause();
    }

    public void Pause (bool showScreen = true) {
        if(player.IsAlive()) {
            this.paused = !this.paused; 
            Time.timeScale = this.paused ? 0 : 1;
            if(showScreen) {
                this.pausePanel.SetActive(this.IsPaused());
            }
        }
    }

    void Update () {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            this.Pause();
        }
    }
}
