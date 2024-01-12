using UnityEngine;

public class EnemyDestructionListener : MonoBehaviour
{
    public delegate void EnemyDestroyedAction();
    public event EnemyDestroyedAction OnEnemyDestroyed;

    private void Start()
    {
        // Get the EnemyExplosion script and subscribe to the OnDestroy event
        EnemyExplosion enemyExplosion = GetComponent<EnemyExplosion>();
        if (enemyExplosion != null)
        {
            enemyExplosion.OnKill += NotifyEnemyDestroyed;
        }
        else
        {
            Debug.LogError("EnemyExplosion script not found on " + gameObject.name);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnDestroy event to clean up
        EnemyExplosion enemyExplosion = GetComponent<EnemyExplosion>();
        if (enemyExplosion != null)
        {
            enemyExplosion.OnKill -= NotifyEnemyDestroyed;
        }
    }

    public void NotifyEnemyDestroyed()
    {
        OnEnemyDestroyed?.Invoke();
    }
}
//baldwin...