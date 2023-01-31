
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private GameObject initialPanel;
    private GameObject configurationsPanel;
    private GameObject defineNicknamePanel;

    void Start() {
        this.initialPanel = GameObject.Find("InitialPanel");
        (this.configurationsPanel = GameObject.Find("ConfigurationPanel")).SetActive(false);
        (this.defineNicknamePanel = GameObject.Find("DefineYourNicknamePanel")).SetActive(false);
        if(Time.timeScale != 1) Time.timeScale = 1;
    }

    public void PlayButton () {
        if(PlayerPrefs.GetString("nickname").Length < 1) {
            this.defineNicknamePanel.SetActive(true);
            return;
        }
        this.goToGame();
    }

    public void goToGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitButton () {
        Application.Quit();
    }

    public void SettingsButton () {
        this.initialPanel.SetActive(false);
        this.configurationsPanel.SetActive(true);
        TMPro.TextMeshProUGUI nicknameField = GameObject.Find("NicknamePlaceholder").GetComponent<TMPro.TextMeshProUGUI>();
        string nickname = PlayerPrefs.GetString("nickname");
        nicknameField.text = ((nickname.Length < 1) ? "Defina seu nickname!" : nickname);
    }

    public void SaveSettings() {
        TMPro.TextMeshProUGUI nicknameField = GameObject.Find("NicknameText").GetComponent<TMPro.TextMeshProUGUI>();
        this.saveNickname(nicknameField.text);
        this.configurationsPanel.SetActive(false);
        this.initialPanel.SetActive(true);
    }

    public void SaveNicknameButton() {
        TMPro.TextMeshProUGUI nicknameField = GameObject.Find("NicknameText").GetComponent<TMPro.TextMeshProUGUI>();
        if(this.saveNickname(nicknameField.text)) {
            this.goToGame();
            return;
        }
        GameObject alert = GameObject.Find("AlertText");
        TMPro.TextMeshProUGUI alertText = alert.GetComponent<TMPro.TextMeshProUGUI>();
        alert.SetActive(true);
        alertText.text = "VOCE DEVE DEFINIR UM NICKNAME COM 3 à 20 CARACTERES!";
    }

    public bool saveNickname (string nickname) {
        if(nickname.Length > 3 && nickname.Length < 20) {
            PlayerPrefs.SetString("nickname", nickname);
            return true;
        }
        return false;
    }
}
