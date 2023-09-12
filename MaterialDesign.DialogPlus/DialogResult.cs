using System.Threading;
using System.Threading.Tasks;

namespace Com.Josh2112.Libs.MaterialDesign.DialogPlus
{
    public class DialogResult<T>
    {
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

        public void Set( T result ) => tcs.TrySetResult( result );

        public Task<T> WaitAsync( CancellationToken? ct = null ) =>
            ct.HasValue ? tcs.Task.WaitAsync( ct.Value ) : tcs.Task;
    }

    public interface IHasDialogResult { }

    public interface IHasDialogResult<T> : IHasDialogResult
    {
        DialogResult<T> Result { get; }
    }
}
