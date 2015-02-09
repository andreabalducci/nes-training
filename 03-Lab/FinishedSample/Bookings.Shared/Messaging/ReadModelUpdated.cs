using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookings.Shared.Messaging
{
    public class ReadModelUpdatedMessage
    {
        public enum UpdateAction
        {
            Created,
            Deleted,
            Updated
        }

        public UpdateAction Action { get; set; }
        public object ReadModel { get; set; }
        public string ModelName { get; set; }

        private static string GetModelName<T>() where T : class
        {
            var modelName = typeof(T).Name;
            if (modelName.EndsWith("ReadModel"))
                modelName = modelName.Remove(modelName.Length - "ReadModel".Length);

            return modelName;
        }

        public static ReadModelUpdatedMessage Created<T>(T document) where T : class
        {
            return new ReadModelUpdatedMessage()
            {
                Action = UpdateAction.Created,
                ReadModel = document,
                ModelName = GetModelName<T>()
            };
        }

        public static ReadModelUpdatedMessage Updated<T>(T document) where T : class
        {
            return new ReadModelUpdatedMessage()
            {
                Action = UpdateAction.Updated,
                ReadModel = document,
                ModelName = GetModelName<T>()
            };
        }

        public static ReadModelUpdatedMessage Deleted<T>(T document) where T : class
        {
            return new ReadModelUpdatedMessage()
            {
                Action = UpdateAction.Deleted,
                ReadModel = document,
                ModelName = GetModelName<T>()
            };
        }
    }
}
