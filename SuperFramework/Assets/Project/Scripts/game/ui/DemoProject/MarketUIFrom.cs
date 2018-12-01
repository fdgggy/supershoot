/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： “商城窗体”   
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
    public class MarketUIFrom : BaseUIForm
    {
        public override UIType GetUIType()
        {
            //窗体性质
            CurrentUIType.UIForm_LucencyType = UIFormLucenyType.Translucence;
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange;
            CurrentUIType.UIForms_Type = UIFormType.PopUp;  //弹出窗体
            return CurrentUIType;
        }
        
        void Awake ()
        {
            //注册按钮事件：退出
            RigisterButtonObjectEvent("Btn_Close",
                P=> CloseUIForm()                
                );
            //注册道具事件：神杖 
            RigisterButtonObjectEvent("BtnTicket",
                P =>
                {
                    //打开子窗体
                    OpenUIForm(ProConst.PRO_DETAIL_UIFORM);
                    //传递数据
                    string[] strArray = new string[] { "神杖详情", "神杖详细介绍。。。" };
                    SendMessage("Props", "ticket", strArray);
                }
                );

            //注册道具事件：战靴 
            RigisterButtonObjectEvent("BtnShoe",
                P =>
                {
                    //打开子窗体
                    OpenUIForm(ProConst.PRO_DETAIL_UIFORM);
                    //传递数据
                    string[] strArray = new string[] { "战靴详情", "战靴详细介绍。。。" };
                    SendMessage("Props", "shoes", strArray);
                }
                );

            //注册道具事件：盔甲 
            RigisterButtonObjectEvent("BtnCloth",
                P =>
                {
                    //打开子窗体
                    OpenUIForm(ProConst.PRO_DETAIL_UIFORM);
                    //传递数据
                    string[] strArray = new string[] { "盔甲详情", "盔甲详细介绍。。。" };
                    SendMessage("Props", "cloth", strArray);
                }
                );
        }
		
	}
}