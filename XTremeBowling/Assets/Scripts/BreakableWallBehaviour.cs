using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * used to be with 3 lives that cracks appear on each life lost until it breaks.
 * Removed the extra lives because it halted the gameplay experience.
 * by simply changing the breaking state back to 0 in the inspector
 * will make it with its full HP again
 */

public class BreakableWallBehaviour : MonoBehaviour
{
    public GameObject breakingLevel1;
    public GameObject breakingLevel2;
    public GameObject wallPiece;
    public GameObject cubePiece;

    public int breakingState = 0;
    private List<GameObject> cubePieces = new List<GameObject>();

    private void Start()
    {
        for (int i = -14; i < 14; i++)
        {
            for (int j = -9; j < 9; j++)
            {
                GameObject piece = Instantiate(cubePiece, Vector3.zero, Quaternion.identity);
                piece.transform.parent = gameObject.transform;
                piece.transform.localPosition = new Vector3(i + 0.5f, j + 0.5f, 1.0f);
                cubePieces.Add(piece);
                piece.SetActive(false);
            }
        }
    }

    public void IncreaseBreakingState()
    {
        breakingState++;

        switch(breakingState)
        {
            case 1:
                breakingLevel1.SetActive(true);
                break;
            case 2:
                breakingLevel2.SetActive(true);
                break;
            case 3:
                breakingLevel1.SetActive(false);
                breakingLevel2.SetActive(false);
                wallPiece.SetActive(false);
                gameObject.GetComponent<BoxCollider>().enabled = false;
                ActivatePieceCubes();
                break;
            default:
                break;
        }
    }

    void ActivatePieceCubes()
    {
        foreach (GameObject cube in cubePieces)
        {
            cube.SetActive(true);

        }
    }
}
