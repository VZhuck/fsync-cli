using System;
using System.Collections.Generic;

namespace FSyncCli.Core
{
    public class PipelineItemBase
    {
        private readonly Dictionary<Guid, object> _itemProperties = new Dictionary<Guid, object>();

        public virtual void SetItemProperty<T>(T value)
        {
            var key = typeof(T).GUID;

            _itemProperties[key] = value;
        }

        public virtual T GetItemProperty<T>()
        {
            var key = typeof(T).GUID;
            var isAdded = _itemProperties.TryGetValue(key, out var resValue);

            return isAdded ? (T)resValue : default;
        }
    }

    public class PipelineItemBase<T> : PipelineItemBase
    {
        public PipelineItemBase(T descriptor)
        {
            Descriptor = descriptor;
        }

        public T Descriptor
        {
            get => GetItemProperty<T>();
            private set => SetItemProperty(value);
        }
    }
}