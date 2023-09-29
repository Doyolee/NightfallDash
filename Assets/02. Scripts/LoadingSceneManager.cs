using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene; // �������� �ε��� ���� �̸��� �����ϴ� ����
    [SerializeField] Image progressBar; // �ε� ���൵�� ǥ���� �̹��� UI

    private void Start()
    {
        StartCoroutine(LoadScene()); // ���� �񵿱������� �ε��ϴ� �ڷ�ƾ�� �����մϴ�.
        Time.timeScale = 0.1f;
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName; // ������ �ε��� ���� �̸��� �����մϴ�.
        SceneManager.LoadScene("LoadingScene"); // �ε� ���� �ε��մϴ�.
    }

    IEnumerator LoadScene()
    {
        yield return null; // 1�������� ��ٸ��ϴ�. (�ٸ� ������Ʈ���� Start �Լ��� ������ ���� �������� �ð��� Ȯ��)

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // ���� ���� �񵿱������� �ε��մϴ�.
        op.allowSceneActivation = false; // �ε��� ���� �� �ڵ����� ���� Ȱ��ȭ���� �ʵ��� �����մϴ�.

        float timer = 0.0f; // ��� �ð��� �����ϴ� ����



        while (!op.isDone) // �� �ε尡 ������ �ʾ����� �ݺ��մϴ�.
        {
            yield return null; // ���� �����ӱ��� ��ٸ��ϴ�.

            timer += Time.deltaTime; // ��� �ð��� �����մϴ�.

            // ������ �ε� ���൵�� ������ ����ϴ�.
            // ���� �ε� ���൵(op.progress)���� ������ ����ǵ��� ������ ��(0.01f)�� �����ݴϴ�.
            float slowProgress = Mathf.Min(op.progress + 0.01f, 1.0f);

            if (slowProgress < 0.9f) // �� �ε尡 90% �̸��̸�
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, slowProgress, timer); // �ε� ���൵�� �ε巴�� ������ŵ�ϴ�.
                if (progressBar.fillAmount >= op.progress) // ���൵�� �� �ε� ���൵�� ����������
                {
                    timer = 0f; // ��� �ð��� �ʱ�ȭ�մϴ�.
                }
            }
            else // �� �ε尡 90% �̻��̸�
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); // �ε� ���൵�� �ε巴�� 100%�� ����ϴ�.
                if (progressBar.fillAmount == 1.0f) // ���൵�� 100%�� �Ǹ�
                {
                    op.allowSceneActivation = true; // ���� Ȱ��ȭ�Ͽ� ���� ������ �Ѿ�ϴ�.
                    yield break; // �ڷ�ƾ�� �����մϴ�.
                }
            }
        }
    }
}
