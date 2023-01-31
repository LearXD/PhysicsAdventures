using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private GameManager gameManager;

    [Header("Informações do jogador")]
    public string nickname;

    [Header("Atributos de Movimento")]
    public bool canJump = true;
    public bool phisics = true;

    public bool alive = false;

    public float velocity = 0f;
    public float rollerMultiplier = 1.5f; // VER O BAGULHO DOS PATINS DPS

    public float jumpForce = 0f;

    [Header("Atributos de Level")]
    public int level = 0;
    private float platformDifference = 1.500f;
    public float platform = 0;


    private Rigidbody2D rigidbodyInstance;
    private Animator animatorInstance;

    private Collider2D lastCollision;
    private Transform spawnPosition;

    void Start()
    {
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        this.nickname = PlayerPrefs.GetString("nickname");
        
        this.rigidbodyInstance = this.GetComponent<Rigidbody2D>();
        this.animatorInstance = this.GetComponent<Animator>();

        this.spawnPosition = GameObject.FindGameObjectWithTag("SpawnPosition").transform;
    }

    public void init () {
        this.alive = true;
        this.OnLevelUp();
    }

    public bool IsAlive() => this.alive;
    
    void MovementManager()
    {
        switch (level)
        {
            case 0:
            case 1:
                PlatformMovementManager();
                break;
            case 2:
                SampleMovementManager();
                break;
        }
    }

    void Kill()
    {
        if(this.alive) {
            this.gameManager.Pause(false);
            this.gameManager.respawnPanel.SetActive(true);
            this.alive = false;
            this.platform = 0;
        }
    
    }

    public void Respawn()
    {
        if(!this.alive) {
            this.rigidbodyInstance.velocity = new Vector2(0f, 0f);
            this.alive = true;
            this.gameManager.Pause(false);
            this.gameManager.respawnPanel.SetActive(false);
            this.transform.position = spawnPosition.position;
        }
    }

    // DESCONTINUEI, NEM VAI PRECISAR :/
    void pausePhisics()
    {
        rigidbodyInstance.isKinematic = !(this.phisics = !this.phisics);
    }


    void FixedUpdate()
    {

        /*
         * PARA EVITAR PROBLEMAS COM A DETEÇÃO DAS PLATAFORMAS
        */
        if (level == 1)
        {
            float y = transform.position.y;
            if (y > -3.2 && y < -2.5) platform = 0;
            if (y > -1.7 && y < -1) platform = 1;
            if (y > 0.3) platform = 2;
        }


        if (this.transform.position.y < -8f)
        {
            this.Kill();
        }
    }

    void PlatformMovementManager()
    {
        animatorInstance.SetBool("slide", true);
        transform.Translate(new Vector2(velocity * Time.deltaTime * rollerMultiplier, 0f));

        if (this.lastCollision && this.lastCollision.CompareTag("Platform"))
        {
            if (rigidbodyInstance.velocity.y == 0)
            {
                if (this.platform < 2 && Input.GetKeyDown(KeyCode.W))
                {
                    this.platform++;
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y + platformDifference, 0f);
                    this.transform.position = newPos;
                }
                if (this.platform > 0 && Input.GetKeyDown(KeyCode.S))
                {
                    this.platform--;
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y - platformDifference, 0f);
                    this.transform.position = newPos;
                }
            }

        }
    }

    void SampleMovementManager()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                canJump = false;
                animatorInstance.SetBool("jump", true);
                rigidbodyInstance.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }

        if (movement.x != 0)
        {
            animatorInstance.SetBool("run", true);
            transform.eulerAngles = new Vector2(0f, movement.x < 0 ? 180 : 0);
            this.transform.position += (movement * Time.deltaTime * this.velocity);
            return;
        }

        animatorInstance.SetBool("run", false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Checkpoint"))
        {
            Checkpoint checkpointManager = collider.GetComponent<Checkpoint>();
            if (checkpointManager) checkpointManager.SetActive();
        }

        if (collider.CompareTag("Platform"))
        {
            // fazer o player parar de cair
            this.rigidbodyInstance.velocity = new Vector2(0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        this.lastCollision = collision.collider;


        if (collision.collider.CompareTag("Ground"))
        {
            animatorInstance.SetBool("jump", false);
            canJump = true;
        }

        if (collision.collider.CompareTag("Platform"))
        {
            // fazer o player parar de cair
            this.rigidbodyInstance.velocity = new Vector2(0f, 0f);
        }

        if (collision.collider.CompareTag("Traps"))
        {
            this.Kill();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = false;
        }
    }

    public void OnLevelUp() {
        switch(this.level) {
            case 0:
                this.gameManager.GetDialogManager().addDialog("Bem vindo {nickname}!!! Na aula de hoje você aprendera sobre movimentos!");
                this.gameManager.GetDialogManager().addDialog("Fase 1: movimento retilíneo uniforme. ");
                this.gameManager.GetDialogManager().addDialog("Movimento retilíneo uniforme é um movimento em que um objeto se desloca em uma linha reta, com velocidade constante.");
                this.gameManager.GetDialogManager().addDialog("Você terá que passar pelas linhas desviando dos espinhos e buracos.\nLembre-se que que a velocidade é sempre a mesma e você só poderá ir para cima, ou para baixo.");

                this.gameManager.GetDialogManager().StartWriting();
            break;
            case 2:
                animatorInstance.SetBool("slide", false);
                this.rigidbodyInstance.gravityScale = 2;
                this.gameManager.GetDialogManager().addDialog("Parabéns {nickname}! você conseguiu superar a fase 1!!");
                this.gameManager.GetDialogManager().addDialog("Agora, na fase 2 você aprenderá sobre outro tipo de movimento. O movimento retilíneo uniformemente variado!");
                this.gameManager.GetDialogManager().addDialog("O Movimento Retilíneo Uniformemente Variado é um movimento que consiste na variação uniforme da velocidade ao longo do tempo. Isso significa que, em qualquer intervalo de tempo, a diferença da velocidade é sempre a mesma.");
                this.gameManager.GetDialogManager().addDialog("Agora você poderá movimentar-se para frente e para trás e também pular. Complete o parkour para finalizar a fase.");
                this.gameManager.GetDialogManager().StartWriting();
            break;
            case 3: 
                this.gameManager.GetDialogManager().addDialog("Parabéns {nickname}!!! \nVocê conseguiu completar as fases e coletar a medalha de física!");
                this.gameManager.GetDialogManager().addDialog("Agora você entende como esses movimentos funcionam!");
                
                this.gameManager.GetDialogManager().StartWriting();
            break;
        }
    }

    void Update()
    {
        if (!gameManager.IsPaused() && this.alive)
        {
            MovementManager();
        }
    }
}
