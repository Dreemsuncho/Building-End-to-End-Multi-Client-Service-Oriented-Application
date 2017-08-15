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
        protected void InvokeSecurityWrappedMethod(Action func)
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;

            using (new OperationContextScope(InnerChannel))
            {
                var header = new MessageHeader<string>(userName);

                OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));

                func.Invoke();
            }
        }

        protected Ty InvokeSecurityWrappedMethod<Ty>(Func<Ty> func)
        {
            string userName = Thread.CurrentPrincipal.Identity.Name;

            using (new OperationContextScope(InnerChannel))
            {
                var header = new MessageHeader<string>(userName);

                OperationContext.Current.OutgoingMessageHeaders.Add(header.GetUntypedHeader("String", "System"));

                return func.Invoke();
            }
        }
    }
}
