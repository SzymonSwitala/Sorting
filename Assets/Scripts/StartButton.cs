using UnityEngine;
using TMPro;
public class StartButton : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown sortType;
    [SerializeField] private RobotController robot;
    public void Accept()
    {
        SoundController.playSound(SoundController.blipSound);
        robot.StartSorting(sortType);
    }
}
