using Crm.Link.Suitcrm.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Link.Suitcrm.Tools.GateAway
{
    public class ContactGateAway : GateAwayBase<ContactModel>, IContactGateAway
    {
        protected override string Module => "Contacts";
        public ContactGateAway(
            IHttpClientFactory httpClientFactory,
            TokenProvider tokenProvider) : base(tokenProvider)
        {
            HttpClient = httpClientFactory.CreateClient("Crm");
        }
    }
}
