using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        private bool canOpen;
        public GameObject dropItem1,dropItem2;
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(canOpen && !isOpened)
                {
                    Open();
                    DropItem();
                }
            }
        }


        [FoldoutGroup("Runtime"),Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player")  && 
                collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
            {
                canOpen = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") &&
                collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
            {
                canOpen = false;
            }
        }

        void DropItem()
        {

            Instantiate(dropItem1, transform.position, Quaternion.identity);

            for (int i = 0; i < 3; i++)
            {
                Instantiate(dropItem2, transform.position, Quaternion.identity);
            }          
        }
    }
}
