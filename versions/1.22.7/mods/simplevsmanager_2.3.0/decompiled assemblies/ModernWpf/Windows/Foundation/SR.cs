using System;

namespace Windows.Foundation
{
	// Token: 0x02000056 RID: 86
	internal static class SR
	{
		// Token: 0x040000CF RID: 207
		public static string Argument_BufferLengthExceedsCapacity = "The specified useful data length exceeds the capacity of this buffer.";

		// Token: 0x040000D0 RID: 208
		public static string Argument_IndexOutOfArrayBounds = "The specified index is out of bounds of the specified array.";

		// Token: 0x040000D1 RID: 209
		public static string Argument_InsufficientArrayElementsAfterOffset = "The specified array does not contain the specified number of elements starting at the specified offset.";

		// Token: 0x040000D2 RID: 210
		public static string Argument_UnexpectedAsyncResult = "The specified AsyncResult does not correspond to any outstanding IO operation.";

		// Token: 0x040000D3 RID: 211
		public static string ArgumentOutOfRange_NeedNonNegNum = "Non-negative number required.";

		// Token: 0x040000D4 RID: 212
		public static string InvalidOperation_CannotCallThisMethodInCurrentState = "The state of this object does not permit invoking this method.";

		// Token: 0x040000D5 RID: 213
		public static string InvalidOperation_CannotGetResultsFromIncompleteOperation = "Cannot call GetResults on this asynchronous info because the underlying operation has not completed.";

		// Token: 0x040000D6 RID: 214
		public static string InvalidOperation_CannotSetCompletionHanlderMoreThanOnce = "The 'Completed' handler delegate cannot be set more than once, but this handler has already been set.";

		// Token: 0x040000D7 RID: 215
		public static string InvalidOperation_CannotSetStreamSizeCannotWrite = "Cannot set the size of this stream because it cannot be written to.";

		// Token: 0x040000D8 RID: 216
		public static string InvalidOperation_IllegalStateChange = "The specified state transition is illegal for the current state of this object.";

		// Token: 0x040000D9 RID: 217
		public static string InvalidOperation_InvalidAsyncCompletion = "The asynchronous operation could not be completed.";

		// Token: 0x040000DA RID: 218
		public static string InvalidOperation_MultipleIOCompletionCallbackInvocation = "A callback for the same asynchronous IO operation was invoked more than once.";

		// Token: 0x040000DB RID: 219
		public static string InvalidOperation_TaskProviderReturnedUnstartedTask = "The Task provider delegate specified for this IAsyncInfo instance returned a Task object that was not started. Task instances must be run immediately upon creation.";

		// Token: 0x040000DC RID: 220
		public static string InvalidOperation_UnexpectedAsyncOperationID = "This AsyncResult or Task corresponds to a different asynchronous operation ID than the one that invoked the completion callback.";

		// Token: 0x040000DD RID: 221
		public static string InvalidOperation_UnstartedTaskSpecified = "The specified underlying Task is not started. Task instances must be run immediately upon creation.";

		// Token: 0x040000DE RID: 222
		public static string IO_General = "An IO error occurred in the Windows runtime system.";

		// Token: 0x040000DF RID: 223
		public static string NotImplemented_NativeRoutineNotFound = "A native library routine was not found: {0}.";

		// Token: 0x040000E0 RID: 224
		public static string NullReference_IOCompletionCallbackCannotProcessNullAsyncInfo = "The Windows Runtime stream that underlies this System.IO.Stream object has invoked an IO completion callback and specified null for the IAsyncInfo instance that describes the completed IO operation. This behavior is not supported because results cannot be retrieved from a null operation. Either the underlying Windows Runtime stream has a faulty implementation, or you are using a Windows Runtime object in an unsupported runtime environment.";

		// Token: 0x040000E1 RID: 225
		public static string NullReference_TaskProviderReturnedNull = "The task provider delegate used to create this asynchronous operation returned null, but a valid Task object was expected.";

		// Token: 0x040000E2 RID: 226
		public static string ObjectDisposed_AsyncInfoIsClosed = "The requested invocation is not permitted because this IAsyncInfo instance has already been closed.";

		// Token: 0x040000E3 RID: 227
		public static string ObjectDisposed_CannotPerformOperation = "The requested operation cannot be performed because this stream has already been disposed.";

		// Token: 0x040000E4 RID: 228
		public static string WinRtCOM_Error = "An error has occurred.";

		// Token: 0x040000E5 RID: 229
		public static string Argument_InvalidSeekOrigin = "Invalid seek origin.";

		// Token: 0x040000E6 RID: 230
		public static string InvalidOperation_SendNotSupportedOnWindowsrTSynchronizationContext = "Send is not supported in the Windows Runtime SynchronizationContext";

		// Token: 0x040000E7 RID: 231
		public static string InvalidAction = "Invalid action value: '{0}'.";
	}
}
