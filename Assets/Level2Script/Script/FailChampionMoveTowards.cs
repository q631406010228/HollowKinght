using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{ 
    public class FailChampionMoveTowards : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("The agent has arrived when the magnitude is less than this value")]
        public SharedFloat arriveDistance = 0.1f;
        [Tooltip("Should the agent be looking at the target position?")]
        public SharedBool lookAtTarget = true;
        [Tooltip("Max rotation delta if lookAtTarget is enabled")]
        public SharedFloat maxLookAtRotationDelta;
        [Tooltip("The GameObject that the agent is moving towards")]
        public SharedGameObject target;
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 targetPosition;

        public override TaskStatus OnUpdate()
        {
            var position = Target();
            float y = transform.position.y;
            // Return a task status of success once we've reached the target
            if (Vector3.Magnitude(transform.position - position) < arriveDistance.Value) {
                return TaskStatus.Success;
            }
            // We haven't reached the target yet so keep moving towards it
            transform.position = Vector3.MoveTowards(transform.position, position, speed.Value * Time.deltaTime);
            if (lookAtTarget.Value && (position - transform.position).sqrMagnitude > 0.01f) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(position - transform.position), maxLookAtRotationDelta.Value);
            }
            transform.position = new Vector2(transform.position.x, y);
            return TaskStatus.Running;
        }

        // Return targetPosition if targetTransform is null
        private Vector3 Target()
        {
            if (target == null || target.Value == null) {
                return targetPosition.Value;
            }
            return target.Value.transform.position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            arriveDistance = 0.1f;
            lookAtTarget = true;
        }
    }
}