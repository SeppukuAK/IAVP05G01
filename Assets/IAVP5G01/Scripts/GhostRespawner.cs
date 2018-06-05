using UnityEngine;


using Opsive.ThirdPersonController;

/// <summary>
/// When the character respawns it should respawn in the location determined by SpawnSelection.
/// </summary>
public class GhostRespawner : Respawner
{
    /// <summary>
    /// The character should spawn. Override Spawn to allow the SpawnSelection component determine the location that the character should spawn.
    /// Call the corresponding server or client method.
    /// </summary>
    public override void Spawn()
    {
        var location = GhostSpawnSelection.GetSpawnLocation();

        SpawnLocal(location.position, location.rotation);
    }
    /// <summary>
    /// The character should spawn with the specified position and rotation.
    /// </summary>
    private void SpawnLocal(Vector3 position, Quaternion rotation)
    {
        m_RespawnEvent = null;

        transform.SetPositionAndRotation(position, rotation);
        m_GameObject.SetActive(true);

        EventHandler.ExecuteEvent(m_GameObject, "OnRespawn");
    }
}