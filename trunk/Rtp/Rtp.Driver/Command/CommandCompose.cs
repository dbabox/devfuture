using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ��ϲ�����/��������ִ����������ͬ��ָ��磺00 A4 00 02 3F 00/10 01/00 19
    /// Ҫ�������ϵĲ��ֿ���ȫ�滻ǰ��Ĳ��֣����滻�Ĳ���Ҫ��ȡ�
    /// �ò������Լ�������Ĭ�ϵģ�����cosָ��ʹ��/�滻������2011-2-15
    /// </summary>
    public class CommandCompose:ICommand
    {
        

        
        public CommandCompose()
        {
            
        }

        #region ICommand ��Ա

        public bool execute(string commandBody, CommandContext ctx)
        {
            if (commandBody.Contains("/")) //��������������MAC
            {
                
            }
            return false;
        }

        public string CommandName
        {
            get { return "/"; }
        }

        #endregion
    }
}
