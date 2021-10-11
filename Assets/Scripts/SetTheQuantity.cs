using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetTheQuantity : MonoBehaviour
{
    //buttons and UI
    public int quantity;
    [SerializeField] private int minQuantity;
    [SerializeField] private int maxQuantity;
    [SerializeField] private TextMeshProUGUI quantityText;
    // table and boxes
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject spawner;
    [SerializeField] List<GameObject> boxList;
    private void Awake()
    {
        
        quantityText.text = "" + quantity;
        table.transform.localScale = new Vector3(1, 1, quantity);
       
        for (int i = 0; i < quantity; i++)
        {

            boxList.Add(Instantiate(box, spawner.transform.position + new Vector3(i, 0.5f, 0), Quaternion.identity)); 
            boxList[i].transform.SetParent(spawner.transform);
        
        }
 
        spawner.transform.localPosition = new Vector3(-quantity / 2 , spawner.transform.position.y, 0);
    }
    public void AddOrSubtrack(int number)
    {
        if (quantity <= minQuantity)
        {
            if (number < 0)
            {
                return;
            }
        }                    
        if (quantity >= maxQuantity)
        {
            if (number > 0)
            {
                
                return;
            }
        }
        if (number>0)
        {
            boxList.Add(Instantiate(box, spawner.transform.position + new Vector3(boxList.Count, 2, 0), Quaternion.identity));
            boxList[boxList.Count-1].transform.SetParent(spawner.transform);
        }
        if (number < 0)
        {
            Destroy(boxList[boxList.Count - 1]);
            boxList.RemoveAt(boxList.Count-1);

        }
        
        quantity += number;
        table.transform.localScale = new Vector3(1, 1, quantity);
        quantityText.text = "" + quantity;
        spawner.transform.localPosition = new Vector3(-quantity / 2, spawner.transform.position.y, 0);

    }
}
