using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Common.ServiceModel
{
    public class UserClientBase<T> : ClientBase<T> where T : class
    {
        public UserClientBase()
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;
            var header = new MessageHeader<string>(userName);

            var operationContext = new OperationContextScope(InnerChannel);

            OperationContext.Current
                .OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));
        }
    }
}
