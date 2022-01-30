using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Models;
using Pixelplacement;
using TMPro;
using UnityEngine;

public class DialogsController : Singleton<DialogsController>
{
    public List<Dialog> Dialogs;
    public TextMeshProUGUI Text;

    private Coroutine _showTextRoutine;
    private Animator _anim;

    private Dialog _dialogToShow;
    
    public void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    public static void Show(DialogType type)
    {
        Instance._dialogToShow = Instance.Dialogs.FirstOrDefault(x => x.DialogType == type);
        if (Instance._dialogToShow != null)
        {
            if (Instance._showTextRoutine != null)
                Instance.StopCoroutine(Instance._showTextRoutine);
            Instance.Text.SetText("");
            Instance._anim.Play("RoboDialogs", 0, 0);
        }
    }

    public void OnDialogAnimationEnd()
    {
        if (_dialogToShow != null)
            _showTextRoutine = StartCoroutine(ShowText(_dialogToShow.Text));
    }

    private IEnumerator ShowText(string text)
    {
        var parts = text.Split('$');
        foreach (var part in parts)
        {
            var n = 0;
            while (n <= part.Length)
            {
                Text.SetText(part.Substring(0, n));
                n++;
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(2);
        }
        Instance._anim.Play("RoboDialogsClose", 0, 0);
    }
}
