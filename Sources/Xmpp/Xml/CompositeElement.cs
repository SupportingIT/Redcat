using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Redcat.Xmpp.Xml
{
    public abstract class CompositeElement<T> : Element
    {
        private ICollection<T> items;

        #region Constructors

        protected CompositeElement(string name)
            : base(name)
        { }

        #endregion

        public ICollection<T> Items
        {
            get { return items ?? (items = CreateElementsCollection()); }
        }

        protected virtual ICollection<T> CreateElementsCollection()
        {
            return new List<T>();
        }

        public void AddItem(T item)
        {
            Items.Add(item);
        }

        public void AddItems(params T[] items)
        {
            foreach (T item in items) AddItem(item);
        }

        protected override void WritePayload(XmlWriter writer)
        {
            base.WritePayload(writer);
            WriteElements(writer);
        }

        protected abstract void WriteElements(XmlWriter writer);

        public override bool Equals(object obj)
        {
            CompositeElement<T> element = obj as CompositeElement<T>;
            if (element == null) return false;

            return Equals(element);
        }

        public bool Equals(CompositeElement<T> element)
        {
            return base.Equals(element) && AreChildEquals(element);
        }

        private bool AreChildEquals(CompositeElement<T> compositeElement)
        {
            return Items.SequenceEqual(compositeElement.Items);
        }
    }

    public abstract class CompositeElement : CompositeElement<Element>
    {
        protected CompositeElement(string name) : base(name)
        { }

        protected override void WriteElements(XmlWriter writer)
        {
            foreach (Element element in Items) element.Write(writer);
        }

        public Element Find(string elementName)
        {
            if (elementName == null) throw new ArgumentNullException("elementName");
            if (elementName == "") throw new ArgumentOutOfRangeException("elementName");

            return Items.OfType<Element>().FirstOrDefault(e => e.Name == elementName);
        }

        public TElement Find<TElement>(string elementName) where TElement : Element
        {
            return Find(elementName) as TElement;
        }
    }
}
