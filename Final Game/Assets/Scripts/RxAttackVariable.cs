using UnityEngine;

public class RxAttackVariable : MonoBehaviour
{
    public bool a = false;
    public bool b = false;
    private bool e = false;
    public Animator rexAnim;
    public AudioSource rexSound;
    public AudioClip roarSound;
    public AudioClip BiteSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (b && !e)
        {
            e = true;
            b = false;
            rexAnim.SetTrigger("roarTrigger");
        }
    }
    public void setTrue()
    {
        a = true;
    }

    public void setFalse()
    {
        a = false;
    }

    public void PlayRoarSound()
    {
        rexSound.PlayOneShot(roarSound );
    }

    public void PlayBiteSound()
        {
            rexSound.PlayOneShot(BiteSound);
    }   

}
