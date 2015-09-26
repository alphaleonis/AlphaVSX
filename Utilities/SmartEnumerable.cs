using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx
{
   /// <summary>
   /// A wrapper for IEnumerable&lt;T&gt; that provides easy access to information about the index of
   /// the entry being enumerated and whether its the first or last entry.
   /// </summary>
   /// <remarks>
   /// Note that this enumerator will enumerate the underlying enumerator one step further than what
   /// the client reads from this enumerator. This is normally not a problem, but it might be
   /// important to consider in some occasions.
   /// </remarks>
   /// <typeparam name="T">The type to iterate over.</typeparam>
   public class SmartEnumerable<T> : IEnumerable<SmartEnumerable<T>.Entry>
   {
      #region Private Fields

      private readonly IEnumerable<T> m_enumerable;

      #endregion

      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="sequence">Collection to wrap. Must not be null.</param>
      public SmartEnumerable(IEnumerable<T> sequence)
      {
         if (sequence == null)
            throw new ArgumentNullException(nameof(sequence));
         m_enumerable = sequence;
      }

      /// <summary>
      /// Returns an enumeration of Entry objects, each of which knows
      /// whether it is the first/last of the enumeration, as well as the
      /// current value and next/previous values.
      /// </summary>
      public IEnumerator<Entry> GetEnumerator()
      {
         using (IEnumerator<T> enumerator = m_enumerable.GetEnumerator())
         {
            if (!enumerator.MoveNext())
            {
               yield break;
            }
            bool isFirst = true;
            bool isLast = false;
            int index = 0;

            T current = enumerator.Current;
            isLast = !enumerator.MoveNext();
            var entry = new Entry(isFirst, isLast, current, index++);
            isFirst = false;

            while (!isLast)
            {
               T next = enumerator.Current;
               isLast = !enumerator.MoveNext();
               var entry2 = new Entry(isFirst, isLast, next, index++);
               yield return entry;
               entry = entry2;
            }

            yield return entry;
         }
      }

      /// <summary>
      /// Non-generic form of GetEnumerator.
      /// </summary>
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <summary>
      /// Represents each entry returned within a collection,
      /// containing the value and whether it is the first and/or
      /// the last entry in the collection's. enumeration
      /// </summary>
      public struct Entry
      {
         #region Properties
         /// <summary>
         /// The value of the entry.
         /// </summary>
         public T Value { get; }

         /// <summary>
         /// Whether or not this entry is first in the collection's enumeration.
         /// </summary>
         public bool IsFirst { get; }

         /// <summary>
         /// Whether or not this entry is last in the collection's enumeration.
         /// </summary>
         public bool IsLast { get; }

         /// <summary>
         /// The 0-based index of this entry (i.e. how many entries have been returned before this one)
         /// </summary>
         public int Index { get; }

         #endregion

         #region Constructors

         internal Entry(bool isFirst, bool isLast, T value, int index)
         {
            IsFirst = isFirst;
            IsLast = isLast;
            Value = value;
            Index = index;
         }

         #endregion

         #region Methods            

         public override string ToString()
         {
            return Value?.ToString();
         }

         #endregion

      }
   }

   /// <summary>
   /// Static class to make creation easier. If possible though, use the extension
   /// method in SmartEnumerableExt.
   /// </summary>
   public static class SmartEnumerableExtensions
   {
      /// <summary>
      /// Wraps this sequence as a <see cref="SmartEnumerable{T}"/>.
      /// </summary>
      /// <typeparam name="T">The type of the items in the sequence.</typeparam>
      /// <param name="source">The source sequence.</param>
      /// <returns>A new <see cref="SmartEnumerable{T}"/> of the appropriate type</returns>
      public static SmartEnumerable<T> AsSmartEnumerable<T>(this IEnumerable<T> source)
      {
         return new SmartEnumerable<T>(source);
      }
   }
}
