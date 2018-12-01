/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： PRG 游戏“选择角色”窗体 
 *    Description: 
 *           功能： 选择英雄窗体
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
    public class SelectHeroUIForm : BaseUIForm
    {

        public void Awake()
        {
            //Log.SyncLogCatchToFile();
            //窗体的性质
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;

            //注册进入主城的事件
            RigisterButtonObjectEvent("BtnConfirm",
                p =>
                {
                    OpenUIForm(ProConst.MAIN_CITY_UIFORM);
                    OpenUIForm(ProConst.HERO_INFO_UIFORM);
                }

                );

            //注册返回上一个页面
            RigisterButtonObjectEvent("BtnClose",
                m=>CloseUIForm()
                );
        }
    }
}