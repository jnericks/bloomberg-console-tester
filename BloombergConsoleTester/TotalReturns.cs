using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BloombergConsoleTester.Core.Models;

namespace BloombergConsoleTester
{
    public class TotalReturns : ICollection<TotalReturn>
    {
        public TotalReturn Head { get; protected set; }
        public int Count { get; protected set; }

        public Index Security { get; protected set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TotalReturns(Index security)
        {
            Security = security;
            Count = 0;
            Head = null;
        }

        public IEnumerator<TotalReturn> GetEnumerator()
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
            Add(new TotalReturn(dateTime, value));
        }

        public void Add(TotalReturn @return)
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

            // Find spot for TotalReturn - keep things ordered by DateTime
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

        public bool Contains(TotalReturn @return)
        {
            return Enumerable.Contains(this, @return);
        }

        public void CopyTo(TotalReturn[] array, int arrayIndex)
        {
            var i = arrayIndex;
            foreach (var element in this)
            {
                array.SetValue(element, i);
                i++;
            }
        }

        public bool Remove(TotalReturn @return)
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