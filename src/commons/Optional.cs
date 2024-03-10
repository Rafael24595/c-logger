public class Optional<T> {

    private readonly T? element;

    public Optional() {
    }

    public Optional(T element) {
        this.element = element;
    }

    public static Optional<T> None() {
        return new();
    }

    public static Optional<T> Some(T element) {
        return new(element);
    }

    public bool IsSome() {
        return this.element != null;
    }

    public bool IsNone() {
        return !this.IsSome();
    }

    public T Unwrap() {
        if(this.element == null) {
            throw new NullReferenceException();
        }
        return this.element;
    }

}