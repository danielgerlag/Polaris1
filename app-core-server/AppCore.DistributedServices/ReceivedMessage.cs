using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DistributedServices
{
    public class ReceivedMessage
    {        

        public int Payload { get; set; }


        public ReceivedMessage(int payload)
        {
            Payload = payload;            
        }

        
    }
}
