using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.Command
{
    /// <summary>
    /// ϵͳ����
    /// SAMSLOT ##
    /// eg:SAMSLOT C0
    /// �趨��ǰҪ������SAM����ۺš�
    /// </summary>
    public class CommandSamSlot:ICommand
    {
        
        
        #region ICommand ��Ա

        public bool execute(string commandBody,CommandContext ctx)
        {

            string slotStr = Utility.GetSubStringBetweenChars(commandBody, '(', ')');
            byte slot = 0;
            if (Byte.TryParse(slotStr,
                System.Globalization.NumberStyles.HexNumber, null, out slot))
            {
                //ת����ȷ�ĸ�ʽ��
                if (ctx.Rfid.SAM_SetSlot(slot) == 0)
                {
                    ctx.ReportMessage("SYS>> Current SAM Slot is 0x{0,2:X2}", ctx.Rfid.CurrentSamSlot);
                    return true;
                }
                else
                {
                    ctx.ReportMessage("ERR>>Command ERROR: {0} ", commandBody);
                    return false;
                }
            }
            ctx.ReportMessage("ERR>>Command ERROR: {0} ת��16����ʧ�ܣ�", commandBody);
            return false;
        }

        public string CommandName
        {
            get { return "SYS<SAMSLOT"; }
        }

         

        #endregion
    }
}
