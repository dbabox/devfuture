using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    /// <summary>
    /// 树节点实体
    /// </summary>
    public interface ITvaNode:ICloneable
    {
        
        /// <summary>
        /// 必须的，主键
        /// </summary>
        string TNA_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点主键
        /// </summary>
        string TNA_PID
        {
            get;
            set;
        }
        /// <summary>
        /// 节点名称
        /// </summary>
        string TNA_Text
        {
            get;
            set;
        }
        /// <summary>
        /// 节点逻辑ID
        /// </summary>
        string TNA_LogicId
        {
            get;
            set;
        }

        /// <summary>
        /// 节点所属的列表
        /// </summary>
        IList<ITvaNode> Owner
        {
            get;
            set;

        }

        /// <summary>
        /// 内置层序号，从0开始。
        /// </summary>
        int TNA_Level
        {
            get;
            set;
        }

        /// <summary>
        /// 内置索引，从0开始
        /// </summary>
        int TNA_Index
        {
            get;

        }


        /// <summary>
        /// 自定义图标
        /// </summary>
        System.Drawing.Image Icon
        {
            get;
            set;
        }

        /// <summary>
        /// 是否被选中，供CheckBox使用
        /// </summary>
        bool IsChecked
        {
            get;
            set;
        }

        //TODO:20091217 添加DataRow到ITvaNode对象之间的转换接口
        //TODO:Adapter类应该仅仅封装必要的数据访问功能。

        //应该实现一个通用的DataRowTvaNode类，此类简单的wrapper一个DataRow成为一个ITvaNode
        
       
        /// <summary>
        /// 将Datarow的值赋值到Model。
        /// </summary>
        /// <param name="row"></param>
        void Row2Model(DataRow row);
        /// <summary>
        /// 将Model的值赋值给DataRow。
        /// </summary>
        /// <param name="row"></param>
        void Model2Row(DataRow row);
    }
}
