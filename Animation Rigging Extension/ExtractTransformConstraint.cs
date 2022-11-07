using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;
using UnityEngine.Animations;

namespace Project.AnimationRiggingExtension
{
    //Source: https://forum.unity.com/threads/read-bones-transforms-before-ik-corrections-with-animation-rigging.813966/

    /*
     How to use:

    Similar to adding a Two Bone Constraint (or any other), you'll see there's a new "Extract Transform Constraint" component under the Animation Rigging component menu.

    Simply add this before any other constraint. In order to get the position and rotation extracted by the constraint, you can do:


    // reference to the constraint:
    public ExtractTransformConstraint constraint;
 
    void Update()
    {
        Debug.Log("Position: " + this.constraint.data.position);
        Debug.Log("Rotation: " + this.constraint.data.rotation);
    }
     */


    [DisallowMultipleComponent]
    [AddComponentMenu("Animation Rigging/Extract Transform Constraint")]

    public class ExtractTransformConstraint : RigConstraint<
    ExtractTransformConstraintJob,
    ExtractTransformConstraintData,
    ExtractTransformConstraintJobBinder>
    {

    }

    [Serializable]
    public struct ExtractTransformConstraintData : IAnimationJobData
    {
        [SyncSceneToStream] public Transform bone;

        public Vector3 position;
        public Vector3 localPosition;
        public Quaternion rotation;
        public Quaternion localRotation;

        public bool IsValid()
        {
            return bone != null;
        }

        public void SetDefaultValues()
        {
            this.bone = null;

            this.position = Vector3.zero;
            this.localPosition = Vector3.zero;
            this.rotation = Quaternion.identity;
            this.localRotation = Quaternion.identity;
        }
    }

    public struct ExtractTransformConstraintJob : IWeightedAnimationJob
    {
        public ReadWriteTransformHandle bone;

        public FloatProperty jobWeight { get; set; }

        public Vector3Property position;
        public Vector3Property localPosition;
        public Vector4Property rotation;
        public Vector4Property localRotation;

        public void ProcessRootMotion(AnimationStream stream)
        { }

        public void ProcessAnimation(AnimationStream stream)
        {
            AnimationRuntimeUtils.PassThrough(stream, this.bone);

            Vector3 pos = this.bone.GetPosition(stream);
            Quaternion rot = this.bone.GetRotation(stream);
            Vector3 localPos = this.bone.GetLocalPosition(stream);
            Quaternion localRot = this.bone.GetLocalRotation(stream);

            this.position.Set(stream, pos);
            this.rotation.Set(stream, new Vector4(rot.x, rot.y, rot.z, rot.w));
            this.localPosition.Set(stream, localPos);
            this.localRotation.Set(stream, new Vector4(localRot.x, localRot.y, localRot.z, localRot.w));
        }
    }
    public class ExtractTransformConstraintJobBinder : AnimationJobBinder<
    ExtractTransformConstraintJob,
    ExtractTransformConstraintData>
    {
        public override ExtractTransformConstraintJob Create(Animator animator,
            ref ExtractTransformConstraintData data, Component component)
        {
            return new ExtractTransformConstraintJob
            {
                bone = ReadWriteTransformHandle.Bind(animator, data.bone),
                position = Vector3Property.Bind(animator, component, "m_Data." + nameof(data.position)),
                rotation = Vector4Property.Bind(animator, component, "m_Data." + nameof(data.rotation)),
                localPosition = Vector3Property.Bind(animator, component, "m_Data." + nameof(data.localPosition)),
                localRotation = Vector4Property.Bind(animator, component, "m_Data." + nameof(data.localRotation)),
            };
        }

        public override void Destroy(ExtractTransformConstraintJob job)
        { }
    }
}
