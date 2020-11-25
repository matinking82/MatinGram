using MatinGram.Application.Services.Messages.Commands.AddNewMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Interfaces.FacadPatterns
{
    public interface IMessagesFacad
    {
        IAddNewMessageService AddNewMessageService { get; }
    }
}
