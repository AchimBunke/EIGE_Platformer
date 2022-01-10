
public class WayPoint : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position, 1f);
    }
}