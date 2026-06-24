using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 10f;
    public float viewAngle = 90f;
    public Transform Player;

    public bool CanSeePlayer()
    {
        Vector3 dir = (Player.position - transform.position).normalized;
        if(Vector3.Angle(transform.forward, dir)< viewAngle /2f)
        {
            if(Vector3.Distance(transform.position, Player.position) < viewRadius)
            {
                return true;
            }
        }
        return false;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
