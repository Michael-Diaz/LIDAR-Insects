using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lidar_Module : MonoBehaviour
{
    private GameObject spiderBody;
    public GameObject swarmBoundry;

    private LayerMask walls;
    private LayerMask markers;

    // Start is called before the first frame update
    void Start()
    {
        spiderBody = GameObject.Find("Spider_PH");

        walls = LayerMask.GetMask("Barrier");
        markers = LayerMask.GetMask("Bound");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = spiderBody.transform.position + (Vector3.up * 0.5f);
        transform.Rotate(Vector3.up * Time.deltaTime * 50.0f);

        for (int i = 0; i < 9; i++)
        {
            Vector3 dir = new Vector3(Mathf.Cos(40 * i * Mathf.Deg2Rad), 0.0f, Mathf.Sin(40 * i * Mathf.Deg2Rad));

            RaycastHit hit;
            Ray beam = new Ray(transform.position, transform.TransformDirection(dir));

            Debug.DrawRay(transform.position, transform.TransformDirection(dir) * 4.5f, Color.green); //DEBUG

            if (Physics.Raycast(beam, out hit, 4.5f))
            {
                Collider[] markerCheck = Physics.OverlapSphere(hit.point, 1.0f, markers);
                if (markerCheck.Length == 0)
                    Instantiate(swarmBoundry, hit.point, Quaternion.identity);
            }
        }
    }
}
