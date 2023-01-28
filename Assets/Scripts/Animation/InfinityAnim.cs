using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InfinityAnim : MonoBehaviour
{

    [SerializeField] private float _dragDuration = 1f;
    [SerializeField] private List<RectTransform> _jumpPoints;
    [SerializeField] private RectTransform _handImage;

    [SerializeField] private float _jumpPower;

    private Vector3 _initialPosition;

    void Start()
    {
        _initialPosition = _jumpPoints[0].localPosition;
        PlayAnimation();
    }


    public void PlayAnimation()
    {
        _handImage.localPosition = _initialPosition;

        Sequence _handSequence = DOTween.Sequence();

        for (int i = 0; i < _jumpPoints.Count; i++)
        {
            _handSequence
            .Append(_handImage.DOLocalJump(_jumpPoints[i].localPosition, _jumpPower, 1, _dragDuration).SetEase(Ease.Linear));
        }


        _handSequence
            .SetEase(Ease.Unset)
            .SetLoops(-1);
    }

}
