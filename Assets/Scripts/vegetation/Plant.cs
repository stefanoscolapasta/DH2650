using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegetation
{
    [System.Serializable]
    public class Plant
    {
        [Header("GENERAL")]
        public string name;
        public Sprite icon;

        [Header("TRUNK")] // width: 0.05 length: 0.5
        public Color trunkColor;
        public int trunkBranchNumber;
        public int trunkNumber;

        
        [System.Serializable]
        public class Foliage
        {
            public GameObject prefab;
            public Color color;
            public Color overlayA;
            public Color overlayB;
            public float p;
            public float size;
        }
        [Header("FOLIAGE")]
        public List<Foliage> foliages;
        public GameObject explosion;
    }
}