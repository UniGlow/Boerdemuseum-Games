using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuenstlerportraits
{
    //[CreateAssetMenu(menuName = "Scriptable Objects/Künstlerportraits/Artist")]
    public class Artist : MonoBehaviour//ScriptableObject
    {

        #region Variable Declarations
        [Space]
        public string Name;
        public string Lifetime;
        public Sprite Image;

        [Space]
        [TextArea(1, 20)]
        public string Description;
        [TextArea(1, 20)]
        public string DescriptionEnglish;
        [TextArea(1, 20)]
        public string DescriptionPlatt;
        public List<Category> Categories = new List<Category>();
        #endregion



        #region Public Properties

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            Categories = new List<Category>();
        }
        #endregion



        #region Public Functions
        public Category FindCategory(string pName)
        {
            if (Categories.Count > 0)
            {
                foreach (Category category in Categories)
                {
                    if (category.Name.Equals(pName)) return category;
                }
            }

            return null;
        }
        #endregion



        #region Private Functions

        #endregion



        #region Coroutines

        #endregion
    }
}