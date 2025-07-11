using TMPro;
using UnityEngine;

public class ClickObj : MonoBehaviour
{
    bool move;
    Vector2 randomVector;

    private void Update()
    {
        if (!move) return;
        transform.Translate(randomVector * Time.deltaTime);
    }

    public void StartMotion(int scoreIncrease)
    {
        transform.localPosition = Vector2.zero;
        GetComponent<TextMeshProUGUI>().text = "+" + scoreIncrease;
        randomVector = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        move = true;
        //GetComponent<Animation>().Play("clickTextFade");
        GetComponent<Animation>().Play();
    }
    public void StopMotion()
    {
        move = false;
    }
}
