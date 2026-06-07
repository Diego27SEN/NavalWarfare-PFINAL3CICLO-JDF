using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizeGame : MonoBehaviour
{
    public void ChangeLanguage(string code)
    {
        if(LocalizationSettings.InitializationOperation.IsDone == false)
        {
            return;
        }

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];

            if(locale.Identifier == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                Debug.Log("Idioma Cambiado");
                return;
            }
        }

    }
}
