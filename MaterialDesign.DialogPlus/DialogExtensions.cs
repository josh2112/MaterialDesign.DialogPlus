using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Com.Josh2112.Libs.MaterialDesign.DialogPlus
{
    public static class DialogHostWindowExtensions
    {
        public static Task<T> ShowDialogForResultAsync<T>( this Window window, IHasDialogResult<T> dialog, CancellationToken? cancelToken = null )
        {
            var dialogHost = window.VisualDepthFirstTraversal()
                .FirstOrDefault( v => v is MaterialDesignThemes.Wpf.DialogHost ) as MaterialDesignThemes.Wpf.DialogHost ??
                throw new InvalidOperationException( "Unable to find a DialogHost in visual tree" );
            return ShowDialogForResultAsync( dialogHost, dialog, cancelToken );
        }

        /// <summary>
        /// Finds the Window's DialogHost and shows the given dialog, asynchronously waiting for the
        /// dialog to close itself (via setting its task completion source) or for it to be canceled
        /// via the CancellationToken.
        /// </summary>
        /// <typeparam name="T">type of result expected from the dialog</typeparam>
        /// <param name="window">the Window where the dialog should be shown</param>
        /// <param name="dialog">the dialog content</param>
        /// <param name="cancelToken">an optional CancellationToken</param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException">if the dialog was canceled</exception>
        /// <exception cref="InvalidOperationException">if the window does not contain a DialogHost</exception>
        public static async Task<T> ShowDialogForResultAsync<T>( this MaterialDesignThemes.Wpf.DialogHost dialogHost, IHasDialogResult<T> dialog, CancellationToken? cancelToken = null )
        {
            // Use the non-blocking OpenDialogCommand to show the dialog
            MaterialDesignThemes.Wpf.DialogHost.OpenDialogCommand.Execute( dialog, dialogHost );

            try
            {
                // Wait for the dialog's Result task completion source to be set, or
                // for it to be canceled from the outside. The caller can catch 
                // OperationCanceledException to see if the dialog was canceled.
                return await dialog.Result.WaitAsync( cancelToken );
            }
            finally
            {
                // Close the dialog whether it was canceled or completed.
                dialogHost.IsOpen = false;
            }
        }
    }

    internal static class VisualTreeExtensions
    {
        internal static IEnumerable<DependencyObject> VisualDepthFirstTraversal( this DependencyObject visual )
        {
            if( visual == null )
                throw new ArgumentNullException( "visual" );

            yield return visual;

            for( int i = 0; i < VisualTreeHelper.GetChildrenCount( visual ); i++ )
            {
                foreach( DependencyObject child in VisualTreeHelper.GetChild( visual, i ).VisualDepthFirstTraversal() )
                    yield return child;
            }
        }
    }

#if NET6_0_OR_GREATER

// .NET 6.0 includes Task<T>.WaitAsync( CancellationToken ). For the .NET framework,
// re-implement it here.

#else
    internal static class TaskExtensions
    {
        internal static Task<T> WaitAsync<T>( this Task<T> task, CancellationToken cancellationToken )
        {
            if( task.IsCompleted || !cancellationToken.CanBeCanceled )
                return task;

            if( cancellationToken.IsCancellationRequested )
                return Task.FromCanceled<T>( cancellationToken );

            return DoWaitAsync( task, cancellationToken );
        }

        private static async Task<T> DoWaitAsync<T>( Task<T> task, CancellationToken cancellationToken )
        {
            using( var cancelTaskSource = new CancellationTokenTaskSource<T>( cancellationToken ) )
                return await (await Task.WhenAny( task, cancelTaskSource.Task ).ConfigureAwait( false )).ConfigureAwait( false );
        }

        public sealed class CancellationTokenTaskSource<T> : IDisposable
        {
            private readonly IDisposable registration;

            public Task<T> Task { get; private set; }

            public CancellationTokenTaskSource( CancellationToken cancellationToken )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    Task = System.Threading.Tasks.Task.FromCanceled<T>( cancellationToken );
                    return;
                }

                var tcs = new TaskCompletionSource<T>();
                registration = cancellationToken.Register( () => tcs.TrySetCanceled( cancellationToken ), useSynchronizationContext: false );
                Task = tcs.Task;
            }

            public void Dispose() => registration?.Dispose();
        }
    }
    
#endif // NET6_0_OR_GREATER
}
