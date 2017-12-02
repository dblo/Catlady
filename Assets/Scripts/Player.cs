using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Bed")
        {
            gm.PlayerWentToBed();
        }
    }
}