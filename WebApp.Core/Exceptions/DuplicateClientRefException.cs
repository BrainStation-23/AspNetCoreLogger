//using System;
//using System.Runtime.Remoting;
//using System.Runtime.Serialization;

//namespace WebApp.Core.Exceptions
//{
//    /// <summary>
//    /// Exception raised when duplication client reference is attemted use in a transaction
//    /// </summary>
//    [Serializable]
//    public class DuplicateClientRefException : RemotingException
//    {
//        public string ResellerId { get; set; }
//        public string TaskId { get; set; }
//        public string ClientRef { get; set; }

//        public DuplicateClientRefException(string reseller, string clientRef, string taskId, string message)
//            : base(message)
//        {
//            ResellerId = reseller;
//            TaskId = taskId;
//            ClientRef = clientRef;
//        }

//        public DuplicateClientRefException(SerializationInfo info, StreamingContext context)
//            : base(info, context)
//        {
//            ResellerId = info.GetString("ResellerId");
//            TaskId = info.GetString("TaskId");
//            ClientRef = info.GetString("ClientRef");
//        }

//        public override void GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            base.GetObjectData(info, context);

//            info.AddValue("ResellerId", ResellerId);
//            info.AddValue("TaskId", TaskId);
//            info.AddValue("ClientRef", ClientRef);
//        }
//    }
//}
