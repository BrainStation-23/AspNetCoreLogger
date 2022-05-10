//using System;
//using System.Runtime.Remoting;
//using System.Runtime.Serialization;

//namespace WebApp.Core.Exceptions
//{
//    [Serializable]
//    public class DuplicateCustomerException : RemotingException
//    {
//        public string ResellerId { get; set; }
//        public string CustomerId { get; set; }

//        public DuplicateCustomerException(string reseller, string customerId, string message)
//            : base(message)
//        {
//            ResellerId = reseller;
//            CustomerId = customerId;
//        }

//        public DuplicateCustomerException(SerializationInfo info, StreamingContext context)
//            : base(info, context)
//        {
//            ResellerId = info.GetString("ResellerId");
//            CustomerId = info.GetString("CustomerId");
//        }

//        public override void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            base.GetObjectData(info, context);

//            info.AddValue("ResellerId", ResellerId);
//            info.AddValue("CustomerId", CustomerId);
//        }
//    }
//}
