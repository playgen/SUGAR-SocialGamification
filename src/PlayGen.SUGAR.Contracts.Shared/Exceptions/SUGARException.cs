using System;
//using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

namespace PlayGen.SUGAR.Contracts.Exceptions
{
	/// <summary>
	/// Base class for SUGAR exceptions
	/// </summary>
    public class SUGARException : Exception
    {
		/// <inheritdoc />
		public SUGARException()
		{
		}

		/// <inheritdoc />
		public SUGARException(string message) : base(message)
		{
		}

		/// <inheritdoc />
		public SUGARException(string message, Exception innerException) : base(message, innerException)
		{
		}
        
        // SerializationInfo was not available in .net core
		///// <inheritdoc />
		//protected SUGARException(SerializationInfo info, StreamingContext context) : base(info, context)
		//{
		//}
    }
}
