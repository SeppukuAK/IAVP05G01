﻿using UnityEngine;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Abilities;
using System;

namespace BehaviorDesigner.Runtime.Tasks.ThirdPersonController
{
    [TaskDescription("Tries to start the ability.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=111")]
    [TaskCategory("Third Person Controller")]
    [TaskIcon("Assets/Behavior Designer/Third Party/Third Person Controller/Editor/Icon.png")]
    public class StartAbility : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The name of the ability to start.")]
        [AbilityDrawer]
        public SharedString abilityType;
        [Tooltip("If multiple abilities types are found, the priority index can be used to specify which ability should be started")]
        public SharedInt priorityIndex = -1;

        private GameObject prevTarget;
        private RigidbodyCharacterController controller;
        private ControllerHandler controllerHandler;
        private Ability ability;

        public override TaskStatus OnUpdate()
        {
            var target = GetDefaultGameObject(targetGameObject.Value);
            if (target != prevTarget) {
                controller = target.GetComponentInParent<RigidbodyCharacterController>();
                controllerHandler = target.GetComponentInParent<ControllerHandler>();
                var abilities = controller.GetComponents(TaskUtility.GetTypeWithinAssembly(abilityType.Value));
                if (abilities.Length > 1) {
                    if (priorityIndex.Value != -1) {
                        for (int i = 0; i < abilities.Length; ++i) {
                            var localAbility = abilities[i] as Ability;
                            if (localAbility.Index == priorityIndex.Value) {
                                ability = localAbility;
                                break;
                            }
                        }
                    } else {
                        ability = abilities[0] as Ability;
                    }
                } else if (abilities.Length == 1) {
                    ability = abilities[0] as Ability;
                }
                prevTarget = target;
            }
            if (ability == null) {
                return TaskStatus.Failure;
            }

            controllerHandler.TryStartAbility(ability);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            abilityType = string.Empty;
            priorityIndex = -1;
        }
    }
}