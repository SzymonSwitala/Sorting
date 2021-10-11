using System.Collections;
using UnityEngine;
using TMPro;
public class RobotController : MonoBehaviour
{
    [SerializeField] private float robotSpeed; 
    [SerializeField] private GameObject sortingController;
    [SerializeField] private BoardController boardController;
    [SerializeField] private Animator animator;
    [SerializeField] private Box[] boxes;
    [SerializeField] private Box boxInLeftHand;
    [SerializeField] private Box boxInRightHand;
    [SerializeField] private Transform leftHandPos;
    [SerializeField] private Transform rightHandPos;
    [SerializeField] private ParticleSystem ps;


    public void StartSorting(TMP_Dropdown sortType)
    {

        boxes = new Box[sortingController.transform.childCount];
        boxes = sortingController.GetComponentsInChildren<Box>();
        boardController.ShowTexts();
        boardController.SetTexts(sortType.options[sortType.value].text);
        switch (sortType.value)
        {
            case 0:
                Debug.Log("Sorting 0");
                StartCoroutine(BubbleSort(boxes));
                break;
            case 1:
                Debug.Log("Sorting 1");
                StartCoroutine(ShakeSort(boxes));
                break;
            case 2:
                StartCoroutine(InsertionSort(boxes));
                break;
        }
        boardController.startTimer = true;
    }
    private IEnumerator BubbleSort(Box[] allBoxes)
    {

        for (int i = 1; i <= allBoxes.Length - 1; ++i)
        {
            for (int j = 0; j < allBoxes.Length - i; ++j)
            {

                yield return StartCoroutine(PickingUp(j, true));
                yield return StartCoroutine(PickingUp(j + 1, false));
                yield return StartCoroutine(PlayAnimation("head", true));
                PlayAnimation("head", false);
                
                if (CheckBoxes(boxInLeftHand, boxInRightHand))
                {
                    yield return StartCoroutine(PuttingAway(j, false));
                    yield return StartCoroutine(PuttingAway(j + 1, true));

                    Swap(j, j + 1);
                }
                else
                {
                    yield return StartCoroutine(PuttingAway(j, true));
                    yield return StartCoroutine(PuttingAway(j + 1, false));

                }
                boardController.AddComparisons(1);

            }
            allBoxes[allBoxes.Length - i].ChangeMaterial();
        }
        allBoxes[0].ChangeMaterial();
        boardController.startTimer = false;
        yield return StartCoroutine(Finish(allBoxes));
    }
    private IEnumerator ShakeSort(Box[] allBoxes)
    {
        int bottom = 0, top = allBoxes.Length - 1;
        bool replace = true;

        while (replace)
        {
            replace = false;

            for (int i = bottom; i < top; i++)
            {

                yield return StartCoroutine(PickingUp(i, true));
                yield return StartCoroutine(PickingUp(i + 1, false));
                yield return StartCoroutine(PlayAnimation("head", true));
                PlayAnimation("head", false);
                if (CheckBoxes(boxInLeftHand, boxInRightHand))
                {
                    yield return StartCoroutine(PuttingAway(i, false));
                    yield return StartCoroutine(PuttingAway(i + 1, true));

                    Swap(i, i + 1);


                    replace = true;
                }
                else
                {
                    yield return StartCoroutine(PuttingAway(i, true));
                    yield return StartCoroutine(PuttingAway(i + 1, false));

                }
                boardController.AddComparisons(1);

            }
            allBoxes[top].ChangeMaterial();

            top--;
            for (int i = top; i > bottom; i--)
            {
                yield return StartCoroutine(PickingUp(i, true));
                yield return StartCoroutine(PickingUp(i - 1, false));
                yield return StartCoroutine(PlayAnimation("head", true));
                PlayAnimation("head", false);
                if (CheckBoxes(boxInRightHand, boxInLeftHand))
                {

                    yield return StartCoroutine(PuttingAway(i, false));
                    yield return StartCoroutine(PuttingAway(i - 1, true));

                    Swap(i, i - 1);

                    replace = true;
                }
                else
                {
                    yield return StartCoroutine(PuttingAway(i, true));
                    yield return StartCoroutine(PuttingAway(i - 1, false));

                }
                boardController.AddComparisons(1);
            }
            allBoxes[bottom].ChangeMaterial();
            bottom++;

        }

        allBoxes[bottom - 1].ChangeMaterial();
        allBoxes[top + 1].ChangeMaterial();

        boardController.startTimer = false;
        yield return StartCoroutine(Finish(allBoxes));
    }
    private IEnumerator InsertionSort(Box[] allBoxes)
    {

        int j;
        allBoxes[0].ChangeMaterial();
        for (int i = 1; i < allBoxes.Length; i++)
        {
            allBoxes[i].ChangeMaterialToPurple();
            Box temp = allBoxes[i];
            for (j = i - 1; j >= 0; j--)
            {
                yield return StartCoroutine(PickingUp(j + 1, false));
               
                yield return StartCoroutine(PickingUp(j, true));
               
                yield return StartCoroutine(PlayAnimation("head",true));
                PlayAnimation("head", false);
                if (CheckBoxes(boxInLeftHand, boxInRightHand))
                {

                    yield return StartCoroutine(PuttingAway(j + 1, true));
                    yield return StartCoroutine(PuttingAway(j, false));
                
                    Swap(j, j + 1);


                }
                else
                {
                  
                    yield return StartCoroutine(PuttingAway(j, true));
                    yield return StartCoroutine(PuttingAway(j + 1, false));
                
                    break;
                }
                
                boardController.AddComparisons(1);
                
            }
            temp.ChangeMaterial();

        }
        boardController.startTimer = false;
        yield return StartCoroutine(Finish(allBoxes));
    }
    private IEnumerator PickingUp(int boxNumber, bool rightHand)// box number , if true right hand if false left hand
    {

        Box box = boxes[boxNumber];

        box.DisablePhysics(true);

        if (rightHand == true)
        {

            yield return StartCoroutine(Move(box.transform.position));
            box.transform.SetParent(rightHandPos);
            yield return StartCoroutine(PlayAnimation("RightHand",true));
          
            boxInRightHand = box;
        }
        else
        {

            yield return StartCoroutine(Move(boxes[boxNumber].transform.position - new Vector3(1, 0, 0)));
            box.transform.SetParent(leftHandPos);
            yield return StartCoroutine(PlayAnimation("LeftHand",true));
         
            boxInLeftHand = box;
        }
      
       
        yield return null;
    }
    private IEnumerator PuttingAway(int posX, bool rightHand) //Position to put away the box , if true right hand if false left hand
    {



        if (rightHand == true)
        {

            boxInRightHand.DisablePhysics(true);
            yield return StartCoroutine(Move(sortingController.transform.position + new Vector3(posX, 0, 0)));
            
            yield return StartCoroutine(PlayAnimation("RightHand",false));
            boxInRightHand.transform.SetParent(sortingController.transform);
         
            boxInRightHand = null;
        }
        else
        {

            boxInLeftHand.DisablePhysics(true);
            yield return StartCoroutine(Move(sortingController.transform.position + new Vector3(posX - 1, 0, 0)));
            yield return StartCoroutine(PlayAnimation("LeftHand",false));
            boxInLeftHand.transform.SetParent(sortingController.transform);
          
            boxInLeftHand = null;
        }

        yield return new WaitForSeconds(0.5f);
        yield return null;
    }
    private void Swap(int box1, int box2)
    {
        Box temp = boxes[box1];
        boxes[box1] = boxes[box2];
        boxes[box2] = temp;

    }// swap boxes in array 
    private bool CheckBoxes(Box box1, Box box2) // if 1 < 2 return true
    {
        if (box1.number < box2.number)
        {
            Debug.Log("swap");
            return true;
        }
        Debug.Log("no swap");
        return false;
    }
    private IEnumerator Move(Vector3 target)
    {

        while (this.gameObject.transform.position.x != target.x + 0.5f)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, new Vector3(target.x + 0.5f, 0, target.z + 0.9f), robotSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }


        yield return null;

    }
    private IEnumerator PlayAnimation(string name, bool state)
    {
        float time = 0;
        if (name == "head")
        {
            time = 1f;

        }
        else
        {
            time = 0.5f;
        }
        animator.SetBool(name,state);
        yield return new WaitForSeconds(time);
    }
private IEnumerator Finish(Box[] allBoxes)
    {
        for (int i=0;i<allBoxes.Length;i++)
        {

            yield return StartCoroutine(Move(allBoxes[i].transform.position));
            Instantiate(ps,transform.position+new Vector3(0,0,2),Quaternion.identity);
        }
    }
}
