using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RecourcePlayModeTest {
    [Test]
    public void TestExceDataLoad()
    {
        //ResManager.Instance.Init(() => {
        //    ExcelDataManager.Instance.Init(() =>
        //    {
        //        Debug.Log("ExcelDataManager load over!");
        //        ExcelExample exd = ExcelDataManager.Instance.GetExcel(ExcelType.ExcelExample) as ExcelExample;
        //        ExcelExampleData d = exd.QueryByID(8);
        //        Debug.Log("Id:"+d.Id+" Name:"+d.Name+ " Strengh:"+d.Strength+" Dif:"+d.Difficulty);
        //    });
        //});
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator RecourcePlayModeTestWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        ResManager.Instance.Init(() => {
            //ExcelDataManager.Instance.Init(() =>
            //{
            //    Debug.Log("ExcelDataManager load over!");
            //    ExcelExample exd = ExcelDataManager.Instance.GetExcel(ExcelType.ExcelExample) as ExcelExample;
            //    ExcelExampleData d = exd.QueryByID(8);
            //    Debug.Log("Id:" + d.Id + " Name:" + d.Name + " Strengh:" + d.Strength + " Dif:" + d.Difficulty);
            //});
        });

        yield return new WaitForSeconds(1f);
	}

    [UnityTest]
    public IEnumerator RecourcePlayModeTestLoadScene()
    {
        ResManager.Instance.Init(() => {
            //SceneMgr.Instance.EnterScene(SceneType.Login);
        });

        yield return new WaitForSeconds(50f);
    }
}
