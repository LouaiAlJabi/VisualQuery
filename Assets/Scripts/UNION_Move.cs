using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JOIN_Move : MonoBehaviour
{
    [SerializeField] private Transform[] _columnsToDestroy;
    [SerializeField] private Transform[] _columnsStaying;
    private Vector3[] _originalDestroyedPos = new Vector3[2];
    private Vector3[] _originalStayingPos = new Vector3[4];
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
            //flash the staying stuff
            foreach (var column in _columnsStaying)
            {
                for (int cell = 0; cell < column.childCount; cell++)
                {
                    column.GetChild(cell).GetComponent<Renderer>().material.DOColor(Color.green, 1.4f)
                        .SetEase(Ease.Flash, 6);
                }
            }

            //destroy the destroyed stuff
            foreach (var column in _columnsToDestroy)
            {
                column.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).SetDelay(1.5f);
            }

            _columnsStaying[2].GetChild(0).transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).SetDelay(1.5f);
            _columnsStaying[3].GetChild(0).transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).SetDelay(1.5f);

            //move the left table to the middle and the right table underneath it
            _columnsStaying[0].DOMove(new Vector3(_columnsStaying[0].transform.position.x + 2f, 0.6f, 0),
                    _cycleLength * 0.5f)
                .SetEase(Ease.InOutSine).SetDelay(1.9f);
            _columnsStaying[1].DOMove(new Vector3(_columnsStaying[1].transform.position.x + 1.2f, 0.6f, 0),
                    _cycleLength * 0.5f)
                .SetEase(Ease.InOutSine).SetDelay(1.9f);
            _columnsStaying[2].DOMove(new Vector3(_columnsStaying[0].transform.position.x + 2f, -0.75f, 0),
                    _cycleLength * 0.5f)
                .SetEase(Ease.InOutSine).SetDelay(1.9f);
            _columnsStaying[3].DOMove(new Vector3(_columnsStaying[1].transform.position.x + 1.2f, -0.75f, 0),
                    _cycleLength * 0.5f)
                .SetEase(Ease.InOutSine).SetDelay(1.9f);

            //scale everything down to fit the screen
            foreach (var column in _columnsStaying)
            {
                column.DOScale(new Vector3(0.7f, 0.7f, 0.1f), 0.5f).SetDelay(1.5f);
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
            //scale staying back
            foreach (var column in _columnsStaying)
            {
                column.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
            }

            //move the tables back to their position
            for (int cell = 0; cell < _columnsStaying.Length; cell++)
            {
                _columnsStaying[cell].DOMove(_originalStayingPos[cell], _cycleLength * 0.5f).SetEase(Ease.InOutSine);
            }

            //scale destroyed back
            foreach (var column in _columnsToDestroy)
            {
                column.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InBounce).SetDelay(1.5f);
            }

            _columnsStaying[2].GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InBounce)
                .SetDelay(1.5f);
            _columnsStaying[3].GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.InBounce)
                .SetDelay(1.5f);

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
