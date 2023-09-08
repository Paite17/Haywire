using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Attack", menuName = "Attacks/Create new player attack")]
public class PlayerMove : ScriptableObject
{
    [SerializeField] private string moveName;

    [TextArea][SerializeField] private string description;

    [SerializeField] private int movePower;
    // differences between these two is that one holds the position of the note and the other holds which note it is
    [SerializeField] private List<Vector3> posChart;
    [SerializeField] private List<GameObject> noteChart;
    [SerializeField] private Sprite comboPreview;

    public string MoveName
    {
        get { return moveName; }
    }

    public string Description
    {
        get { return description; }
    }

    public int MovePower
    {
        get { return movePower; }
    }

    public List<Vector3> PosChart
    {
        get { return posChart; }
    }

    public List<GameObject> NoteChart
    {
        get { return noteChart; }
    }

    public Sprite ComboPreview
    {
        get { return comboPreview; }
    }
}
