using Assets.Scripts.Actor;
using EzySlice;
using System;
using UnityEngine;

namespace Assets.Scripts.Slicing
{
    public class SlicingManager : MonoBehaviour
    {
        public Transform cuttingPlane;

        InputManager inputManager;

        public Material crossMaterial;
        public LayerMask layerMask;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
        }

        internal void EnableSlice()
        {
            Time.timeScale = 0.2f;
            cuttingPlane.gameObject.SetActive(true);
        }

        internal void DisableSlice()
        {
            Time.timeScale = 1.0f;
            cuttingPlane.gameObject.SetActive(false);
        }

        public void Slice()
        {
            Collider[] hits = Physics.OverlapBox(cuttingPlane.position, new Vector3(5, 0.1f, 5), cuttingPlane.rotation, layerMask);

            if (hits.Length <= 0)
            {
                return;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
                if (hull != null)
                {
                    GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                    GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                    AddHullComponents(bottom);
                    AddHullComponents(top);
                    Destroy(hits[i].gameObject);
                }
            }
        }
        public void AddHullComponents(GameObject go)
        {
            go.layer = 6;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            MeshCollider collider = go.AddComponent<MeshCollider>();
            rb.mass = 1.0f;
            collider.convex = true;

            rb.AddExplosionForce(200, go.transform.position, 20);
        }

        public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
        {
            // slice the provided object using the transforms of this object
            if (obj.GetComponent<MeshFilter>() == null)
                return null;

            return obj.Slice(cuttingPlane.position, cuttingPlane.up, crossSectionMaterial);
        }

        public void HandleAllSlicing()
        {
            RotatePlayer();
            RotatePlane();
        }

        private void RotatePlane()
        {
            cuttingPlane.eulerAngles += new Vector3(0, 0, - inputManager.rollAim.x * 5);
        }

        private void RotatePlayer()
        {
            //throw new NotImplementedException();
        }
    }
}
