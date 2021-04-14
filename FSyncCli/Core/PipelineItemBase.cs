using System;
using System.Collections.Generic;

namespace FSyncCli.Core
{
    public class PipelineItemBase
    {
        private readonly Dictionary<Guid, object> _metadata = new Dictionary<Guid, object>();

        public virtual void SetItemValue<T>(T value)
        {
            var key = typeof(T).GUID;

            _metadata[key] = value;
        }

        public virtual T GetItemValue<T>()
        {
            var key = typeof(T).GUID;
            var isAdded = _metadata.TryGetValue(key, out var resValue);

            return isAdded ? (T)resValue : default;
        }
    }
}