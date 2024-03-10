public class Result<T, K> {

    private readonly Optional<T> ok;
    private readonly Optional<K> err;

    private Result(T ok, K err) {
        this.ok = Optional<T>.Some(ok);
        this.err = Optional<K>.Some(err);
    }

    private Result(T ok) {
        this.ok = Optional<T>.Some(ok);
        this.err = Optional<K>.None();
    }

    private Result(K err) {
        this.ok = Optional<T>.None();
        this.err = Optional<K>.Some(err);
    }

    public static Result<T, K> OK(T ok) {
        return new Result<T, K>(ok);
    }

     public static Result<T, K> ERR(K err) {
        return new Result<T, K>(err);
    }

    public bool IsOk() {
        return this.ok.IsSome();
    }

    public bool IsErr() {
        return this.err.IsSome();
    }

    public Optional<T> Ok() {
        return this.ok;
    }

    public Optional<K> Err() {
        return this.err;
    }

}