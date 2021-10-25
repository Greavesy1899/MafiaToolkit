using System;
using System.Collections.Generic;
using Utils.Extensions;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public enum MT_MessageType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }

    /*
     *  Object which tracks errors logged by the validation process.
     */
    public class MT_ValidationTracker
    {
        private Dictionary<IValidator, List<string>> Messages;
        private Stack<IValidator> ObjectStack;
        private Dictionary<IValidator, List<IValidator>> LinkedObjects;

        public MT_ValidationTracker()
        {
            Messages = new Dictionary<IValidator, List<string>>();
            ObjectStack = new Stack<IValidator>();
            LinkedObjects = new Dictionary<IValidator, List<IValidator>>();
        }

        public void Setup(IValidator ValidationObject)
        {
            if (ObjectStack.Count > 0)
            {
                // Try add linked objects
                IValidator TopMostObject = ObjectStack.Peek();
                if (TopMostObject != null)
                {
                    LinkedObjects[TopMostObject].Add(ValidationObject);
                }
            }

            // Add new message array (and construct linked object)
            Messages.Add(ValidationObject, new List<string>());
            LinkedObjects.Add(ValidationObject, new List<IValidator>());

            ObjectStack.Push(ValidationObject);
        }

        public void PopObject(IValidator ValidationObject)
        {
            IValidator TopObject = ObjectStack.Peek();
            if (TopObject == ValidationObject)
            {
                ObjectStack.Pop();

                // add if doesn't exist
                if (!Messages.ContainsKey(TopObject))
                {
                    Messages.Add(TopObject, new List<string>());
                }
            }
        }

        public void AddMessage(IValidator CallerObject, MT_MessageType MessageType, string Format, params object[] Args)
        {
            string Message = string.Format(Format, Args);
            AddMessage(CallerObject, MessageType, Message);
        }

        public void AddMessage(IValidator CallerObject, MT_MessageType MessageType, string Text)
        {
            string FinalMessage = string.Format("[{0}] - {1}", MessageType.ToString(), Text);

            Messages.TryGet(CallerObject)?.Add(FinalMessage);
        }

        public bool IsObjectValid(IValidator ObjectToCheck)
        {
            List<string> ObjectMsgs = InternalGetObjectMessages(ObjectToCheck);

            return ObjectMsgs.Count == 0;
        }

        public int GetMessageCount()
        {
            int CurrentCount = 0;
            foreach(var Entry in Messages)
            {
                CurrentCount += Entry.Value.Count;
            }

            return CurrentCount;
        }
        public List<string> GetObjectMessages(IValidator ObjectToCheck)
        {
            return InternalGetObjectMessages(ObjectToCheck);
        }

        private List<string> InternalGetObjectMessages(IValidator ObjectToCheck)
        {
            List<string> OutMessageList = new List<string>();
            OutMessageList.AddRange(Messages.TryGet(ObjectToCheck));

            foreach (IValidator ObjectEntry in LinkedObjects[ObjectToCheck])
            {
                OutMessageList.AddRange(InternalGetObjectMessages(ObjectEntry));
            }

            return OutMessageList;
        }
    }
}
