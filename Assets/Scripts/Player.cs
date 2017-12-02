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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MissionPoint")
        {
            gm.PlayerReachedMissionPoint();
        }
    }
}
