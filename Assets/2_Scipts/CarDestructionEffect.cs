using System.Collections;
using UnityEngine;

public class CarDestructionEffect : MonoBehaviour
{
    [Tooltip("�ڵ��� ���־�(����) ������Ʈ")]
    public GameObject carVisual;

    [Tooltip("������ ����Ʈ ������Ʈ (ParticleSystem ��, ��Ȱ��ȭ ���·� �μ���)")]
    public GameObject breakEffectObject;

    [Tooltip("����Ʈ ��� �� ���־� ����� �г� ��������� ������(��)")]
    public float effectToPanelDelay = 0.5f;

  

    public void PlayDestructionEffect(System.Action onEffectEnd = null)
    {
        // 1. ���־� �����
        if (carVisual != null)
            carVisual.SetActive(false);

        // 2. ����Ʈ ���
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

        // 3. ������ �� �ݹ�(��: �г� ����)
        StartCoroutine(EffectDelayCoroutine(onEffectEnd));
    }

    private IEnumerator EffectDelayCoroutine(System.Action onEffectEnd)
    {
        yield return new WaitForSecondsRealtime(effectToPanelDelay);
        onEffectEnd?.Invoke();
    }
}
