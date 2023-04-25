using Hybel.StateMachines;
using UnityEngine;

public class CubeAgent : ActiveStateObject<CubeState>
{
    public CubeAgent(Transform transform, float moveSpeed)
    {
        // Setup all the states we're using.
        var moveLeftState = new MoveLeftState(transform, moveSpeed);
        var moveRightState = new MoveRightState(transform, moveSpeed);
        var moveUpState = new MoveUpState(transform, moveSpeed);
        var moveDownState = new MoveDownState(transform, moveSpeed);

        // Define some conditions for when to change state.
        Condition movingUpAndAbove3Y = () => CurrentState is MoveUpState && transform.position.y > 3;
        Condition movingDownAndBelow3Y = () => CurrentState is MoveDownState && transform.position.y < -3;
        Condition movingRightAndBeyond3X = () => CurrentState is MoveRightState && transform.position.x > 3;
        Condition movingLeftAndBeyondNegative3X = () => CurrentState is MoveLeftState && transform.position.x < -3;

        // Configure the state machine.
        Configure()
            // Assuming we're in the 'moveLeftState'...
            .At(moveLeftState)
                // Allow a transition to the 'moveDownState' when the 'movingLeftAndBeyondNegative3X' condition is true.
                .Permit(moveDownState, movingLeftAndBeyondNegative3X)
            // Assuming we're in the 'moveDownState'...
            .At(moveDownState)
                // Allow a transition to the 'moveRightState' when the 'movingDownAndBelow3Y' condition is true.
                .Permit(moveRightState, movingDownAndBelow3Y)
            // Assuming we're in the 'moveRightState'...
            .At(moveRightState)
                // Allow a transition to the 'moveUpState' when the 'movingRightAndBeyond3X' condition is true.
                .Permit(moveUpState, movingRightAndBeyond3X)
            // Assuming we're in the 'moveUpState'...
            .At(moveUpState)
                // Allow a transition to the 'moveLeftState' when the 'movingUpAndAbove3Y' condition is true.
                .Permit(moveLeftState, movingUpAndAbove3Y);

        // Initialize the state machine in the 'moveLeftState'.
        // This is required to make the state machine work and must be called AFTER configuring the state machine.
        SetInitialState(moveLeftState);
    }
}

// Base state to limit which states i can use in the cube agent. This gives better compile-time checks as states
// from other state machines can't be used in this one.
public abstract class CubeState : IActiveState
{
    protected readonly Transform _transform;
    protected readonly float _moveSpeed;

    public CubeState(Transform transform, float moveSpeed)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;
    }

    // These are not needed for this state machine so just leave them empty here.
    public void OnEnterState() { }
    public void OnExitState() { }

    // Make the interface implementation of Tick abstract so that deriving classes have to implement it.
    public abstract void Tick();
}

// Derive the CubeState class and override the Tick() method.
public class MoveLeftState : CubeState
{
    public MoveLeftState(Transform transform, float moveSpeed) : base(transform, moveSpeed) { }

    public override void Tick()
    {
        // In the tick method we translate the transform to the left constantly.
        // Note that this won't be called when this is not the active state in the state machine.
        _transform.Translate(Vector3.left * _moveSpeed);
    }
}

public class MoveRightState : CubeState
{
    public MoveRightState(Transform transform, float moveSpeed) : base(transform, moveSpeed) { }

    public override void Tick() => _transform.Translate(Vector3.right * _moveSpeed);
}

public class MoveUpState : CubeState
{
    public MoveUpState(Transform transform, float moveSpeed) : base(transform, moveSpeed) { }
    public override void Tick() => _transform.Translate(Vector3.up * _moveSpeed);
}

public class MoveDownState : CubeState
{
    public MoveDownState(Transform transform, float moveSpeed) : base(transform, moveSpeed) { }

    public override void Tick() => _transform.Translate(Vector3.down * _moveSpeed);
}
