/*
 * ���������ʶ������
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.RfidReader;

namespace Rtp.Driver.Command
{
    public interface ICommand
    {
        bool execute(string commandBody, CommandContext ctx);
        string CommandName { get;}
       
           
    }
}
