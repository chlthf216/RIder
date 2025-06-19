using System.Collections;
using UnityEngine;

public class CarDestructionEffect : MonoBehaviour
{
    [Tooltip("자동차 비주얼(외형) 오브젝트")]
    public GameObject carVisual;

    [Tooltip("터지는 이펙트 오브젝트 (ParticleSystem 등, 비활성화 상태로 두세요)")]
    public GameObject breakEffectObject;

    [Tooltip("이펙트 재생 후 비주얼 숨기고 패널 띄우기까지의 딜레이(초)")]
    public float effectToPanelDelay = 0.5f;

  

    public void PlayDestructionEffect(System.Action onEffectEnd = null)
    {
        // 1. 비주얼 숨기기
        if (carVisual != null)
            carVisual.SetActive(false);

        // 2. 이펙트 재생
        if (breakEffectObject != null)
        {
            if (breakEffectObject.TryGetComponent<ParticleSystem>(out var ps))
            {
                breakEffectObject.SetActive(true);
                ps.Play();
            }
            else
            {
                breakEffectObject.SetActive(true);
            }
        }

        // 3. 딜레이 후 콜백(예: 패널 띄우기)
        StartCoroutine(EffectDelayCoroutine(onEffectEnd));
    }

    private IEnumerator EffectDelayCoroutine(System.Action onEffectEnd)
    {
        yield return new WaitForSecondsRealtime(effectToPanelDelay);
        onEffectEnd?.Invoke();
    }
}
