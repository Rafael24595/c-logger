public enum EFieldKey {
    IS_KEY,
    IS_NOT_KEY
}

static class EFieldKeyMethods {

    public static bool IsKey(this EFieldKey f) {
        return f == EFieldKey.IS_KEY;
    }

    public static bool IsNotKey(this EFieldKey f) {
        return !IsKey(f);
    }
    
}