using System.Collections;
using UnityEngine;

public class Printer : MonoBehaviour
{
    [SerializeField] private Transform printingPoint;
    [SerializeField] private float speed;
    [SerializeField] private float printingInterval;
    [SerializeField] private float moveUntil;
    [SerializeField] private float intervalBetweenPrints;
    [SerializeField] private GameObject[] graphs;

    bool turnOn = false;

    public void TogglePrinter()
    {
        if (turnOn)
        {
            if(AudioManager.Instance != null)
                AudioManager.Instance.PlayPrinterOff();
            turnOn = false;
        }
        else
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayPrinterOn();
            turnOn = true;
            GameManager.Instance.OnPrinterOn?.Invoke();
        }
    }

    public void PrintGraphs()
    {
        StartCoroutine(PrintGraphsCoroutine());
    }

    IEnumerator PrintGraphsCoroutine()
    {
        foreach (GameObject graph in graphs)
        {
            GameObject go = Instantiate(graph, printingPoint.position, printingPoint.rotation, transform);
            go.GetComponent<Rigidbody>().isKinematic = true;

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayPrintingLoop();

            while (go.transform.localPosition.y > moveUntil)
            {
                go.transform.localPosition -= go.transform.up * speed * Time.deltaTime;
                yield return new WaitForSeconds(printingInterval);
            }

            go.GetComponent<Rigidbody>().isKinematic = false;
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopPrintingLoop();

            yield return new WaitForSeconds(intervalBetweenPrints);
        }
    }
}
