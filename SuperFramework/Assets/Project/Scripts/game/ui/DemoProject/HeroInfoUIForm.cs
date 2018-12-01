/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 英雄信息显示窗体
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
	public class HeroInfoUIForm : BaseUIForm {


		void Awake () 
        {
		    //窗体性质
            CurrentUIType.UIForms_Type = UIFormType.Fixed;  //固定在主窗体上面显示
            
        }
		
	}
}