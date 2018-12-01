using UnityEngine;
using System;
using System.Collections;

namespace resTEST
{
    public class ResTester : MonoBehaviour
    {
        void Start()
        {
            ResManager.Instance.Init(delegate
            {
                Loger.Info("mainfest load ok");

                //test0();
                //test1();
                //string a = "LoadingUIROOT";
                //testload(a);
                //string b = "PF_Loading";
                //testload(b);
                //string c = "PF_ChooseServer1";
                //testload(c);
                //string d = "player1_atk_pugong3_Sf_E";
                //testload(d);
                //testStopCoroutine();
                //testmusic();
            });
        }
        //void testmusic()
        //{
        //    AudioMgr.SetPlayBMGBool(true);
        //    AudioMgr.PlayBGM("battle1");
        //}
        IEnumerator wait(float second, Action call)
        {
            yield return new WaitForSeconds(second);
            call();
        }
        void testStopCoroutine()
        {
            string p1 = "daPengDiao_atk_pugong3_Sf_E";
            ResManager.Instance.LoadPrefab(p1, delegate (string assetName, object o)
            {
                GameObject go = o as GameObject;
                GameObject go1 = Instantiate<GameObject>(go);
                go1.SetActive(true);
                Loger.Info("{0} load ok", p1);
            });

            StartCoroutine(wait(0.5f, delegate()
            {
                ResManager.Instance.ClearNoUseRes();
            }));
        }
        void testload(string asset)
        {
            ResManager.Instance.LoadPrefab(asset, delegate (string assetName, object o)
            {
                GameObject go = o as GameObject;
                GameObject go1 = Instantiate<GameObject>(go);
                go1.SetActive(true);
                Loger.Info("{0} load ok", asset);
            });
        }

        /// <summary>
        /// A->B 单向依赖，连续加载A, 只会生成一次AB，多次回调出来，延迟卸载
        /// </summary>
        void test0()
        {
            string p1 = "daPengDiao_atk_pugong3_Sf_E";
            ResManager.Instance.LoadPrefab(p1, delegate (string assetName, object o)
            {
                GameObject go = o as GameObject;
                GameObject go1 = Instantiate<GameObject>(go);
                go1.SetActive(true);
                Loger.Info("{0} load ok", p1);
            });
            ResManager.Instance.LoadPrefab(p1, delegate (string assetName, object o)
            {
                GameObject go = o as GameObject;
                GameObject go1 = Instantiate<GameObject>(go);
                go1.SetActive(true);
                Loger.Info("{0} load ok", p1);
            });
        }
        /// <summary>
        /// A->B 单向依赖，已经加载好A，等待回收，再次加载
        /// </summary>
        void test1()
        {
            string p1 = "daPengDiao_atk_pugong3_Sf_E";
            ResManager.Instance.LoadPrefab(p1, delegate (string assetName, object o)
            {
                GameObject go = o as GameObject;
                GameObject go1 = Instantiate<GameObject>(go);
                go1.SetActive(true);
                Loger.Info("{0} load ok", p1);

                StartCoroutine(wait(0.5f, delegate
                {
                    Loger.Info("wait for 0.5s");

                    ResManager.Instance.LoadPrefab(p1, delegate (string assetName2,object oo)
                    {
                        GameObject goo = oo as GameObject;
                        GameObject go11 = Instantiate<GameObject>(goo);
                        go11.SetActive(true);
                        Loger.Info("{0} load ok", p1);
                    });
                }));
            });
        }


    }
}