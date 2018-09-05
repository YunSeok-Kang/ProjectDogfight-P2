using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenIndicator : MonoBehaviour
{
    //public Image OnScreenSprite; // can use gameobject with spriterenderer, or reference a Sprite here, and create a new gameobject, add a sprite renderer to it.
    public Image enemyImage;
    public Image itemImage;
    public List<Image> indicators = new List<Image>();
    public Canvas canvas;
    public float offset;


   // public List<GameObject> objects;
    //public Image[] onScreenSprites; // sprite object pooling
    //public Image[] offScreenSprites;

   // public Vector3 objectPoolPos;
    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) * .5f;

    // Use this for initialization
    void Start()
    {
        //onScreenSprites = new GameObject[objects.Count];
        //offScreenSprites = new GameObject[objects.Count];
        //Debug.Log ("Center: " + screenCenter);
        //create the sprites at load time
    /*    for (int i = 0; i < objects.Count; i++)
        {
            //onScreenSprites[i] = Instantiate(OnScreenSprite,objectPoolPos,this.transform.rotation) as GameObject;
            //offScreenSprites[i] = Instantiate(OffScreenSprite,objectPoolPos,this.transform.rotation) as GameObject;
        }*/
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlaceIndicators();
        
        StartCoroutine(ClearIndicators());
    }

    void PlaceIndicators()
    {
        VoxObject[] allObjects = FindObjectsOfType<VoxObject>();
        List<VoxObject> objects = new List<VoxObject>();
        foreach (VoxObject ob in allObjects)
        {
            //비히클이면
            if(ob.GetComponent<Vehicle>())
            {
                //플레이어는 무시
                if(ob.CompareTag("Player"))
                {
                    continue;
                }
                else
                {
                    objects.Add(ob);
                }
            }
            //아이템이면
            else if (ob.GetComponent<Item>())
            {
                objects.Add(ob);
            }
        }

        //GameObject[] objects = GameObject.FindObjectsOfType(typeof(AI)) as GameObject[]; //find objects by type (might ahve to find by gameobejct, and then filter for AI)
        //go through all the objects we care about
        foreach (VoxObject go in objects)
        {
            // 그 오브젝트의 스크린 상에서 위치 구하기.
            Vector3 screenpos = Camera.main.WorldToScreenPoint(go.transform.position);

            //if onscreen
            if (screenpos.z >= 0 && screenpos.x < Screen.width && screenpos.x > 0 && screenpos.y < Screen.height && screenpos.y > 0)
            //if (screenpos.z > 0 && screenpos.x < Screen.width && screenpos.x > 0 && screenpos.y < Screen.height && screenpos.y > 0)
            {

                //int index = objects.IndexOf(go); // get an index to use for our sprite, based on the objects index in the objects list // we should make an objectpool manager
                //OnScreenSprite.rectTransform.position = screenpos;
                //Debug.Log("OnScreen: " + screenpos);
            }
            else
            {
                PlaceOffscreen(screenpos, go);
            }
        }

    }

    void PlaceOffscreen(Vector3 screenpos, VoxObject target)
    {
        float x = screenpos.x;
        float y = screenpos.y;

        //스크린 뒤로 넘어가면
        //2D 라 이건 필요없다.
        /*if (screenpos.z < 0)
        {
            screenpos = -screenpos;
        }*/

        //x 좌표가 오른쪽을 넘어가면
        if (screenpos.x > Screen.width)
        {
            x = Screen.width - offset;
        }
        //x좌표가 왼족을 넘어가면
        if (screenpos.x < 0)
        {
            x = offset;
        }
        //y좌표 위쪽을 넘어가면
        if (screenpos.y > Screen.height)
        {
            y = Screen.height - offset;
        }
        //y좌표 아래쪽을 넘어가면
        if (screenpos.y < 0)
        {
            y = offset;
        }


        var ind = CreateIndicator(target);

        ind.rectTransform.position = new Vector3(x, y,0);

        //화살표들의 각도는 화면 중간을 중심으로 정해져야함
        //그러나 screen pos들은 Bottom left를 중심으로 되어있음.
        //그러므로 보간해줘야함.
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
        float angle = Mathf.Atan2(screenCenter.y - screenpos.y, screenCenter.x -screenpos.x);
        //angle -= 90 * Mathf.Deg2Rad;

        ind.rectTransform.localRotation= Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
    }
    private Image CreateIndicator(VoxObject target)
    {
        Image ind;

        if (target.CompareTag("Enemy"))
        {
            ind = Instantiate(enemyImage, canvas.transform);
        }
        else
        {
            ind = Instantiate(itemImage, canvas.transform);
        }


        indicators.Add(ind);

        return ind;
    }
    private IEnumerator ClearIndicators()
    {
        yield return new WaitForEndOfFrame();

        while(indicators.Count !=0)
        {
            var obj = indicators[indicators.Count - 1];
            indicators.Remove(obj);
            Destroy(obj.gameObject);
        }
    }
}