/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 登陆窗体   
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
using UnityEngine.UI;

namespace DemoProject
{
    public class LogonUIForm : BaseUIForm
    {
        public Text TxtLogonName;                           //登陆名称
        public Text TxtLogonNameByBtn;                      //登陆名称(Button)

        public void Awake()
        {
            //定义本窗体的性质(默认数值，可以不写)
            base.CurrentUIType.UIForms_Type = UIFormType.Normal;
            base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;
            base.CurrentUIType.UIForm_LucencyType = UIFormLucenyType.Lucency;
            /* 给按钮注册事件 */
            //RigisterButtonObjectEvent("Btn_OK", LogonSys);
            //Lamda表达式写法
            RigisterButtonObjectEvent("Btn_OK", 
                p=>OpenUIForm(ProConst.SELECT_HERO_FORM)
                );

        }


        public void Start()
        {
            //string strDisplayInfo = LauguageMgr.GetInstance().ShowText("LogonSystem");

            if (TxtLogonName)
            {
                TxtLogonName.text = Show("LogonSystem");
            }
            if (TxtLogonNameByBtn)
            {
                TxtLogonNameByBtn.text = Show("LogonSystem");
            }
        }

    }
}