using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BloombergConsoleTester
{
    public class BBIndexReturnCollection : ICollection<BBIndexReturn>
    {
        public BBIndexReturn Head { get; protected set; }
        public int Count { get; protected set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public BBIndexReturnCollection()
        {
            Count = 0;
            Head = null;
        }

        public IEnumerator<BBIndexReturn> GetEnumerator()
        {
            var current = Head;
            while (current != null)
            {
                yield return current;
                current = current._next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(DateTime dateTime, decimal? value = 0)
        {
            Add(new BBIndexReturn(dateTime, value));
        }

        public void Add(BBIndexReturn @return)
        {
            Count++;

            // Clean the node
            @return._previous = null;
            @return._next = null;

            // Empty Collection
            if (Head == null)
            {
                Head = @return;
                return;
            }

            // Find spot for BBIndexReturn - keep things ordered by DateTime
            var current = Head;

            // Is this the new Head?
            if (@return.DateTime < current.DateTime)
            {
                Head = @return;
                @return._next = current;
                current._previous = @return;
                return;
            }

            while (current.DateTime < @return.DateTime)
            {
                if (current._next == null)
                {
                    current._next = @return;
                    @return._previous = current;
                    return;
                }
                current = current._next;
            }

            var next = current;
            current = current._previous;
            current._next = @return;
            @return._previous = current;
            @return._next = next;
            next._previous = @return;
        }

        public void Clear()
        {
            Count = 0;
            Head = null;
        }

        public bool Contains(BBIndexReturn @return)
        {
            return Enumerable.Contains(this, @return);
        }

        public void CopyTo(BBIndexReturn[] array, int arrayIndex)
        {
            var i = arrayIndex;
            foreach (var element in this)
            {
                array.SetValue(element, i);
                i++;
            }
        }

        public bool Remove(BBIndexReturn @return)
        {
            if (Contains(@return))
            {
                Count--;
                @return._previous._next = @return._next;
                return true;
            }
            return false;
        }
    }
}