using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SELECT_Move : MonoBehaviour
{
    [SerializeField] private Transform[] _columnsToDestroy;
    [SerializeField] private Transform[] _columnsStaying;
    private Vector3[] _originalDestroyedPos = new Vector3[2];
    private Vector3[] _originalStayingPos = new Vector3[1];
    [SerializeField] private float _cycleLength = 2;
    private bool _isDestroyed = false;
    void Start()
    {
        Button destroyingButton = GameObject.Find("ExecuteButton").GetComponent<Button>();
        destroyingButton.onClick.AddListener(DestroyColumn);
        
        
        Button revivingButton = GameObject.Find("ReviveButton").GetComponent<Button>();
        revivingButton.onClick.AddListener(ReviveColumn);

        Button menuButton = GameObject.Find("BackToMenu").GetComponent<Button>();
        menuButton.onClick.AddListener(mainScene);
        
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
            foreach (var column in _columnsStaying)
            {
                for (int i = 0; i < column.childCount; i++)
                {
                    column.GetChild(i).GetComponent<Renderer>().material.DOColor(Color.green, 0.8f).SetEase(Ease.Flash,4);
                }
            }
            
            
            foreach (var column in _columnsToDestroy)
            {
                column.DOMove(new Vector3(column.gameObject.transform.position.x + 2, 0, 0), _cycleLength * 0.5f)
                    .SetEase(Ease.InOutSine).SetDelay(0.9f).OnComplete(
                        () => { column.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce); });
            }

            foreach (var column in _columnsStaying)
            {
                
                column.DOMove(new Vector3(1, 0, 0), _cycleLength * 0.5f).SetEase(Ease.InOutSine).SetDelay(1f);
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
            
            foreach (var column in _columnsToDestroy)
            {
                column.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InBounce)
                    .OnComplete(() =>
                    {
                        for (int cell = 0; cell < _columnsStaying.Length; cell++)
                        {
                            _columnsStaying[cell].DOMove(_originalStayingPos[cell], _cycleLength * 0.5f)
                                .SetEase(Ease.InOutSine);
                        }

                        column.DOMove(new Vector3(column.gameObject.transform.position.x - 2, 0, 0),
                            _cycleLength * 0.5f).SetEase(Ease.InOutSine);

                    });
            }
            

            _isDestroyed = false;
        }
        else
        {
            return;
        }
    }

    void mainScene()
    {
        SceneManager.LoadScene("Main_Screen");
    }
}

