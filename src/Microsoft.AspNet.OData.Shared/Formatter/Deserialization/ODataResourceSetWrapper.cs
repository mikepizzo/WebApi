// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.OData;

namespace Microsoft.AspNet.OData.Formatter.Deserialization
{
    /// <summary>
    /// Encapsulates an <see cref="ODataResourceSet"/> and the <see cref="ODataResource"/>'s that are part of it.
    /// </summary>
    public sealed class ODataResourceSetWrapper : ODataResourceSetWrapperBase
    {
        private IList<ODataResourceWrapper> resources;
        /// <summary>
        /// Initializes a new instance of <see cref="ODataResourceSetWrapper"/>.
        /// </summary>
        /// <param name="item">The wrapped item.</param>        
        public ODataResourceSetWrapper(ODataResourceSet item)
            : base(item)
        {
            ResourceSet = item;
            resources = new List<ODataResourceWrapper>();
            Items = new ODataItemList(this.resources);
        }

        internal override ResourceSetType ResourceSetType => ResourceSetType.ResourceSet;

        /// <summary>
        /// Gets the wrapped <see cref="ODataResourceSet"/>.
        /// </summary>
        public ODataResourceSet ResourceSet { get; }

        /// <summary>
        /// Gets the nested resources of this ResourceSet.
        /// </summary>
        [Obsolete("Resources is Obsolete. Please use Items.")]
        public IList<ODataResourceWrapper> Resources { get { return resources; } }

        /// <summary>
        /// Gets the list of resources of this ResourceSet
        /// </summary>
        public override IList<ODataItemBase> Items { get; }

        /// <summary>
        /// Private class to expose a List of ODataItemBase over a List of ODataResourceWrappers
        /// </summary>
        private class ODataItemList : IList<ODataItemBase>
        {
            private IList<ODataResourceWrapper> items;
            public ODataItemList(IList<ODataResourceWrapper> items)
            {
                this.items = items;
            }

            public ODataItemBase this[int index]
            {
                get
                {
                    return this.items[index];
                }
                set
                {
                    ODataResourceWrapper resourceValue = ValidateResourceWrapper(value);
                    this.items[index] = resourceValue;
                }
            }

            public int Count => this.items.Count;

            public bool IsReadOnly => this.items.IsReadOnly;

            public void Add(ODataItemBase item)
            {
                ODataResourceWrapper resourceValue = ValidateResourceWrapper(item);
                this.items.Add(resourceValue);
            }

            public void Clear()
            {
                this.items.Clear();
            }

            public bool Contains(ODataItemBase item)
            {
                return this.items.Contains(item as ODataResourceWrapper);
            }

            public void CopyTo(ODataItemBase[] array, int arrayIndex)
            {
                for (int index = 0; index < array.Length; index++)
                {
                    ODataResourceWrapper resourceValue = ValidateResourceWrapper(array[index]);
                    this.items.Insert(arrayIndex + index, resourceValue);
                }
            }

            public IEnumerator<ODataItemBase> GetEnumerator()
            {
                return this.items.GetEnumerator();
            }

            public int IndexOf(ODataItemBase item)
            {
                return this.IndexOf(item);
            }

            public void Insert(int index, ODataItemBase item)
            {
                ODataResourceWrapper resourceValue = ValidateResourceWrapper(item);
                this.items.Insert(index, resourceValue);
            }

            public bool Remove(ODataItemBase item)
            {
                return this.items.Remove(item as ODataResourceWrapper);
            }

            public void RemoveAt(int index)
            {
                this.items.RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.items.GetEnumerator();
            }

            private ODataResourceWrapper ValidateResourceWrapper(ODataItemBase item)
            {
                ODataResourceWrapper resourceWrapper = item as ODataResourceWrapper;
                if (item == null)
                {
                    // todo: make this a real exception
                    throw new Exception("can only ad resourcewrappers to ResourceSetWrapper");
                }
                return resourceWrapper;
            }
        }

    }
}
