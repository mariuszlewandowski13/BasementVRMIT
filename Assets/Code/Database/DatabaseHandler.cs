using System.Collections;
using UnityEngine;

public class DatabaseHandler: MonoBehaviour{

    private string message;

    public void ExequteSQL(string sql)   // WITH CHAT MG
    {
        WWWForm form = new WWWForm();
        form.AddField("sqlCommand", sql);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "ExecuteSQL.php", form);
        StartCoroutine(request(w));
    }

    IEnumerator request(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }

        Debug.Log(message);
    }
}
