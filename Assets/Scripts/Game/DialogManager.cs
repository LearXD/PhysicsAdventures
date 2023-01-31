using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{

    private GameManager gameManager;

    [Header("Informações do Panel")]
    private GameObject dialogPanel;
    private TMPro.TextMeshProUGUI dialogText;

    [Header("Informações sobre diálogo...")]
    public string text;
    public float writeSpeed = 0;
    private bool writing = false;

    [Header("Informações do dialogo")]
    private List<string> dialogs = new ();
    private int currentDialog = 0;

    void Start()
    {
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        this.dialogPanel = GameObject.Find("DialogPanel");
        this.dialogText = GameObject.Find("DialogText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Show() {
        this.gameManager.paused = !this.dialogPanel.activeSelf;
        this.dialogPanel.SetActive(!this.dialogPanel.activeSelf);
    }

    void ShowDialog(int dialog = 0) {
        this.currentDialog = dialog;
        this.SetText(dialogs.ToArray()[this.currentDialog]);
        this.writing = true;
        StartCoroutine("WriteLether");
    }

    public void SetWriteSpeed(float velocity) {
        this.writeSpeed = velocity;
    }

    public void StartWriting () {
        this.Show();
        this.ShowDialog();
    }

    public void SetText(string text, float velocity = 0.01f) {
        this.dialogText.text = "";
        this.text = text.Replace("{nickname}", this.gameManager.GetPlayer().nickname);
    }

    public void addDialog(string dialog) {
        this.dialogs.Add(dialog);
    }

    public void setDialogs(List<string> dialogs) {
        this.dialogs = dialogs;
    }

    public void clearDialogs() {
        this.dialogs.Clear();
    }

    public void NextButton() {
        if(this.writing) {
            this.writing = false;
            return;
        }

        if((this.dialogs.ToArray().Length - 1) > this.currentDialog) {
            ShowDialog(this.currentDialog + 1);
            return;
        }

        this.clearDialogs();
        this.Show();
    }

    IEnumerator WriteLether() {
        for(int i = 0; i < text.Length; i++) {
            if(this.writing) {
                this.dialogText.text += text[i];
                yield return new WaitForSeconds(this.writeSpeed);
            }
            continue;
        }
        // SKIP
        if(this.dialogText.text != this.text) {
            this.dialogText.text = this.text;
        }
        this.writing = false;
    }

}
