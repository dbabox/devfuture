using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace TreeEditor.Core
{
    /// <summary>
    /// ���ڵ�ʵ��
    /// </summary>
    public interface ITvaNode:ICloneable
    {
        
        /// <summary>
        /// ����ģ�����
        /// </summary>
        string TNA_ID
        {
            get;
            set;
        }

        /// <summary>
        /// ���ڵ�����
        /// </summary>
        string TNA_PID
        {
            get;
            set;
        }
        /// <summary>
        /// �ڵ�����
        /// </summary>
        string TNA_Text
        {
            get;
            set;
        }
        /// <summary>
        /// �ڵ��߼�ID
        /// </summary>
        string TNA_LogicId
        {
            get;
            set;
        }

        /// <summary>
        /// �ڵ��������б�
        /// </summary>
        IList<ITvaNode> Owner
        {
            get;
            set;

        }

        /// <summary>
        /// ���ò���ţ���0��ʼ��
        /// </summary>
        int TNA_Level
        {
            get;
            set;
        }

        /// <summary>
        /// ������������0��ʼ
        /// </summary>
        int TNA_Index
        {
            get;

        }


        /// <summary>
        /// �Զ���ͼ��
        /// </summary>
        System.Drawing.Image Icon
        {
            get;
            set;
        }

        /// <summary>
        /// �Ƿ�ѡ�У���CheckBoxʹ��
        /// </summary>
        bool IsChecked
        {
            get;
            set;
        }

        //TODO:20091217 ���DataRow��ITvaNode����֮���ת���ӿ�
        //TODO:Adapter��Ӧ�ý�����װ��Ҫ�����ݷ��ʹ��ܡ�

        //Ӧ��ʵ��һ��ͨ�õ�DataRowTvaNode�࣬����򵥵�wrapperһ��DataRow��Ϊһ��ITvaNode
        
       
        /// <summary>
        /// ��Datarow��ֵ��ֵ��Model��
        /// </summary>
        /// <param name="row"></param>
        void Row2Model(DataRow row);
        /// <summary>
        /// ��Model��ֵ��ֵ��DataRow��
        /// </summary>
        /// <param name="row"></param>
        void Model2Row(DataRow row);
    }
}
