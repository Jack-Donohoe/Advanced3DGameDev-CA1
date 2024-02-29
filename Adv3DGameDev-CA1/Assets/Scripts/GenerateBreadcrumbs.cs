using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBreadcrumbs : MonoBehaviour
{
    private Vector3 prevPosition, currentPosition;
    private float counter = 0;
    
    public GameObject BC;
    
    // Start is called before the first frame update
    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DropBreadCrumb();
    }

    void DropBreadCrumb()
    {
        currentPosition = transform.position;
        float distance = Vector3.Distance(prevPosition, currentPosition);

        if (distance > 1)
        {
            prevPosition = currentPosition;

            GameObject breadcrumb = Instantiate(BC, currentPosition, Quaternion.identity);
            breadcrumb.name = "BC" + counter;

            counter++;
            Destroy(breadcrumb, 3f);
        }
    }
}
