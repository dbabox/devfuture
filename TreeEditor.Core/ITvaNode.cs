using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
