using System.Collections.Generic;
using UnityEngine;

public class PlayerPath : MonoBehaviour
{
    float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 1)
        {
            _timer = 0;
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);
            int counter = 0;
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.CompareTag(TagList.obstacle))
                {
                    counter++;
                }
            }
            if (counter == 0)
            {
                //path is fully cleared
                //GameController.win?.Invoke();
            }
        }
    }

    public void SetXScale(float xScale)
    {
        Vector3 scaleBuffer = transform.localScale;
        scaleBuffer.x = xScale;
        transform.localScale = scaleBuffer;
    }
}