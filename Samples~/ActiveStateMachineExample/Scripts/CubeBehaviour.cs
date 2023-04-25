using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    [SerializeField, Min(0f)] private float moveSpeed;

    private CubeAgent _cubeAgent;

    private void Awake()
    {
        // Create a cube agent and store it a field.
        _cubeAgent = new CubeAgent(transform, moveSpeed);
    }

    private void Update()
    {
        // Make the agent active by calling its Tick() method in the Update() method.
        // This will check the conditions for changing state and will also call the Tick() method on
        // the current state.
        _cubeAgent.Tick();
    }
}
