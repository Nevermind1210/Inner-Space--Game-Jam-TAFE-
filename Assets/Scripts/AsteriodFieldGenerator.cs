using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class AsteriodFieldGenerator : MonoBehaviour
    {
        [SerializeField] private Transform asteriodPrefab;
        [SerializeField] private int fieldRadius = 100;
        [SerializeField] private int asteriodCount = 1000;
        [SerializeField] private GameObject player;
        private void Start()
        {
            InstanitateAsteriods(); // This will run when start.
        }

        /// <summary>
        /// This is a function that will instantiate the asteroids.
        /// </summary>
        void InstanitateAsteriods()
        {
            // This forloop will look at the count
            for (int i = 0; i < asteriodCount; i++)
            {
                Transform newAsteriod = Instantiate(asteriodPrefab, Random.insideUnitSphere * fieldRadius, Random.rotation);
                newAsteriod.localScale = newAsteriod.localScale * Random.Range(0.5f, 5);
            }
        }
        
    }
}