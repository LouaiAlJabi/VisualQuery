using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WHERE_Move : MonoBehaviour
{
    [SerializeField] private Transform[] _columnsToDestroy;
    [SerializeField] private Transform[] _columnsStaying;
    private Vector3[] _originalDestroyedPos = new Vector3[3];
    private Vector3[] _originalStayingPos = new Vector3[3];
    [SerializeField] private float _cycleLength = 2;
    private bool _isDestroyed = false;
    //destroyed row index is 2
    
    void Start()
    {
        Button destroyingButton = GameObject.Find("ExecuteButton").GetComponent<Button>();
        destroyingButton.onClick.AddListener(DestroyColumn);
        
        Button revivingButton = GameObject.Find("ReviveButton").GetComponent<Button>();
        revivingButton.onClick.AddListener(ReviveColumn);
        
        Button menuButton = GameObject.Find("BackToMenu").GetComponent<Button>();
        menuButton.onClick.AddListener(menuScene);

        for (int cell = 0; cell < _columnsToDestroy.Length; cell++)
        {
            _originalDestroyedPos[cell] = _columnsToDestroy[cell].transform.position;
        }
        
        for (int cell = 0; cell < _columnsStaying.Length; cell++)
        {
            _originalStayingPos[cell] = _columnsStaying[cell].transform.position;
        }
        
    }

    private void DestroyColumn()
    {
        if (_isDestroyed == false)
        {
            //flash the two staying rows
            foreach (var column in _columnsStaying)
            {
                for (int cell = 0; cell < column.childCount; cell++)
                {
                    if (cell == 1 || cell == 3)
                    {
                        column.GetChild(cell).GetComponent<Renderer>().material.DOColor(Color.green, 0.8f)
                            .SetEase(Ease.Flash, 4);
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            //move the destroyed rows to the right and destroy it
            foreach (var column in _columnsToDestroy)
            {
                column.GetChild(2)
                    .DOMove(
                        new Vector3(column.GetChild(2).gameObject.transform.position.x + 3,
                            column.GetChild(2).gameObject.transform.position.y,
                            column.GetChild(2).gameObject.transform.position.z), _cycleLength * 0.5f)
                    .SetEase(Ease.InOutSine).SetDelay(0.9f).OnComplete(
                        () => { column.GetChild(2).DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce); });
            }

            //move the staying rows together to the middle
            foreach (var column in _columnsStaying)
            {
                for (int cell = 0; cell < column.childCount; cell++)
                {
                    if (cell == 0 || cell == 1)
                    {
                        column.GetChild(cell)
                            .DOMove(
                                new Vector3(column.GetChild(cell).transform.position.x,
                                    column.GetChild(cell).transform.position.y - 0.6f,
                                    column.GetChild(cell).transform.position.z), _cycleLength * 0.5f)
                            .SetEase(Ease.InOutSine).SetDelay(1.3f);
                    }
                    else if (cell == 3)
                    {
                        column.GetChild(cell)
                            .DOMove(
                                new Vector3(column.GetChild(cell).transform.position.x,
                                    column.GetChild(cell).transform.position.y + 0.4f,
                                    column.GetChild(cell).transform.position.z), _cycleLength * 0.5f)
                            .SetEase(Ease.InOutSine).SetDelay(1.3f);
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            _isDestroyed = true;
        }
        else
        {
            return;
        }
    }
    

    private void ReviveColumn()
    {
        if (_isDestroyed == true)
        {
            //move the columns to where they were
            foreach (var column in _columnsStaying)
            {
                for (int cell = 0; cell < column.childCount; cell++)
                {
                    if (cell == 0 || cell == 1)
                    {
                        column.GetChild(cell)
                            .DOMove(
                                new Vector3(column.GetChild(cell).transform.position.x,
                                    column.GetChild(cell).transform.position.y + 0.6f,
                                    column.GetChild(cell).transform.position.z), _cycleLength * 0.5f)
                            .SetEase(Ease.InOutSine);
                    }
                    else if (cell == 3)
                    {
                        column.GetChild(cell)
                            .DOMove(
                                new Vector3(column.GetChild(cell).transform.position.x,
                                    column.GetChild(cell).transform.position.y - 0.4f,
                                    column.GetChild(cell).transform.position.z), _cycleLength * 0.5f)
                            .SetEase(Ease.InOutSine);
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            //revive the destroyed block and move them back
            foreach (var column in _columnsToDestroy)
            {
                
                column.GetChild(2).DOScale(new Vector3(1,1,1), 0.5f).SetEase(Ease.InBounce)
                    .OnComplete(() =>
                        {
                            column.GetChild(2).DOMove(
                                    new Vector3(column.GetChild(2).gameObject.transform.position.x - 3,
                                        column.GetChild(2).gameObject.transform.position.y,
                                        column.GetChild(2).gameObject.transform.position.z), _cycleLength * 0.5f)
                                .SetEase(Ease.InOutSine);
                        });
            }

            _isDestroyed = false;
        }
        else
        {
            return;
        }
    }

    void menuScene()
    {
        SceneManager.LoadScene("Main_Screen");
    }
}
