using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Vsx
{

   [Serializable]
   public class TextFileGeneratorException : Exception
   {
      public TextFileGeneratorException()
      {
         Line = -1;
         Column = -1;
      }

      public TextFileGeneratorException(int line, string message) : this(line, -1, message) { }

      public TextFileGeneratorException(int line, int column, string message)
         : base(message)
      {
         Line = line;
         Column = column;
      }

      public TextFileGeneratorException(int line, int column, string message, Exception inner)
         : base(message, inner)
      {
         Line = line;
         Column = column;
      }

      public TextFileGeneratorException(string message) : this(-1, -1, message) { }
      public TextFileGeneratorException(string message, Exception inner) : this(-1, -1, message, inner) { }

      protected TextFileGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         Line = info.GetInt32(nameof(Line));
         Column = info.GetInt32(nameof(Column));
      }

      public int Line
      {
         get;
      }

      public int Column
      {
         get;
      }

      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
         info.AddValue(nameof(Line), Line);
         info.AddValue(nameof(Column), Column);
      }
   }
}
