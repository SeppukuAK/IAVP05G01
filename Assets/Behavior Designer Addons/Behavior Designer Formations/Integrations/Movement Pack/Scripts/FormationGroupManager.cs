using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Formations.Integrations.MovementPack
{
    /// <summary>
    /// Allows the Movement Pack tasks to be used as the leader of a Formations Pack group.
    /// </summary>
    public class FormationGroupManager : MonoBehaviour
    {
        [UnityEngine.Tooltip("Specifies the group index of the leader behavior tree. This is not necessary if there is only one behavior tree on the leader")]
        public int behaviorGroupIndex;

        private Behavior behavior;
        private List<Behavior> formationTrees;

        /// <summary>
        /// Cache the component references and list for any interested events.
        /// </summary>
        private void Awake()
        {
            var behaviors = GetComponents<Behavior>();
            if (behaviors.Length > 1) {
                for (int i = 0; i < behaviors.Length; ++i) {
                    if (behaviors[i].Group == behaviorGroupIndex) {
                        behavior = behaviors[i];
                        break;
                    }
                }
            } else if (behaviors.Length == 1) {
                behavior = behaviors[0];
            }

            behavior.RegisterEvent<Behavior>("StartListeningForOrders", StartListeningForOrders);
            behavior.RegisterEvent<Behavior>("StopListeningToOrders", StopListeningToOrders);
            behavior.RegisterEvent<object>("OrdersFinished", OrdersFinished);
        }

        /// <summary>
        /// Adds the agent to the formation group.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        private void StartListeningForOrders(Behavior agent)
        {
            if (formationTrees == null) {
                formationTrees = new List<Behavior>();
            }
            // Notify the current agent of the existing agents.
            for (int i = 0; i < formationTrees.Count; ++i) {
                agent.SendEvent("AddAgentToGroup", formationTrees[i], i);
            }
            var index = formationTrees.Count;
            formationTrees.Add(agent);
            // Notify other agents that the current agent has joined the formation.
            for (int i = 0; i < formationTrees.Count; ++i) {
                formationTrees[i].SendEvent("FormationUpdated", i + 1);
                formationTrees[i].SendEvent("AddAgentToGroup", formationTrees[index], index);
                formationTrees[i].SendEvent("UpdateMoveStatus", i, Tasks.FormationGroup.MoveStatus.Full);
            }
        }

        /// <summary>
        /// Removes the agent from the group.
        /// </summary>
        /// <param name="agent">The agent to remove.</param>
        /// <returns>The index of the agent removed from the group.</returns>
        private void StopListeningToOrders(Behavior agent)
        {
            if (formationTrees != null) {
                var agentTransform = agent.transform;
                for (int i = formationTrees.Count - 1; i >= 0; --i) {
                    if (formationTrees[i].transform == agentTransform) {
                        formationTrees.RemoveAt(i);
                        for (int j = 0; j < formationTrees.Count; ++j) {
                            formationTrees[j].SendEvent("StopListeningToOrders", agent);
                            formationTrees[j].SendEvent("FormationUpdated", j + 1);
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Stops the formation group. Can be called by sending the OrdersFinished event to the leader behavior tree.
        /// </summary>
        /// <param name="successObj">Should the following tasks end in success?</param>
        private void OrdersFinished(object successObj)
        {
            if (formationTrees != null) {
                var success = (bool)successObj;
                for (int i = 0; i < formationTrees.Count; ++i) {
                    formationTrees[i].SendEvent("OrdersFinished", success ? TaskStatus.Success : TaskStatus.Failure);
                }
                formationTrees.Clear();
            }
        }
    }
}