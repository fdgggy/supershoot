/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 主城窗体
 *    Description: 
 *           功能：
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;

namespace DemoProject
{
    public class MainCityUIForm : BaseUIForm
    {
		public void Awake () 
        {
	        //窗体性质
		    CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;

		    //事件注册
            RigisterButtonObjectEvent("BtnMarket",
                p => OpenUIForm(ProConst.MARKET_UIFORM)           
                );

        }
		
	}
}