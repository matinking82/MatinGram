using MatinGram.Application.Interfaces;
using MatinGram.Application.Interfaces.FacadPatterns;
using MatinGram.Application.Services.Messages.Commands.AddNewMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Messages.FacadPattern
{
    public class MessagesFacad: IMessagesFacad
    {
        private readonly IDataBaseContext _context;

        public MessagesFacad(IDataBaseContext context)
        {
            _context = context;
        }


        private IAddNewMessageService _addNewMessageService;
        public IAddNewMessageService AddNewMessageService 
        { 
            get 
            {
                if (_addNewMessageService == null)
                {
                    _addNewMessageService = new AddNewMessageService(_context);
                }

                return _addNewMessageService;
            } 
        }
    }
}
