public enum EFieldKey {
    IS_KEY,
    IS_NOT_KEY
}

static class EFieldKeyMethods {

    public static bool IsKey(this EFieldKey f) {
        return f == EFieldKey.IS_KEY;
    }
    
}