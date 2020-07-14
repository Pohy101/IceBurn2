using System;
using Logger;
using UnityEngine;
using IceModSystem;
using IceBurn.Utils;

namespace IceBurn.Mod
{
	class HandControl : VRmod
	{
        public override int LoadOrder => 9;

        //static RootMotion.FinalIK.VRIK controller;
        //public static Hand hand = Hand.None;

        /*  
         *  Components in Avatar
            UnityEngine.Transform
            UnityEngine.Animator
            VRCSDK2.VRC_AvatarDescriptor
            VRC.Core.PipelineManager
            VRC.DynamicBoneController
            RootMotion.FinalIK.VRIK
            RootMotion.FinalIK.FullBodyBipedIK
            LimbIK
            RealisticEyeMovements.EyeAndHeadAnimator
            RealisticEyeMovements.LookTargetController
            NetworkMetadata
        */

        public static void SetHandControl()
		{
			try
			{
				//controller = PlayerWrapper.GetCurrentPlayer().prop_Player_0.prop_VRCAvatarManager_0.prop_GameObject_0.GetComponent<RootMotion.FinalIK.VRIK>();
			}
			catch (NullReferenceException)
			{
				IceLogger.Error("Cannot find VRIK on avatar");
			}
		}

		public override void OnUpdate()
		{
			if (Input.GetMouseButton(1))
			{			
				/*if (controller != null)
				{
					switch (hand)
					{
						case Hand.Left:
							controller.solver.leftArm.positionWeight = 1;
							controller.solver.leftArm.rotationWeight = 1;
							break;
						case Hand.Right:
							controller.solver.rightArm.positionWeight = 1;
							controller.solver.rightArm.rotationWeight = 1;
							break;
						case Hand.Both:
							controller.solver.leftArm.positionWeight = 1;
							controller.solver.leftArm.rotationWeight = 1;
							controller.solver.rightArm.positionWeight = 1;
							controller.solver.rightArm.rotationWeight = 1;
							break;
						default:
							break;
					}
				}*/
			}
		}
	}
}
