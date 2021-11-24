using System.Collections;
using UnityEngine;

public class AreaExplosionBullet : MonoBehaviour
{
    private Vector3 originPosition;
    private AreaExplosionNode areaExplosionNode;
    private AbstractEntity entity;
    private PlayerController player;

    public void LoadParams(Vector3 originPosition, AreaExplosionNode areaExplosionNode)
    {
        this.originPosition = originPosition;
        this.areaExplosionNode = areaExplosionNode;
    }

    public void LoadPlayer(PlayerController player)
    {
        this.player = player;
    }

    public void LoadEntity(AbstractEntity entity)
    {
        this.entity = entity;
    }

    public void ExecuteExplosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(originPosition, areaExplosionNode.radius);
        foreach (var hitCollider in hitColliders)
        {
            AbstractEntity entityScript = hitCollider.GetComponent<AbstractEntity>();
            PlayerController playerScript = hitCollider.GetComponent<PlayerController>();

            if (entityScript != null && entityScript != entity)
            {
                entityScript.GetHit(areaExplosionNode.damage);
                entityScript.GetSpeedController().ExplodePush(-entityScript.transform.forward, areaExplosionNode.pushForce);
            }

            if (playerScript != null && playerScript != player)
            {
                playerScript.ChangeHealth(-areaExplosionNode.damage);
                playerScript.GetPlayerMovement().ExplodePush(-playerScript.transform.forward, areaExplosionNode.pushForce);
            }
        }
    } 
}
