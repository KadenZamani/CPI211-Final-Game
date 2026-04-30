using UnityEngine;

public class RxAttackVariable : MonoBehaviour
{
    public bool a = false;
    public bool b = false;
    private bool e = false;
    public Animator rexAnim;
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
    void setTrue()
    {
        a = true;
    }

    void setFalse()
    {
        a = false;
    }
}
