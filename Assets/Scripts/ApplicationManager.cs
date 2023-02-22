using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
	[SerializeField] private GameObject _fade;

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	public void NewGame()
	{
		StartCoroutine(ThrowFade());
	}

	private IEnumerator ThrowFade()
	{
		_fade.GetComponent<Animator>().SetTrigger("FadeOut");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("1_Level");
	}
}