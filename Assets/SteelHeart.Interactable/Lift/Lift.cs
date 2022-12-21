using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SteelHeart.Interactable
{
    public class Lift : MonoBehaviour, IStateSwitcher
    {
        [SerializeField] private float _speed = 1f;

        [SerializeField] private AbstractActivatedSource _callToDownButton;
        [SerializeField] private AbstractActivatedSource _callToUpButton;
        [SerializeField] private AbstractActivatedSource _moveButton;

        [SerializeField] private Transform _upPoint;
        [SerializeField] private Transform _downPoint;
        [SerializeField] private Transform _platform;

        private List<BaseLiftState> _states;
        private BaseLiftState _currentState;

        private float _timeToEndPoint;
        private float _elapsedTime;

        private void Start()
        {
            _states = new List<BaseLiftState>()
            {
                new MiddleIdleState(this, _speed, _upPoint, _downPoint, _platform),
                new MovingDownState(this, _speed, _upPoint, _downPoint, _platform),
                new DownedState(this, _speed, _upPoint, _downPoint, _platform),
            };

            _currentState = _states[0];
            _currentState.Start();
        }

        private void Awake()
        {
            _callToUpButton.Activated += OnCallToUp;
            _callToDownButton.Activated += OnCallToDown;
            _moveButton.Activated += OnCallMove;

            _elapsedTime = 0;

            float distance = Vector3.Distance(_downPoint.position, _upPoint.position);
            _timeToEndPoint = distance / _speed;
        }

        private void OnCallMove() =>
            _currentState.OnCallMove();

        private void OnCallToDown() =>
            _currentState.OnCallToDown();

        private void OnCallToUp() =>
            _currentState.OnCallToUp();

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                _currentState.OnCallMove();

            if (Input.GetKey(KeyCode.UpArrow))
                _currentState.OnCallToUp();

            if (Input.GetKey(KeyCode.DownArrow))
                _currentState.OnCallToDown();
        }

        private void GoUp()
        {
            _elapsedTime += Time.deltaTime;

            float elapsedPercentage = _elapsedTime / _timeToEndPoint;

            if (true)
                _platform.position = Vector3.Lerp(_downPoint.position, _upPoint.position, elapsedPercentage);
            else
                _platform.position = Vector3.Lerp(_upPoint.position, _downPoint.position, elapsedPercentage);

            if (elapsedPercentage >= 1)
            {
                _elapsedTime = 0;
            }
        }

        private void SetupPath(Transform target)
        {
            _elapsedTime = 0;
            float distance = Vector3.Distance(_platform.position, target.position);
            _timeToEndPoint = distance / _speed;
        }

        public void Switch<T>() where T : BaseLiftState
        {
            _currentState.Stop();
            _currentState = _states.FirstOrDefault(s => s is T);
            _currentState.Start();
        }
    }
}