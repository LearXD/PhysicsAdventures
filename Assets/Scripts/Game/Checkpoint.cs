using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    
    private Player player;
    private GameObject spawnpoint;
    private Animator animator;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.spawnpoint = GameObject.FindGameObjectWithTag("SpawnPosition");
        this.animator = GetComponent<Animator>();   
    }
 
    public void SetActive() {
        if(!this.animator.GetBool("active")) {
             this.spawnpoint.transform.position = transform.position;

            this.player.level++;
            this.player.OnLevelUp();
           
            this.animator.SetBool("active", true);
        }
    }
}
