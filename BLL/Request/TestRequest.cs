using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Request
{
   public class TestRequest
    {
        public string password { get; set; }
        public string userName { get; set; }
        public string customerId { get; set; }
        public string referenceId { get; set; }
        public List<string> customerRrn { set; get; }
    }

   public class AllCustomerRrn
    {
       public  string RRN { set;get; }
   }
}
