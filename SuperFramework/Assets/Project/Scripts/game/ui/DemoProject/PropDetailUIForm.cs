/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 道具详细信息窗体 
 *    Description: 
 *           功能： 显示各种道具信息
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

namespace DemoProject
{
	public class PropDetailUIForm : BaseUIForm
	{
	    public Text TxtName;                                //窗体显示名称

        public override UIType GetUIType()
        {
            //窗体的性质
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange;
            CurrentUIType.UIForm_LucencyType = UIFormLucenyType.Translucence;
            CurrentUIType.UIForms_Type = UIFormType.PopUp;  //弹出窗体
            return CurrentUIType;
        }

        void Awake () 
        {
            /* 按钮的注册  */
            RigisterButtonObjectEvent("BtnClose",
                p=>CloseUIForm()
                );

            /*  接受信息   */
            ReceiveMessage("Props", 
                p =>
                {
                    if (TxtName)
                    {
                        string[] strArray = p.Values as string[];
                        TxtName.text = strArray[0];
                        //print("测试道具的详细信息： "+strArray[1]);
                    }
                }
           );

        }//Awake_end
		
	}
}