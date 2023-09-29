using UnityEngine;

public class ItemBox : MonoBehaviour
{
    int item;
    private void OnTriggerEnter(Collider other)
    {
        float pistol = 0.7f;
        float crossBow = 0.6f;
        float shotgun = 0.3f;
        float akm = 0.1f;


        if (other.CompareTag("PLAYER"))
        {
            if (GameManager.instance.player.playerCamera.isRotate)
                return;

            GameManager.instance.player.deleteItem();

            GameManager.instance.getItem++;

            float ran = Random.value;

            if (ran >= pistol)
                item = 3;
            else if (ran >= crossBow)
                item = 1;
            else if (ran >= shotgun)
                item = 2;
            else if (ran >= akm)
                item = 0;

            GameObject weapon = GameManager.instance.ItemPoolManager.GetItemPools(item);
            weapon.transform.parent = other.transform;
            weapon.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            weapon.transform.localPosition = new Vector3(-34.45f, 107.2f, 35.25f);

            GameManager.instance.player.Items = weapon;
            GameManager.instance.player.getGun = true;

            gameObject.SetActive(false); 
        }
    }
    private void Update()
    {
        //지나친 오브젝트 비활성화
        if (gameObject.transform.position.z <= -64)
        {
            gameObject.SetActive(false);
        }
    }
}
