using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene; // 다음으로 로드할 씬의 이름을 저장하는 변수
    [SerializeField] Image progressBar; // 로딩 진행도를 표시할 이미지 UI

    private void Start()
    {
        StartCoroutine(LoadScene()); // 씬을 비동기적으로 로드하는 코루틴을 시작합니다.
        Time.timeScale = 0.1f;
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName; // 다음에 로드할 씬의 이름을 설정합니다.
        SceneManager.LoadScene("LoadingScene"); // 로딩 씬을 로드합니다.
    }

    IEnumerator LoadScene()
    {
        yield return null; // 1프레임을 기다립니다. (다른 오브젝트들이 Start 함수를 끝내고 씬이 렌더링될 시간을 확보)

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // 다음 씬을 비동기적으로 로드합니다.
        op.allowSceneActivation = false; // 로딩이 끝난 후 자동으로 씬을 활성화하지 않도록 설정합니다.

        float timer = 0.0f; // 경과 시간을 저장하는 변수



        while (!op.isDone) // 씬 로드가 끝나지 않았으면 반복합니다.
        {
            yield return null; // 다음 프레임까지 기다립니다.

            timer += Time.deltaTime; // 경과 시간을 누적합니다.

            // 강제로 로딩 진행도를 느리게 만듭니다.
            // 실제 로딩 진행도(op.progress)보다 느리게 진행되도록 임의의 값(0.01f)을 더해줍니다.
            float slowProgress = Mathf.Min(op.progress + 0.01f, 1.0f);

            if (slowProgress < 0.9f) // 씬 로드가 90% 미만이면
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, slowProgress, timer); // 로딩 진행도를 부드럽게 증가시킵니다.
                if (progressBar.fillAmount >= op.progress) // 진행도가 씬 로드 진행도를 따라잡으면
                {
                    timer = 0f; // 경과 시간을 초기화합니다.
                }
            }
            else // 씬 로드가 90% 이상이면
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); // 로딩 진행도를 부드럽게 100%로 만듭니다.
                if (progressBar.fillAmount == 1.0f) // 진행도가 100%가 되면
                {
                    op.allowSceneActivation = true; // 씬을 활성화하여 다음 씬으로 넘어갑니다.
                    yield break; // 코루틴을 종료합니다.
                }
            }
        }
    }
}
